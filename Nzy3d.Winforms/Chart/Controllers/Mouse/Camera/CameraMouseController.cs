using Nzy3d.Events.Mouse;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.Canvas;

namespace Nzy3d.Chart.Controllers.Mouse.Camera
{
    public class CameraMouseController : BaseCameraMouseController, IMouseListener, IMouseMotionListener, IMouseWheelListener
	{
		public void MouseClicked(object? sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MousePressed(object? sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (HandleSlaveThread(false))
			{
				return;
			}
			_prevMouse.X = e.X;
			_prevMouse.Y = e.Y;
		}

		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MouseDoubleClicked(object? sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (HandleSlaveThread(true))
			{
				return;
			}
			_prevMouse.X = e.X;
			_prevMouse.Y = e.Y;
		}

		public void MouseReleased(object? sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		public void MouseMoved(object? sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sender is not ICanvas canvas)
			{
				return;
			}

			Coord2d mouse = new Coord2d(e.X, e.Y);

			SetMousePosition(e.X, canvas.RendererHeight - e.Y);

			if (e.Button != MouseButtons.None)
			{
				// Rotate
				if (e.Button == MouseButtons.Left)
				{
					Coord2d move = mouse.Substract(_prevMouse).Divide(100);
					Rotate(move);
				}
				else if (e.Button == MouseButtons.Right)
				{
					Coord2d move = mouse.Substract(_prevMouse);
					if (move.Y != 0)
					{
						Shift((float)(move.Y / 250));
					}
				}
				_prevMouse = mouse;
			}
		}

		public void MouseWheelMoved(object? sender, System.Windows.Forms.MouseEventArgs e)
		{
			_threadController?.Stop();

			if (e.Delta > 0)
			{
				_prevZoomZ = 0.9f;
			}
			else
			{
				_prevZoomZ = 1.1f;
			}
			ZoomXYZ(_prevZoomZ);
		}
	}
}
