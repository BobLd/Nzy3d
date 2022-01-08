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

		public abstract void drawSimpleText(Camera cam, string s, Coord3d position, Color color);

		public BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color)
		{
			return drawText(cam, s, position, halign, valign, color, defScreenOffset, defSceneOffset);
		}

		public BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset)
		{
			return drawText(cam, s, position, halign, valign, color, screenOffset, defSceneOffset);
		}

		public abstract BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord2d screenOffset, Coord3d sceneOffset);

		public BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Color color, Coord3d sceneOffset)
		{
			return drawText(cam, s, position, halign, valign, color, defScreenOffset, sceneOffset);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
