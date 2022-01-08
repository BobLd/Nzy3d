using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Delaunay.Jdt;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public class DelaunayTessellator : Tessellator
	{
		public AbstractComposite build(List<Coord3d> Coordinates)
		{
			return this.build(new Coordinates(Coordinates));
		}

		public AbstractComposite build(Coordinates coord)
		{
			ICoordinateValidator cv = new DelaunayCoordinateValidator(coord);
			Delaunay_Triangulation dt = new Delaunay_Triangulation();
			DelaunayTriangulationManager tesselator = new DelaunayTriangulationManager(cv, dt);
			return (Shape)tesselator.buildDrawable();
		}

		public override AbstractComposite build(float[] x, float[] y, float[] z)
		{
			return this.build(new Coordinates(x, y, z));
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
