namespace Nzy3d.Events.Mouse
{
	public interface IMouseWheelListener : IBaseMouseWheelListener
	{
		/// <summary>
		/// Invoked when a mouse button is pressed on a component and then dragged.
		/// </summary>
		void MouseWheelMoved(object? sender, System.Windows.Forms.MouseEventArgs e);
	}
}
