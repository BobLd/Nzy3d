using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public class DelaunayCoordinateValidator : ICoordinateValidator
	{
		private readonly float[] _x;
		private readonly float[] _y;
		private readonly float[,] _z_as_fxy;

		public DelaunayCoordinateValidator(Coordinates coords)
		{
			if (coords == null)
			{
				throw new ArgumentNullException(nameof(coords), "Function call with illegal value 'null' for parameter coords.");
			}

			if (coords.X == null)
			{
				throw new ArgumentNullException(nameof(coords), "Illegal result value 'null' on x property of parameter coords.");
			}

			if (coords.Y == null)
			{
				throw new ArgumentNullException(nameof(coords), "Illegal result value 'null' on y property of parameter coords.");
			}

			if (coords.Z == null)
			{
				throw new ArgumentNullException(nameof(coords), "Illegal result value 'null' on z property of parameter coords.");
			}

			if (coords.X.Length != coords.Y.Length)
			{
				throw new ArgumentNullException(nameof(coords), $"Parameter coords has different x size ({coords.X.Length}) than y size ({coords.Y.Length})");
			}

			if (coords.X.Length != coords.Z.Length)
			{
				throw new ArgumentNullException(nameof(coords), $"Parameter coords has different x size ({coords.X.Length}) than z size ({coords.Z.Length})");
			}

			_x = coords.X;
			_y = coords.Y;
			_z_as_fxy = SetData(coords.Z);
		}

		internal static float[,] SetData(float[] z)
		{
			int length = z.Length;
			float[,] z_as_fxy = new float[length, length];
			for (int p = 0; p <= length - 1; p++)
			{
				z_as_fxy[p, p] = z[p];
			}
			return z_as_fxy;
		}

		public float[,] Get_Z_as_fxy()
		{
			return _z_as_fxy;
		}

		public float[] GetX()
		{
			return _x;
		}

		public float[] GetY()
		{
			return _y;
		}
	}
}
