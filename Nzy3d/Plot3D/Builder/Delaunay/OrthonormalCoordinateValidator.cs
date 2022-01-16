using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public class OrthonormalCoordinateValidator : ICoordinateValidator
	{
		private float[] _x;
		private float[] _y;
#pragma warning disable RCS1169, IDE0044 // Make field read-only.
		private float[] _z;
#pragma warning restore RCS1169, IDE0044 // Make field read-only.
		private float[,] _z_as_fxy;
		private int _findxi;

		private int _findyj;
		public OrthonormalCoordinateValidator(Coordinates coords)
		{
			if (coords == null)
			{
				throw new ArgumentException("Function call with illegal value 'Nothing' for parameter coords.", nameof(coords));
			}

			if (coords.X == null)
			{
				throw new ArgumentException("Illegal result value 'Nothing' on x property of parameter coords.", nameof(coords));
			}

			if (coords.Y == null)
			{
				throw new ArgumentException("Illegal result value 'Nothing' on y property of parameter coords.", nameof(coords));
			}

			if (coords.Z == null)
			{
				throw new ArgumentException("Illegal result value 'Nothing' on z property of parameter coords.", nameof(coords));
			}

			if (coords.X.Length != coords.Y.Length)
			{
				throw new ArgumentException("Parameter coords has different x size (" + coords.X.Length + ") than y size (" + coords.Y.Length + ")", nameof(coords));
			}

			if (coords.X.Length != coords.Z.Length)
			{
				throw new ArgumentException("Parameter coords has different x size (" + coords.X.Length + ") than z size (" + coords.Z.Length + ")", nameof(coords));
			}

			SetData(coords);
		}

		internal void SetData(Coordinates coords)
		{
			_x = MakeCoordinatesUnique(coords.X);
			_y = MakeCoordinatesUnique(coords.Y);
			_z_as_fxy = new float[_x.Length, _y.Length];

			for (int i = 0; i <= _x.Length - 1; i++)
			{
				for (int j = 0; j <= _y.Length - 1; j++)
				{
					_z_as_fxy[i, j] = float.NaN;
				}
			}

			for (int p = 0; p <= coords.Z.Length - 1; p++)
			{
				bool found = Find(_x, _y, coords.X[p], coords.Y[p]);
				if (!found)
				{
					throw new Exception("It seems (x[p],y[p]) has not been properly stored into (this.x,this.y)");
				}
				_z_as_fxy[_findxi, _findyj] = coords.Z[p];
			}
		}

		internal bool Find(float[] x, float[] y, float vx, float vy)
		{
			for (int i = 0; i <= x.Length - 1; i++)
			{
				for (int j = 0; j <= y.Length - 1; j++)
				{
					if (x[i] == vx && y[j] == vy)
					{
						_findxi = i;
						_findyj = j;
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Compute a sorted array from input, with a unique occurrence of each
		/// value. Note: any NaN value will be ignored and won't appear in the output
		/// array.
		/// </summary>
		/// <param name="data">Input array</param>
		/// <returns>A sorted array containing only one occurrence of each input value, without NaN</returns>
		internal static float[] MakeCoordinatesUnique(float[] data)
		{
			float[] copy = (float[])data.Clone();
			System.Array.Sort(copy);
			int nunique = 0;
			float last = float.NaN;

			for (int i = 0; i <= copy.Length - 1; i++)
			{
				if (float.IsNaN(copy[i]))
				{
					// Ignore NaN values
				}
				else if (copy[i] != last)
				{
					nunique++;
					last = copy[i];
				}
			}

			float[] result = new float[nunique];
			int r = 0;
			last = float.NaN;

			for (int i = 0; i <= copy.Length - 1; i++)
			{
				if (double.IsNaN(copy[i]))
				{
					// Ignore NaN values
				}
				else if (copy[i] != last)
				{
					result[r] = copy[i];
					r++;
					last = copy[i];
				}
			}
			return result;
		}

		/// <inheritdoc/>
		public float[,] GetZAsFxy()
		{
			return this._z_as_fxy;
		}

		/// <inheritdoc/>
		public float[] GetX()
		{
			return this._y;
		}

		/// <inheritdoc/>
		public float[] GetY()
		{
			return this._y;
		}

		/// <inheritdoc/>
		public float[] GetZ()
		{
			return this._z;
		}
	}
}
