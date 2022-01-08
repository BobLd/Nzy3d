using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Delaunay.Jdt;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public class DelaunayTriangulationManager
	{
		protected float[] _x;
		protected float[] _y;
		protected float[,] _z_as_fxy;

		protected ITriangulation _triangulator;

		public DelaunayTriangulationManager(ICoordinateValidator cv, ITriangulation triangulator)
		{
			_triangulator = triangulator;
			this.X = cv.GetX();
			this.Y = cv.GetY();
			this.Z_as_fxy = cv.Get_Z_as_fxy();
		}

		public AbstractDrawable BuildDrawable()
		{
			Shape s = new Shape();
			s.Add(GetFacets());
			return s;
		}

		// TODO: three different point classes coord3d, point_dt !!
		private List<Polygon> GetFacets()
		{
			int xlen = _x.Length;
			for (int i = 0; i <= xlen - 1; i++)
			{
				Point_dt point_dt = new Point_dt(X[i], Y[i], Z_as_fxy[i, i]);
				_triangulator.insertPoint(point_dt);
			}

			var polygons = new List<Polygon>();
			IEnumerator<Triangle_dt> trianglesIter = _triangulator.trianglesIterator();

			while (trianglesIter.MoveNext())
			{
				Triangle_dt triangle = trianglesIter.Current;
				// isHalfplane means a degenerated triangle 
				if (triangle.IsHalfplane)
				{
					continue;
				}

				Polygon newPolygon = BuildPolygonFrom(triangle);
				polygons.Add(newPolygon);
			}
			return polygons;
		}

		private static Polygon BuildPolygonFrom(Triangle_dt triangle)
		{
			Coord3d c1 = triangle.P1.Coord3d;
			Coord3d c2 = triangle.P2.Coord3d;
			Coord3d c3 = triangle.P3.Coord3d;
			Polygon polygon = new Polygon();
			polygon.Add(new Point(c1));
			polygon.Add(new Point(c2));
			polygon.Add(new Point(c3));
			return polygon;
		}

		public float[] X
		{
			get { return _x; }
			set { _x = value; }
		}

		public float[] Y
		{
			get { return _y; }
			set { _y = value; }
		}

		public float[,] Z_as_fxy
		{
			get { return _z_as_fxy; }
			set { _z_as_fxy = value; }
		}
	}
}
