using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
    public class Renderer3dFactory
	{
		public static object getInstance(View view, bool traceGL, bool debugGL)
		{
			throw new NotFiniteNumberException();
			//return new Renderer3d(view, traceGL, debugGL);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
