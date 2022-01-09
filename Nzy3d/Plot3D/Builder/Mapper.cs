namespace Nzy3d.Plot3D.Builder
{
	public abstract class Mapper
	{
		/// <summary>
		/// Function to map. For a given x/y point, computes z value
		/// </summary>
		public abstract float f(float x, float y);

		/// <summary>
		/// Default implementation providing iterative calls to f(x,y)
		/// </summary>
		public float[] f(float[] x, float[] y)
		{
			float[] z = new float[x.Length];
			for (int i = 0; i <= x.Length - 1; i++)
			{
				z[i] = f(x[i], y[i]);
			}
			return z;
		}

		/// <summary>
		/// Default implementation providing iterative calls to f(x,y)
		/// </summary>
		/// <param name="xy">Array whose second dimension must be equal to two</param>
		public float[] f(float[,] xy)
		{
			if (xy.GetLength(1) != 2)
			{
				throw new ArgumentException("Input xy array must be have a length of 2 in second dimension. Current array second dimension has a lenght of " + xy.GetLength(1), nameof(xy));
			}

			float[] z = new float[xy.GetLength(0)];
			for (int i = 0; i <= xy.GetLength(0) - 1; i++)
			{
				z[i] = f(xy[i, 0], xy[i, 1]);
			}
			return z;
		}

		/// <summary>
		/// Default implementation providing iterative calls to f(x,y)
		/// </summary>
		public float[] fAsSingle(float[] x, float[] y)
		{
			float[] z = new float[x.Length];
			for (int i = 0; i <= x.Length - 1; i++)
			{
				z[i] = Convert.ToSingle(f(x[i], y[i]));
			}
			return z;
		}

		/// <summary>
		/// Default implementation providing iterative calls to f(x,y)
		/// </summary>
		/// <param name="xy">Array whose second dimension must be equal to two</param>
		public float[] fAsSingle(float[,] xy)
		{
			if (xy.GetLength(1) != 2)
			{
				throw new ArgumentException("Input xy array must be have a length of 2 in second dimension. Current array second dimension has a lenght of " + xy.GetLength(1), nameof(xy));
			}
			float[] z = new float[xy.GetLength(0)];
			for (int i = 0; i <= xy.GetLength(0) - 1; i++)
			{
				z[i] = Convert.ToSingle(f(xy[i, 0], xy[i, 0]));
			}
			return z;
		}
	}
}
