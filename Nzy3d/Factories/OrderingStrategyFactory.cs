using Nzy3d.Plot3D.Rendering.Ordering;

namespace Nzy3d.Factories
{
	public class OrderingStrategyFactory
	{
		public static AbstractOrderingStrategy getInstance()
		{
			return DEFAULTORDERING;
		}

		public static BarycentreOrderingStrategy DEFAULTORDERING = new BarycentreOrderingStrategy();
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
