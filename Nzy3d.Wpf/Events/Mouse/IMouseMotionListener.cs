using Nzy3d.Events.Mouse;

namespace Nzy3d.Wpf.Events.Mouse
{
    public interface IMouseMotionListener : IBaseMouseMotionListener
	{
		/// <summary>
		/// Invoked when the mouse cursor has been moved onto a component but no buttons have been pushed.
		/// </summary>
		void MouseMoved(object? sender, System.Windows.Input.MouseEventArgs e);
	}
}
