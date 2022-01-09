namespace Nzy3d.Maths
{
	public class Grid
	{
		private float[] m_x;
		private float[] m_y;
		private float[,] m_z;

		public Grid()
		{
		}

		public Grid(float[] x, float[] y, float[,] z)
		{
			this.SetData(x, y, z);
		}

		public Grid(int[] x, int[] y, int[,] z)
		{
			this.SetData(x, y, z);
		}

		public void SetData(float[] x, float[] y, float[,] z)
		{
			m_x = x;
			m_y = y;
			m_z = z;
		}

		public void SetData(int[] x, int[] y, int[,] z)
		{
			this.SetData(ToFloatArray(x), ToFloatArray(y), ToFloatArray(z));
		}

		public float[] X
		{
			get { return m_x; }
		}

		public float[] Y
		{
			get { return m_y; }
		}

		public float[,] Z
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

		internal static float[] ToFloatArray(int[] input)
		{
			float[] _out = new float[input.Length];
			for (int i = 0; i <= input.Length - 1; i++)
			{
				_out[i] = input[i];
			}
			return _out;
		}

		internal static float[,] ToFloatArray(int[,] input)
		{
			float[,] _out = new float[input.GetLength(0), input.GetLength(1)];
			for (int i = 0; i <= input.GetLength(0) - 1; i++)
			{
				for (int j = 0; j <= input.GetLength(1) - 1; j++)
				{
					_out[i, j] = input[i, j];
				}
			}
			return _out;
		}
	}
}
