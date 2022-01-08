using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;
using Nzy3d.Plot3D.Text.Align;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Text.Renderers
{
    public class TextBitmapRenderer : AbstractTextRenderer, ITextRenderer
	{
		internal int _fontHeight;

		internal int _font;
		public TextBitmapRenderer() : base()
		{
			_font = Glut.Glut.BITMAP_HELVETICA_10;
			_fontHeight = 10;
		}

		public override void drawSimpleText(Camera cam, string s, Coord3d position, Color color)
		{
			GL.Color3(color.r, color.g, color.b);
			GL.RasterPos3(position.x, position.y, position.z);
			Glut.Glut.BitmapString(_font, s);
		}

		public override BoundingBox3d drawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Colors.Color color, Coord2d screenOffset, Coord3d sceneOffset)
		{
			GL.Color3(color.r, color.g, color.b);
			Coord3d posScreen = cam.ModelToScreen(position);
			float strlen = Glut.Glut.BitmapLength(_font, s);
			float x = 0;
			float y = 0;
			switch (halign)
			{
				case Halign.RIGHT:
					x = (float)posScreen.x;
					break;
				case Halign.CENTER:
					x = (float)(posScreen.x - strlen / 2);
					break;
				case Halign.LEFT:
					x = (float)(posScreen.x - strlen);
					break;
				default:
					throw new Exception("Unsupported halign value");
			}
			switch (valign)
			{
				case Valign.TOP:
					y = (float)(posScreen.y);
					break;
				case Valign.GROUND:
					y = (float)(posScreen.y);
					break;
				case Valign.CENTER:
					y = (float)(posScreen.y - _fontHeight / 2);
					break;
				case Valign.BOTTOM:
					y = (float)(posScreen.y - _fontHeight);
					break;
				default:
					throw new Exception("Unsupported valign value");
			}
			Coord3d posScreenShifted = new Coord3d(x + screenOffset.x, y + screenOffset.y, posScreen.z);
			Coord3d posReal = default(Coord3d);
			try
			{
				posReal = cam.ScreenToModel(posScreenShifted);
				// TODO: really solve this bug due to a Camera.PERSPECTIVE mode
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("TextBitmap.drawText(): could not process text position: " + posScreen.ToString() + " " + posScreenShifted.ToString());
				return new BoundingBox3d();
			}
			// Draw actual string
			GL.RasterPos3(posReal.x + sceneOffset.x, posReal.y + sceneOffset.y, posReal.z + sceneOffset.z);
			Glut.Glut.BitmapString(_font, s);
			// Compute bounds of text
			Coord3d botLeft = new Coord3d();
			Coord3d topRight = new Coord3d();
			botLeft.x = posScreenShifted.x;
			botLeft.y = posScreenShifted.y;
			botLeft.z = posScreenShifted.z;
			topRight.x = botLeft.x + strlen;
			topRight.y = botLeft.y + _fontHeight;
			topRight.z = botLeft.z;
			BoundingBox3d txtBounds = new BoundingBox3d();
			txtBounds.@add(cam.ScreenToModel(botLeft));
			txtBounds.@add(cam.ScreenToModel(topRight));
			return txtBounds;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
