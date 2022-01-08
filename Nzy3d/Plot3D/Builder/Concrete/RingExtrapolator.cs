using Nzy3d.Colors;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder.Concrete
{
	public class RingExtrapolator : OrthonormalTessellator
	{
		internal float _ringMax;
		internal ColorMapper _cmap;
		internal Color _factor;

		internal RingTessellator _interpolator;
		public RingExtrapolator(float ringMax, ColorMapper cmap, Color factor)
		{
			_ringMax = ringMax;
			_cmap = cmap;
			_factor = factor;
			_interpolator = new RingTessellator(0, _ringMax, _cmap, _factor);
		}

		private RingExtrapolator()
		{
			throw new Exception("Forbidden constructor");
		}

		public override AbstractComposite Build(float[] x, float[] y, float[] z)
		{
			SetData(x, y, z);
			var s = new Shape();
			s.Add(GetExtrapolatedRingPolygons());
			return s;
		}

		public List<Polygon> GetExtrapolatedRingPolygons()
		{
			//backup current coords and extrapolate
			float[] xbackup = (float[])X.Clone();
			float[] ybackup = (float[])Y.Clone();
			float[,] zbackup = (float[,])Z.Clone();

			// compute required extrapolation
			float sstep = X[1] - X[0];
			int nstep = X.Length;
			const int ENLARGE = 2;
			int required = (int)Math.Ceiling((_ringMax * 2 - sstep * nstep) / sstep);
			required = required < 0 ? ENLARGE : required + ENLARGE;

			if (required > 0)
			{
				Extrapolate(required);
			}

			_interpolator.X = X;
			_interpolator.Y = Y;
			_interpolator.Z = Z;
			List<Polygon> polygons = _interpolator.GetInterpolatedRingPolygons();

			// get back to previous grid
			X = xbackup;
			Y = ybackup;
			Z = zbackup;
			return polygons;
		}

		/// <summary>
		/// Add extrapolated points on the grid. If the grid is too small for extrapolation, the arrays
		/// are maximized.
		/// </summary>
		/// <param name="n"></param>
		public void Extrapolate(int n)
		{
			float[] xnew = new float[X.Length + n * 2];
			float[] ynew = new float[Y.Length + n * 2];
			float[,] znew = new float[X.Length + n * 2, Y.Length + n * 2];

			// assume x and y grid are allready sorted and create new grids
			float xmin = X[0];
			float xmax = X[X.Length - 1];
			float xgap = X[1] - X[0];
			float ymin = Y[0];
			float ymax = Y[Y.Length - 1];
			float ygap = Y[1] - Y[0];

			for (int i = 0; i <= xnew.Length - 1; i++)
			{
				// --- x grid ---
				// fill before
				if (i < n)
				{
					xnew[i] = xmin - (n - i) * xgap;
					// copy content
				}
				else if (i >= n && i < X.Length + n)
				{
					xnew[i] = X[i - n];
					// fill after
				}
				else if (i >= X.Length + n)
				{
					xnew[i] = xmax + (i - (X.Length + n) + 1) * xgap;
				}

				// --- y grid ---
				for (int j = 0; j <= ynew.Length - 1; j++)
				{
					// fill before
					if (j < n)
					{
						ynew[j] = ymin - (n - j) * ygap;
						znew[i, j] = float.NaN;
						// copy content
					}
					else if (j >= n && j < (Y.Length + n))
					{
						ynew[j] = Y[j - n];
						// copy z grid
						if (i >= n && i < X.Length + n)
						{
							znew[i, j] = Z[i - n, j - n];
						}
						else
						{
							znew[i, j] = float.NaN;
						}
						// fill after
					}
					else if (j >= (Y.Length + n))
					{
						ynew[j] = ymax + (j - (Y.Length + n) + 1) * ygap;
						znew[i, j] = float.NaN;
					}
				}
			}

			// extrapolation
			float olddiameter = xgap * (X.Length) / 2;
			float newdiameter = xgap * (X.Length - 1 + n * 2) / 2;
			olddiameter *= olddiameter;
			newdiameter *= newdiameter;
			int xmiddle = (xnew.Length - 1) / 2;

			// assume it is an uneven grid
			int ymiddle = (ynew.Length - 1) / 2;

			// assume it is an uneven grid		
			// start from center, and add extrapolated values iteratively on each quadrant
			for (int i = xmiddle; i <= xnew.Length - 1; i++)
			{
				for (int j = ymiddle; j <= ynew.Length - 1; j++)
				{
					float sqrad = xnew[i] * xnew[i] + ynew[j] * ynew[j];

					// distance to center
					if (sqrad < olddiameter)
					{
						// ignore existing values
						continue;
					}

					if (sqrad < newdiameter && sqrad >= olddiameter)
					{
						// ignore existing values
						int xopp = i - 2 * (i - xmiddle);
						int yopp = j - 2 * (j - ymiddle);
						znew[i, j] = GetExtrapolatedZ(znew, i, j);

						// right up quadrant
						znew[xopp, j] = GetExtrapolatedZ(znew, xopp, j);

						// left  up
						znew[i, yopp] = GetExtrapolatedZ(znew, i, yopp);

						// right down
						znew[xopp, yopp] = GetExtrapolatedZ(znew, xopp, yopp);
						// left  down
						//if(sqrad > newdiameter)
					}
					else
					{
						// ignore values standing outside desired diameter
						znew[i, j] = float.NaN;
					}
				}
			}

			// store result
			X = xnew;
			Y = ynew;
			Z = znew;
		}

		private static float GetExtrapolatedZ(float[,] grid, int currentXi, int currentYi)
		{
			dynamic left = (currentXi - 1 > 0 ? currentXi - 1 : currentXi);
			dynamic right = (currentXi + 1 < grid.Length ? currentXi + 1 : currentXi);
			dynamic bottom = (currentYi - 1 > 0 ? currentYi - 1 : currentYi);
			dynamic up = (currentYi + 1 < grid.GetLength(1) ? currentYi + 1 : currentYi);
			float cumval = 0;
			int nval = 0;

			for (int u = left; u <= right; u++)
			{
				for (int v = bottom; v <= up; v++)
				{
					if (!float.IsNaN(grid[u, v]))
					{
						cumval += grid[u, v];
						nval++;
					}
				}
			}

			if (nval > 0)
			{
				return cumval / nval;
			}
			else
			{
				return float.NaN;
			}
		}
	}
}
