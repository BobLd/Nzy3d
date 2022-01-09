using Nzy3d.Plot3D.Builder.Delaunay.Jdt;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public interface ITriangulation
	{
		void insertPoint(Point_dt p);

		IEnumerator<Triangle_dt> trianglesIterator();

		IEnumerator<Point_dt> VerticesIterator();

		int trianglesSize();
	}
}
