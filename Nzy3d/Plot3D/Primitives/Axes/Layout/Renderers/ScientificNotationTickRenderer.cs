using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	/// <summary>
	/// Formats 1000 to '1.0e3'
	/// </summary>
	public class ScientificNotationTickRenderer : ITickRenderer
	{
		internal int _precision;
		public ScientificNotationTickRenderer() : this(1)
		{
		}

		public ScientificNotationTickRenderer(int precision)
		{
			_precision = precision;
		}

		public string Format(double value)
		{
			return Utils.Num2str('e', value, _precision);
		}
	}
}
