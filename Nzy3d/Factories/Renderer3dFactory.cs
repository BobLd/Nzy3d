using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
	public class Renderer3dFactory
	{
		public static object GetInstance(View view, bool traceGL, bool debugGL)
		{
			throw new NotFiniteNumberException("Renderer3dFactory.GetInstance()");
			//return new Renderer3d(view, traceGL, debugGL);
		}
	}
}
