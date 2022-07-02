using Nzy3d.Events.Mouse;

namespace Nzy3d.Wpf.Events.Mouse
{
    public interface IMouseListener : IBaseMouseListener
	{
		/// <summary>
		/// Invoked when the mouse button has been clicked (pressed and released) on a component.
		/// </summary>
		//void MouseClicked(object? sender, System.Windows.Forms.MouseEventArgs e);

		/// <summary>
		/// Invoked when a mouse button has been pressed on a component.
		/// </summary>
		void MousePressed(object? sender, System.Windows.Input.MouseButtonEventArgs e);

		/// <summary>
		/// Invoked when a mouse button has been released on a component.
		/// </summary>
		void MouseReleased(object? sender, System.Windows.Input.MouseButtonEventArgs e);

		/// <summary>
		/// Invoked when a mouse button has been double clicked.
		/// </summary>
		//void MouseDoubleClicked(object? sender, System.Windows.Forms.MouseEventArgs e);
	}
}
