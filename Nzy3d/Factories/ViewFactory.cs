using Nzy3d.Chart;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
    public class ViewFactory
	{
		public static View getInstance(Scene scene, ICanvas canvas, Quality quality)
		{
			return new ChartView(scene, canvas, quality);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
