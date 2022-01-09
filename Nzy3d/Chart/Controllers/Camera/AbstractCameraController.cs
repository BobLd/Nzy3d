using Nzy3d.Maths;

namespace Nzy3d.Chart.Controllers.Camera
{
	public class AbstractCameraController : AbstractController
	{
		public static bool DEFAULT_UPDATE_VIEW = false;

		public AbstractCameraController() : base()
		{
		}

		public AbstractCameraController(Chart chart) : base(chart)
		{
		}

		protected void Rotate(Coord2d move)
		{
			Rotate(move, DEFAULT_UPDATE_VIEW);
		}

		protected void Rotate(Coord2d move, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.Rotate(move, DEFAULT_UPDATE_VIEW);
			}
			fireControllerEvent(ControllerType.ROTATE, move);
		}

		protected void Shift(float factor)
		{
			Shift(factor, DEFAULT_UPDATE_VIEW);
		}

		protected void Shift(float factor, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.Shift(factor, updateView);
			}
			fireControllerEvent(ControllerType.SHIFT, factor);
		}

		protected void ZoomX(float factor)
		{
			ZoomX(factor, DEFAULT_UPDATE_VIEW);
		}

		protected void ZoomX(float factor, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.ZoomX(factor, updateView);
			}
			fireControllerEvent(ControllerType.ZOOM, factor);
		}

		protected void ZoomY(float factor)
		{
			ZoomY(factor, DEFAULT_UPDATE_VIEW);
		}

		protected void ZoomY(float factor, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.ZoomY(factor, updateView);
			}
			fireControllerEvent(ControllerType.ZOOM, factor);
		}

		protected void ZoomZ(float factor)
		{
			ZoomZ(factor, DEFAULT_UPDATE_VIEW);
		}

		protected void ZoomZ(float factor, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.Zoom(factor, updateView);
			}
			fireControllerEvent(ControllerType.ZOOM, factor);
		}
	}
}
