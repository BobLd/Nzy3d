using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Delaunay.Jdt;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public class DelaunayTessellator : Tessellator
	{
		public AbstractComposite Build(List<Coord3d> Coordinates)
		{
			return this.Build(new Coordinates(Coordinates));
		}

		public AbstractComposite Build(Coordinates coord)
		{
			ICoordinateValidator cv = new DelaunayCoordinateValidator(coord);
			Delaunay_Triangulation dt = new Delaunay_Triangulation();
			DelaunayTriangulationManager tesselator = new DelaunayTriangulationManager(cv, dt);
			return (Shape)tesselator.BuildDrawable();
		}

		public override AbstractComposite Build(float[] x, float[] y, float[] z)
		{
			return this.Build(new Coordinates(x, y, z));
		}
	}
}
