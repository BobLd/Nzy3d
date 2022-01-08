using Nzy3d.Chart;

namespace Nzy3d.Factories
{

    public class SceneFactory
	{
		public static ChartScene getInstance(bool sort)
		{
			return new ChartScene(sort);
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
