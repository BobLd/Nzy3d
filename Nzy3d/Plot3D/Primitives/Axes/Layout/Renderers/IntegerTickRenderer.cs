namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers
{
	public class IntegerTickRenderer : ITickRenderer
	{
		public string Format(float value)
		{
			return Convert.ToInt32(value).ToString();
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
