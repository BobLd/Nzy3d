using Nzy3d.Events;

namespace Nzy3d.Chart.Controllers
{
	public class AbstractController
	{
		protected readonly List<Chart> _targets = new List<Chart>();

		protected readonly List<IControllerEventListener> _controllerListeners = new List<IControllerEventListener>();

		public AbstractController()
		{
		}

		public AbstractController(Chart chart)
		{
			Register(chart);
		}

		public virtual void Register(Chart chart)
		{
			_targets.Add(chart);
		}

		public void Unregister(Chart chart)
		{
			_targets.Remove(chart);
		}

		protected Chart Chart
		{
			get { return _targets[0]; }
		}

		public virtual void Dispose()
		{
			_targets.Clear();
			_controllerListeners.Clear();
		}

		public void AddControllerEventListener(IControllerEventListener listener)
		{
			_controllerListeners.Add(listener);
		}

		public void RemoveControllerEventListener(IControllerEventListener listener)
		{
			_controllerListeners.Remove(listener);
		}

		protected Task FireControllerEvent(ControllerType type, object value)
		{
			return Task.Run(() =>
			{
				var e = new ControllerEventArgs(this, type, value);
				foreach (IControllerEventListener aListener in _controllerListeners)
				{
					aListener.ControllerEventFired(e);
				}
			});
		}
	}
}
