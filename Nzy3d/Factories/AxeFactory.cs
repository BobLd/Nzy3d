using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives.Axes;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
    public class AxeFactory
	{
		public static object getInstance(BoundingBox3d box, View view)
		{
			AxeBox axe = new AxeBox(box);
			axe.View = view;
			return axe;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
