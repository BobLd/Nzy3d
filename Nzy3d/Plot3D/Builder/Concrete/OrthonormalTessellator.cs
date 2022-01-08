using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder.Concrete
{
	/// <summary>
	/// <para>
	/// The <see cref="OrthonormalTessellator"/> checks that coordinates are lying on an orthormal grid,
	/// and is able to provide a <see cref="AbstractComposite"/> made of <see cref="Polygon"/>s built according to this grid
	/// </para>
	/// <para>
	/// On this model, one input coordinate is represented by one <see cref="Polygon"/>, for which each point is
	/// a mean point between two grid ticks:
	/// </para>
	/// <para>
	///  ^                           ^
	///  |                           |
	///  -   +   +   +               -   +   +   +
	///  |                           |     *---*
	///  -   +   o   +        >>     -   + | o | +
	///  |                           |     *---*
	///  -   +   +   +               -   +   +   +
	///  |                           |
	///  |---|---|---|-->            |---|---|---|-->
	/// </para>
	/// <para>
	///  In this figure, the representation of a coordinate ("o" on the left) is a polygon
	///  made of mean points ("*" on the right) that require the existence of four surrounding
	///  points (the "o" and the three "+")
	/// </para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public class OrthonormalTessellator : Tessellator
	{
		protected internal float[] X;
		protected internal float[] Y;
		protected internal float[,] Z;
		protected internal int FindXi;
		protected internal int FindYj;

		protected internal void SetData(float[] x, float[] y, float[] z)
		{
			if (x.Length != y.Length || x.Length != z.Length)
			{
				throw new Exception("x, y, and z arrays must agree in length.");
			}

			// Initialize loading
			this.X = Unique(x);
			this.Y = Unique(y);
			this.Z = new float[this.X.Length + 1, this.Y.Length + 1];
			for (int i = 0; i <= this.X.Length - 1; i++)
			{
				for (int j = 0; j <= this.Y.Length - 1; j++)
				{
					this.Z[i, j] = float.NaN;
				}
			}

            for (int p = 0; p <= z.Length - 1; p++)
			{
                // Fill Z matrix and set surface minimum and maximum
                bool found = Find(this.X, this.Y, x[p], y[p]);
                if (!found)
				{
					throw new Exception("it seems (x[p],y[p]) has not been properly stored into (this.x,this.y)");
				}
				this.Z[FindXi, FindYj] = z[p];
			}
		}

		internal static float[] Unique(float[] data)
		{
			float[] copy = (float[])data.Clone();
			System.Array.Sort(copy);

			// count unique values
			int nunique = 0;
			float last = float.NaN;

			for (int i = 0; i <= copy.Length - 1; i++)
			{
				if (float.IsNaN(copy[i]))
				{
					// System.out.println("Ignoring NaN value at " + i);
				}
				else if (copy[i] != last)
				{
                    nunique++;
					last = copy[i];
				}
			}

			// Fill a sorted unique array
			float[] result = new float[nunique];
			last = float.NaN;
			int r = 0;
			for (int d = 0; d <= copy.Length - 1; d++)
			{
				if (float.IsNaN(copy[d]))
				{
					// System.out.println("Ignoring NaN value at " + d);
				}
				else if (copy[d] != last)
				{
					result[r] = copy[d];
					last = copy[d];
                    r++;
				}
			}
			return result;
		}

		/// <summary>
		/// Search in a couple of array a combination of values vx and vy.
		/// Positions xi and yi are returned in findxi and findyj class variables
		/// Function returns true if the couple of data may be retrieved,
		/// false otherwise (in this case, findxi and findyj remain unchanged).
		/// </summary>
		internal bool Find(float[] x, float[] y, float vx, float vy)
		{
			int xi = -1;
			int yj = -1;
			for (int i = 0; i <= x.Length - 1; i++)
			{
				if (x[i] == vx)
				{
					xi = i;
				}
			}

			if (xi == -1)
			{
				return false;
			}

			for (int j = 0; j <= y.Length - 1; j++)
			{
				if (y[j] == vy)
				{
					yj = j;
				}
			}

			if (yj == -1)
			{
				return false;
			}

			FindXi = xi;
			FindYj = yj;
			return true;
		}

		public List<Polygon> GetSquarePolygonsOnCoordinates()
		{
			return GetSquarePolygonsOnCoordinates(null, null);
		}

		public List<Polygon> GetSquarePolygonsOnCoordinates(ColorMapper cmap, Color colorFactor)
		{
			List<Polygon> polygons = new List<Polygon>();
			for (int xi = 0; xi <= X.Length - 2; xi++)
			{
				for (int yi = 0; yi <= Y.Length - 2; yi++)
				{
					// Compute quad making a polygon
					Point[] p = GetRealQuadStandingOnPoint(xi, yi);
					if (!ValidZ(p))
					{
						continue;
						// ignore non valid set of points
					}

                    if (cmap != null)
					{
						p[0].Color = cmap.Color(p[0].XYZ);
						p[1].Color = cmap.Color(p[1].XYZ);
						p[2].Color = cmap.Color(p[2].XYZ);
						p[3].Color = cmap.Color(p[3].XYZ);
					}

					if (colorFactor != null)
					{
						p[0].Rgb.mul(colorFactor);
						p[1].Rgb.mul(colorFactor);
						p[2].Rgb.mul(colorFactor);
						p[3].Rgb.mul(colorFactor);
					}

					// Store quad
					var quad = new Polygon();
					for (int pi = 0; pi <= p.Length - 1; pi++)
					{
						quad.Add(p[pi]);
					}
					polygons.Add(quad);
				}
			}
			return polygons;
		}

		public object GetSquarePolygonsAroundCoordinates()
		{
			return GetSquarePolygonsAroundCoordinates(null, null);
		}

		public object GetSquarePolygonsAroundCoordinates(ColorMapper cmap, Color colorFactor)
		{
			var polygons = new List<Polygon>();

			for (int xi = 0; xi <= X.Length - 2; xi++)
			{
				for (int yi = 0; yi <= Y.Length - 2; yi++)
				{
					// Compute quad making a polygon
					Point[] p = GetEstimatedQuadSurroundingPoint(xi, yi);
					if (!ValidZ(p))
					{
						continue;
						// ignore non valid set of points
					}

                    if (cmap != null)
					{
						p[0].Color = cmap.Color(p[0].XYZ);
						p[1].Color = cmap.Color(p[1].XYZ);
						p[2].Color = cmap.Color(p[2].XYZ);
						p[3].Color = cmap.Color(p[3].XYZ);
					}

                    if (colorFactor != null)
					{
						p[0].Rgb.mul(colorFactor);
						p[1].Rgb.mul(colorFactor);
						p[2].Rgb.mul(colorFactor);
						p[3].Rgb.mul(colorFactor);
					}
					// Store quad
					Polygon quad = new Polygon();
					for (int pi = 0; pi <= p.Length - 1; pi++)
					{
						quad.Add(p[pi]);
					}
					polygons.Add(quad);
				}
			}
			return polygons;
		}

		protected internal Point[] GetRealQuadStandingOnPoint(int xi, int yi)
		{
			Point[] p = new Point[4];
			p[0] = new Point(new Coord3d(X[xi], Y[yi], Z[xi, yi]));
			p[1] = new Point(new Coord3d(X[xi + 1], Y[yi], Z[xi + 1, yi]));
			p[2] = new Point(new Coord3d(X[xi + 1], Y[yi + 1], Z[xi + 1, yi + 1]));
			p[3] = new Point(new Coord3d(X[xi], Y[yi + 1], Z[xi, yi + 1]));
			return p;
		}

		internal Point[] GetEstimatedQuadSurroundingPoint(int xi, int yi)
		{
			Point[] p = new Point[4];
			p[0] = new Point(new Coord3d((X[xi - 1] + X[xi]) / 2, (Y[yi + 1] + Y[yi]) / 2, (Z[xi - 1, yi + 1] + Z[xi - 1, yi] + Z[xi, yi] + Z[xi, yi + 1]) / 4));
			p[1] = new Point(new Coord3d((X[xi - 1] + X[xi]) / 2, (Y[yi - 1] + Y[yi]) / 2, (Z[xi - 1, yi] + Z[xi - 1, yi - 1] + Z[xi, yi - 1] + Z[xi, yi]) / 4));
			p[2] = new Point(new Coord3d((X[xi + 1] + X[xi]) / 2, (Y[yi - 1] + Y[yi]) / 2, (Z[xi, yi] + Z[xi, yi - 1] + Z[xi + 1, yi - 1] + Z[xi + 1, yi]) / 4));
			p[3] = new Point(new Coord3d((X[xi + 1] + X[xi]) / 2, (Y[yi + 1] + Y[yi]) / 2, (Z[xi, yi + 1] + Z[xi, yi] + Z[xi + 1, yi] + Z[xi + 1, yi + 1]) / 4));
			return p;
		}

		internal static bool ValidZ(Point[] points)
		{
			foreach (Point p in points)
			{
				if (!ValidZ(p))
				{
					return false;
				}
			}
			return true;
		}

		internal static bool ValidZ(Point p)
		{
			return !double.IsNaN(p.XYZ.Z);
		}

		public override AbstractComposite Build(float[] x, float[] y, float[] z)
		{
			SetData(x, y, z);
			Shape s = new Shape();
			s.Add(GetSquarePolygonsOnCoordinates());
			return s;
		}
	}
}
