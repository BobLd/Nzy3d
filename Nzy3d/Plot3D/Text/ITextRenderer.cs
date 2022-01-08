using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using Nzy3d.Plot3D.Text.Align;

namespace Nzy3d.Plot3D.Text
{
    public interface ITextRenderer
	{
		BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color);
		BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset, Coord3d sceneOffset);
		BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset);
		BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord3d sceneOffset);
		void drawSimpleText(Camera cam, string s, Coord3d position, Color color);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
