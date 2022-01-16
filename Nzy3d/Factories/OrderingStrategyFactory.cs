using Nzy3d.Plot3D.Rendering.Ordering;

namespace Nzy3d.Factories
{
	public class OrderingStrategyFactory
	{
		public static AbstractOrderingStrategy GetInstance()
		{
			return DEFAULTORDERING;
		}

		public static readonly BarycentreOrderingStrategy DEFAULTORDERING = new BarycentreOrderingStrategy();
	}
}
