using Nzy3d.Events.Mouse;
using Nzy3d.Maths;

namespace Nzy3d.Chart.Controllers.Mouse.Camera
{
	public class CameraMouseController : BaseCameraMouseController, IMouseListener, IMouseMotionListener, IMouseWheelListener
	{
		public void MouseClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MousePressed(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (handleSlaveThread(false))
			{
				return;
			}
			_prevMouse.x = e.X;
			_prevMouse.y = e.Y;
		}

		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MouseDoubleClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (handleSlaveThread(true))
			{
				return;
			}
			_prevMouse.x = e.X;
			_prevMouse.y = e.Y;
		}

		public void MouseReleased(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		public void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button != MouseButtons.None)
			{
				Coord2d mouse = new Coord2d(e.X, e.Y);
				// Rotate
				if (e.Button == MouseButtons.Left)
				{
					Coord2d move = mouse.substract(_prevMouse).divide(100);
					Rotate(move);
				}
				if (e.Button == MouseButtons.Right)
				{
					Coord2d move = mouse.substract(_prevMouse);
					if (move.y != 0)
					{
						Shift((float)(move.y / 250));
					}
				}
				_prevMouse = mouse;
			}
		}

		public void MouseWheelMoved(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (((_threadController != null)))
			{
				_threadController.StopT();
			}

			if (e.Delta > 0)
			{
				_prevZoomZ = 1.25f;
			}
			else
			{
				_prevZoomZ = 0.8f;
			}
			ZoomZ(_prevZoomZ);
		}
	}
}
