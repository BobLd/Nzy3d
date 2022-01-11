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

		protected Task Rotate(Coord2d move)
		{
			return Rotate(move, DEFAULT_UPDATE_VIEW);
		}

		protected async Task Rotate(Coord2d move, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				await c.View.Rotate(move, updateView).ConfigureAwait(false);
			}
			await FireControllerEvent(ControllerType.ROTATE, move).ConfigureAwait(false);
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
			FireControllerEvent(ControllerType.SHIFT, factor);
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
			FireControllerEvent(ControllerType.ZOOM, factor);
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
			FireControllerEvent(ControllerType.ZOOM, factor);
		}

		protected void ZoomZ(float factor)
		{
			ZoomZ(factor, DEFAULT_UPDATE_VIEW);
		}

		protected void ZoomZ(float factor, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.ZoomZ(factor, updateView);
			}
			FireControllerEvent(ControllerType.ZOOM, factor);
		}

		protected void ZoomXYZ(float factor)
		{
			ZoomXYZ(factor, DEFAULT_UPDATE_VIEW);
		}

		protected void ZoomXYZ(float factor, bool updateView)
		{
			foreach (Chart c in _targets)
			{
				c.View.ZoomXYZ(factor, updateView);
			}
			FireControllerEvent(ControllerType.ZOOM, factor);
		}
	}
}