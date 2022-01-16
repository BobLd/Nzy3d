namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Providers
{
	public interface ITickProvider
	{
		float[] GenerateTicks(float min, float max);

		float[] GenerateTicks(float min, float max, int steps);

		int DefaultSteps { get; }
	}
}
