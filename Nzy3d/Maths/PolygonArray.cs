namespace Nzy3d.Maths
{
	public sealed class PolygonArray
	{
		internal double[] _x;
		internal double[] _y;

		internal double[] _z;
		public PolygonArray(double[] x, double[] y, double[] z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public int Length
		{
			get { return _x.Length; }
		}

		public Coord3d Barycentre
		{
			get { return new Coord3d(Statistics.Mean(_x), Statistics.Mean(_y), Statistics.Mean(_z)); }
		}

		public double[] X
		{
			get { return _x; }
		}

		public double[] Y
		{
			get { return _y; }
		}

		public double[] Z
		{
			get { return _z; }
		}
	}
}
