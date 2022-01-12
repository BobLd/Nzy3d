using Nzy3d.Plot3D.Builder.Delaunay.Jdt;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
    public interface ITriangulation
	{
		void InsertPoint(Point_dt p);

		IEnumerator<Triangle_dt> TrianglesIterator();

		IEnumerator<Point_dt> VerticesIterator();

		int TrianglesSize();
	}
}