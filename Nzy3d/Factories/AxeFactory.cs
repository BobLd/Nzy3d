using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives.Axes;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Factories
{
	public class AxeFactory
	{
		public static object GetInstance(BoundingBox3d box, View view)
		{
            return new AxeBox(box)
            {
                View = view
            };
		}
	}
}
