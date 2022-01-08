namespace Nzy3d.Events.Keyboard
{
	public interface IKeyListener : IBaseKeyListener
	{
		/// <summary>
		/// Invoked when a key has been typed (key pressed in .net). 
		/// </summary>
		void KeyTyped(object sender, KeyPressEventArgs e);

		/// <summary>
		/// Invoked when a key has been pressed (key down in .net).
		/// </summary>
		void KeyPressed(object sender, KeyEventArgs e);

		/// <summary>
		/// Invoked when a key has been released (key up in .net).
		/// </summary>
		void KeyReleased(object sender, KeyEventArgs e);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
