namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Providers
{
	public class StaticTickProvider : AbstractTickProvider, ITickProvider
	{
		internal float[] _values;
		public StaticTickProvider(float[] values)
		{
			_values = values;
		}

		public override int DefaultSteps
		{
			get { return 0; }
		}

		public override float[] generateTicks(float min, float max, int steps)
		{
			return _values;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
