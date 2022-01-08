using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using Nzy3d.Plot3D.Text.Align;

namespace Nzy3d.Plot3D.Text
{
    public interface ITextRenderer
	{
		BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color);
		BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset, Coord3d sceneOffset);
		BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset);
		BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord3d sceneOffset);
		void DrawSimpleText(Camera cam, string s, Coord3d position, Color color);
	}
}
