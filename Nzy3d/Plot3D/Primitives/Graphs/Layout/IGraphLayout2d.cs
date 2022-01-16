using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Graphs.Layout
{
	public interface IGraphLayout2d<V>
	{
		Coord2d VertexPosition { get; set; }

		Coord2d GetV(V v);

		List<Coord2d> Values();
	}
}
