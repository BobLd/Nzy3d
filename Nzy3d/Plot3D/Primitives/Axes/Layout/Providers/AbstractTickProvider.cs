namespace Nzy3d.Plot3D.Primitives.Axes.Layout.Providers
{
	public abstract class AbstractTickProvider : ITickProvider
	{
		public float[] GenerateTicks(float min, float max)
		{
			return GenerateTicks(min, max, DefaultSteps);
		}

		public abstract int DefaultSteps { get; }

		public abstract float[] GenerateTicks(float min, float max, int steps);
	}
}
