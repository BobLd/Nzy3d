using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
    public class CameraFactory
	{
		public static Camera getInstance(Coord3d center)
		{
			return new Camera(center);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
