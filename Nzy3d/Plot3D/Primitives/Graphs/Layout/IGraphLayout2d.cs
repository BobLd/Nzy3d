using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Graphs.Layout
{
	public interface IGraphLayout2d<V>
	{
		Coord2d VertexPosition { get; set; }
		Coord2d getV(V v);
		List<Coord2d> values();
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
