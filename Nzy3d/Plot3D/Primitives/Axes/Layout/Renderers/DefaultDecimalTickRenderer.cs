using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	/// <summary>
	/// Force number to be represented with a given number of decimals
	/// </summary>
	public class DefaultDecimalTickRenderer : ITickRenderer
	{
		internal int _precision;
		public DefaultDecimalTickRenderer() : this(6)
		{
		}

		public DefaultDecimalTickRenderer(int precision)
		{
			_precision = precision;
		}

		public string Format(double value)
		{
			return Utils.Num2str('g', value, _precision);
		}
	}
}
