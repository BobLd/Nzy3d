using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
	public class CameraFactory
	{
		public static Camera GetInstance(Coord3d center)
		{
			return new Camera(center);
		}
	}
}
