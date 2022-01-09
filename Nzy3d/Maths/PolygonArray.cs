namespace Nzy3d.Maths
{
	public class PolygonArray
	{
		internal float[] _x;
		internal float[] _y;
		internal float[] _z;

		public PolygonArray(float[] x, float[] y, float[] z)
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

		public float[] X
		{
			get { return _x; }
		}

		public float[] Y
		{
			get { return _y; }
		}

		public float[] Z
		{
			get { return _z; }
		}
	}
}
