using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	/// <summary>
	/// Force number to be represented with a given number of decimals
	/// </summary>
	public class DateTickRenderer : ITickRenderer
	{
		internal string _format;
		public DateTickRenderer() : this("dd/MM/yyyy HH:mm:ss")
		{
		}

		public DateTickRenderer(string format)
		{
			_format = format;
		}

		public string Format(float value)
		{
			DateTime ldate = Utils.Num2date(Convert.ToInt64(value));
			return Utils.Dat2str(ldate, _format);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
