using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Primitives.Axes
{
    public interface IAxe
	{
		void Dispose();
		void setAxe(BoundingBox3d box);
		void Draw(Camera camera);
		void setScale(Coord3d scale);
		BoundingBox3d getBoxBounds();
		Coord3d getCenter();
		IAxeLayout getLayout();
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
