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

		public override void DrawSimpleText(Camera cam, string s, Coord3d position, Color color)
		{
			GL.Color3(color.R, color.G, color.B);
			GL.RasterPos3(position.X, position.Y, position.Z);
			Glut.Glut.BitmapString(_font, s);
		}

		public override BoundingBox3d DrawText(Camera cam, string s, Coord3d position, Halign halign, Valign valign, Colors.Color color, Coord2d screenOffset, Coord3d sceneOffset)
		{
			GL.Color3(color.R, color.G, color.B);
			Coord3d posScreen = cam.ModelToScreen(position);
			float strlen = Glut.Glut.BitmapLength(_font, s);
            float x;
            switch (halign)
            {
                case Halign.RIGHT:
                    x = (float)posScreen.X;
                    break;

                case Halign.CENTER:
                    x = (float)(posScreen.X - strlen / 2);
                    break;

                case Halign.LEFT:
                    x = (float)(posScreen.X - strlen);
                    break;

                default:
                    throw new Exception("Unsupported halign value");
            }

            float y;
            switch (valign)
            {
                case Valign.TOP:
                case Valign.GROUND:
                    y = (float)posScreen.Y;
                    break;

                case Valign.CENTER:
                    y = (float)(posScreen.Y - _fontHeight / 2);
                    break;

                case Valign.BOTTOM:
                    y = (float)(posScreen.Y - _fontHeight);
                    break;

                default:
                    throw new Exception("Unsupported valign value");
            }
            Coord3d posScreenShifted = new Coord3d(x + screenOffset.X, y + screenOffset.Y, posScreen.Z);

			Coord3d posReal;
			try
			{
				posReal = cam.ScreenToModel(posScreenShifted);
			}
			catch (InvalidOperationException ex)
			{
				// The bug due to a Camera.PERSPECTIVE mode / 'Near' value < 1 should be fixe, this should not happen anymore
				System.Diagnostics.Debug.WriteLine("TextBitmap.drawText(): Error - could not process text position: " + posScreen.ToString() + " " + posScreenShifted.ToString() + "\n" + ex.Message);
				return new BoundingBox3d();
			}

			// Draw actual string
			GL.RasterPos3(posReal.X + sceneOffset.X, posReal.Y + sceneOffset.Y, posReal.Z + sceneOffset.Z);
			Glut.Glut.BitmapString(_font, s);
			// Compute bounds of text
			Coord3d botLeft = new Coord3d();
			Coord3d topRight = new Coord3d();
			botLeft.X = posScreenShifted.X;
			botLeft.Y = posScreenShifted.Y;
			botLeft.Z = posScreenShifted.Z;
			topRight.X = botLeft.X + strlen;
			topRight.Y = botLeft.Y + _fontHeight;
			topRight.Z = botLeft.Z;
			BoundingBox3d txtBounds = new BoundingBox3d();
			txtBounds.Add(cam.ScreenToModel(botLeft));
			txtBounds.Add(cam.ScreenToModel(topRight));
			return txtBounds;
		}
	}
}
