using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using Nzy3d.Plot3D.Text.Align;

namespace Nzy3d.Plot3D.Text
{
	public abstract class AbstractTextRenderer : ITextRenderer
	{
		internal Coord2d defScreenOffset;

		internal Coord3d defSceneOffset;
		public AbstractTextRenderer()
		{
			defScreenOffset = new Coord2d();
			defSceneOffset = new Coord3d();
		}

		public abstract void DrawSimpleText(Camera cam, string s, Coord3d position, Color color);

		public BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color)
		{
			return DrawText(cam, s, position, halign, valign, color, defScreenOffset, defSceneOffset);
		}

		public BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset)
		{
			return DrawText(cam, s, position, halign, valign, color, screenOffset, defSceneOffset);
		}

		public abstract BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset, Coord3d sceneOffset);

		public BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord3d sceneOffset)
		{
			return DrawText(cam, s, position, halign, valign, color, defScreenOffset, sceneOffset);
		}
	}
}

