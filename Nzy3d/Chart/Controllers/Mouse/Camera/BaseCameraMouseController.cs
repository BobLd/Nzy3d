using Nzy3d.Chart.Controllers.Camera;
using Nzy3d.Chart.Controllers.Thread.Camera;
using Nzy3d.Events.Mouse;
using Nzy3d.Maths;

namespace Nzy3d.Chart.Controllers.Mouse.Camera
{
	public class BaseCameraMouseController : AbstractCameraController, IBaseMouseListener, IBaseMouseMotionListener, IBaseMouseWheelListener
	{
		protected Coord2d _prevMouse;
		protected CameraThreadController _threadController;

		protected float _prevZoom = 1;
		public BaseCameraMouseController()
		{
		}

		public BaseCameraMouseController(Chart chart)
		{
			Register(chart);
		}

		public override void Register(Chart chart)
		{
			base.Register(chart);
			_prevMouse = Coord2d.ORIGIN;
			chart.Canvas.AddMouseListener(this);
			chart.Canvas.AddMouseMotionListener(this);
			chart.Canvas.AddMouseWheelListener(this);
		}

		public override void Dispose()
		{
			foreach (Chart c in _targets)
			{
				c.Canvas.RemoveMouseListener(this);
				c.Canvas.RemoveMouseMotionListener(this);
				c.Canvas.RemoveMouseWheelListener(this);
			}

			_threadController?.Dispose();
			base.Dispose();
		}

		/// <summary>
		/// Remove existing threadcontroller (if existing) and add the one passed in parameters as controller.
		/// </summary>
		public void AddSlaveThreadController(CameraThreadController controller)
		{
			RemoveSlaveThreadController();
			_threadController = controller;
		}

		public void RemoveSlaveThreadController()
		{
			_threadController?.StopT();
			_threadController = null;
		}

		//public void MouseClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//}

		///// <summary>
		///// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		///// rotation, while simple click stops it.
		///// </summary>
		//public void MousePressed(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//	if (handleSlaveThread(false))
		//	{
		//		return;
		//	}
		//	_prevMouse.x = e.X;
		//	_prevMouse.y = e.Y;
		//}

		///// <summary>
		///// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		///// rotation, while simple click stops it.
		///// </summary>
		//public void MouseDoubleClicked(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//	if (handleSlaveThread(true))
		//	{
		//		return;
		//	}
		//	_prevMouse.x = e.X;
		//	_prevMouse.y = e.Y;
		//}

		public bool HandleSlaveThread(bool isDoucleClick)
		{
            if (isDoucleClick && _threadController != null)
            {
                _threadController.Start();
                return true;
            }

            _threadController?.StopT();
			return false;
		}

		//public void MouseReleased(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//}

		//Public Sub MouseDragged(sender As Object, e As System.Windows.Forms.MouseEventArgs) Implements Events.Mouse.IMouseMotionListener.MouseDragged
		//  ' Never raised by Winfo
		//End Sub

		//public void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//	if (e.Button != System.Windows.Forms.MouseButtons.None)
		//	{
		//		Coord2d mouse = new Coord2d(e.X, e.Y);
		//		// Rotate
		//		if (e.Button == System.Windows.Forms.MouseButtons.Left)
		//		{
		//			Coord2d move = mouse.substract(_prevMouse).divide(100);
		//			Rotate(move);
		//		}
		//		if (e.Button == System.Windows.Forms.MouseButtons.Right)
		//		{
		//			Coord2d move = mouse.substract(_prevMouse);
		//			if (move.y != 0)
		//			{
		//				Shift((float)(move.y / 250));
		//			}
		//		}
		//		_prevMouse = mouse;
		//	}
		//}

		//public void MouseWheelMoved(object sender, System.Windows.Forms.MouseEventArgs e)
		//{
		//	if (((_threadController != null)))
		//	{
		//		_threadController.StopT();
		//	}
		//	if (e.Delta > 0)
		//	{
		//		_prevZoomZ = 1.25f;
		//	}
		//	else
		//	{
		//		_prevZoomZ = 0.8f;
		//	}
		//	ZoomZ(_prevZoomZ);
		//}
	}
}
