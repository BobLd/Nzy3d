using Nzy3d.Events.Mouse;

namespace Nzy3d.Wpf.Events.Mouse
{
    public interface IMouseWheelListener : IBaseMouseWheelListener
	{
		/// <summary>
		/// Invoked when a mouse button is pressed on a component and then dragged.
		/// </summary>
		void MouseWheelMoved(object? sender, System.Windows.Input.MouseWheelEventArgs e);
	}
}
