namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Providers
{
    public interface ITickProvider
	{
		float[] generateTicks(float min, float max);
		float[] generateTicks(float min, float max, int steps);
		int DefaultSteps { get; }
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
