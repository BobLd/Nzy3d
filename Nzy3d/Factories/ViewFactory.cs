using Nzy3d.Chart;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
	public class ViewFactory
	{
		public static View GetInstance(Scene scene, ICanvas canvas, Quality quality)
		{
			return new ChartView(scene, canvas, quality);
		}
	}
}
