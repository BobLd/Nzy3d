using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Primitives
{
    public interface ISortableDraw
	{
		double GetDistance(Camera camera);

		double GetShortestDistance(Camera camera);

		double GetLongestDistance(Camera camera);
	}
}
