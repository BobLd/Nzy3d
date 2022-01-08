namespace Nzy3d.Maths
{
	public class Grid
	{
		private double[] m_x;
		private double[] m_y;

		private double[,] m_z;

		public Grid()
		{
		}

		public Grid(double[] x, double[] y, double[,] z)
		{
			this.SetData(x, y, z);
		}

		public Grid(int[] x, int[] y, int[,] z)
		{
			this.SetData(x, y, z);
		}

		public void SetData(double[] x, double[] y, double[,] z)
		{
			m_x = x;
			m_y = y;
			m_z = z;
		}

		public void SetData(int[] x, int[] y, int[,] z)
		{
			this.SetData(ToDoubleArray(x), ToDoubleArray(y), ToDoubleArray(z));
		}

		public double[] X
		{
			get { return m_x; }
		}

		public double[] Y
		{
			get { return m_y; }
		}

		public double[,] Z
		{
			get { return m_z; }
		}

		/// <summary>
		/// Computed and returns the bound of datas in the grid (x,y,z)
		/// </summary>
		/// <remarks>BoundingBox is recomputed each time the function is called, in contrary to nzy3D where it is kept in a dangerous cache.</remarks>
		public BoundingBox3d GetBounds()
		{
			return new BoundingBox3d(Statistics.Min(X), Statistics.Max(X),
									 Statistics.Min(Y), Statistics.Max(Y),
									 Statistics.Min(Z), Statistics.Max(Z));
		}

		internal double[] ToDoubleArray(int[] input)
		{
			double[] @out = new double[input.Length];
			for (int i = 0; i <= input.Length - 1; i++)
			{
				@out[i] = input[i];
			}
			return @out;
		}

		internal double[,] ToDoubleArray(int[,] input)
		{
			double[,] @out = new double[input.GetLength(0), input.GetLength(1)];
			for (int i = 0; i <= input.GetLength(0) - 1; i++)
			{
				for (int j = 0; j <= input.GetLength(1) - 1; j++)
				{
					@out[i, j] = input[i, j];
				}
			}
			return @out;
		}
	}
}
