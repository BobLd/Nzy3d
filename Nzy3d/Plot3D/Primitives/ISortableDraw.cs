using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Primitives
{
    public interface ISortableDraw
	{
		double getDistance(Camera camera);
		double getShortestDistance(Camera camera);
		double getLongestDistance(Camera camera);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
