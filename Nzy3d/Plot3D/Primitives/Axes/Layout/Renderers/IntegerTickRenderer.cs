namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	public class IntegerTickRenderer : ITickRenderer
	{
		public string Format(double value)
		{
			return Convert.ToInt32(value).ToString();
		}
	}
}
