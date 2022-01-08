using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	/// <summary>
	/// Force number to be represented with a given number of decimals
	/// </summary>
	public class FixedDecimalTickRenderer : ITickRenderer
	{
		internal int _precision;
		public FixedDecimalTickRenderer() : this(6)
		{
		}

		public FixedDecimalTickRenderer(int precision)
		{
			_precision = precision;
		}

		public string Format(float value)
		{
			return Utils.Num2str('f', value, _precision);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
