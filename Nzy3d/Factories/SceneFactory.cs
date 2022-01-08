using Nzy3d.Chart;

namespace Nzy3d.Factories
{
	public class SceneFactory
	{
		public static ChartScene GetInstance(bool sort)
		{
			return new ChartScene(sort);
		}
	}
}
