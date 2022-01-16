using Nzy3d.Colors;
using Nzy3d.Plot3D.Primitives.Axes.Layout.Providers;
using Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers;

namespace Nzy3d.Plot3D.Primitives.Axes.Layout
{
	public interface IAxeLayout
	{
		Color MainColor { get; set; }

		Color GridColor { get; set; }

		bool FaceDisplayed { get; set; }

		Color QuadColor { get; set; }

		string XAxeLabel { get; set; }

		string YAxeLabel { get; set; }

		string ZAxeLabel { get; set; }

		bool XAxeLabelDisplayed { get; set; }

		bool YAxeLabelDisplayed { get; set; }

		bool ZAxeLabelDisplayed { get; set; }

		bool XTickLabelDisplayed { get; set; }

		bool YTickLabelDisplayed { get; set; }

		bool ZTickLabelDisplayed { get; set; }

		bool TickLineDisplayed { get; set; }

		ITickProvider XTickProvider { get; set; }

		ITickProvider YTickProvider { get; set; }

		ITickProvider ZTickProvider { get; set; }

		ITickRenderer XTickRenderer { get; set; }

		ITickRenderer YTickRenderer { get; set; }

		ITickRenderer ZTickRenderer { get; set; }

		Color XTickColor { get; set; }

		Color YTickColor { get; set; }

		Color ZTickColor { get; set; }

		float[] XTicks();

		float[] YTicks();

		float[] ZTicks();

		float[] XTicks(float min, float max);

		float[] YTicks(float min, float max);

		float[] ZTicks(float min, float max);
	}
}
