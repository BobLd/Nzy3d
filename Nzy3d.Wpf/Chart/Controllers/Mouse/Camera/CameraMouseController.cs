using Nzy3d.Chart.Controllers.Mouse.Camera;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Wpf.Events.Mouse;
using System;
using System.Windows;

namespace Nzy3d.Wpf.Chart.Controllers.Mouse.Camera
{
    public class CameraMouseController : BaseCameraMouseController, IMouseListener, IMouseMotionListener, IMouseWheelListener
	{
		/// <summary>
		/// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
		/// rotation, while simple click stops it.
		/// </summary>
		public void MousePressed(object? sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is not FrameworkElement element)
			{
				throw new ArgumentException(nameof(sender));
			}

			bool isDoubleClick = e.ClickCount == 2;

			// if double click
			// Handles toggle between mouse rotation/auto rotation: double-click starts the animated
			// rotation, while simple click stops it.

			if (HandleSlaveThread(isDoubleClick))
			{
				return;
			}

			var p = e.GetPosition(element);

			_prevMouse.X = p.X;
			_prevMouse.Y = p.Y;
		}

		public void MouseReleased(object? sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		}

		public void MouseMoved(object? sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is not ICanvas canvas)
			{
				throw new ArgumentException(nameof(sender));
			}

			var p = e.GetPosition((FrameworkElement)sender);

			Coord2d mouse = new Coord2d(p.X, p.Y);

			SetMousePosition((int)p.X, (int)(canvas.RendererHeight - p.Y));

			// Rotate
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
			{
				Coord2d move = mouse.Substract(_prevMouse).Divide(100);
				Rotate(move);
			}
			else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
			{
				Coord2d move = mouse.Substract(_prevMouse);
				if (move.Y != 0)
				{
					Shift((float)(move.Y / 250));
				}
			}
			_prevMouse = mouse;
		}

		public void MouseWheelMoved(object? sender, System.Windows.Input.MouseWheelEventArgs e)
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
