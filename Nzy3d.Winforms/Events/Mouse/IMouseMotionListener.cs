namespace Nzy3d.Events.Mouse
{
    public interface IMouseMotionListener : IBaseMouseMotionListener
	{
		// ''' <summary>
		// ''' Invoked when a mouse button is pressed on a component and then dragged.
		// ''' </summary>
		//Sub MouseDragged(sender As Object, e As System.Windows.Forms.MouseEventArgs)
		//Never raised by winfoms

		/// <summary>
		/// Invoked when the mouse cursor has been moved onto a component but no buttons have been pushed.
		/// </summary>
		void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
