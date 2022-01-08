using Nzy3d.Plot3D.Rendering.View.Modes;

namespace Nzy3d.Events
{
	public class ViewModeChangedEventArgs : ObjectEventArgs
	{
		private ViewPositionMode _mode;
		public ViewModeChangedEventArgs(object objectChanged, ViewPositionMode mode) : base(objectChanged)
		{
			_mode = mode;
		}

		public ViewPositionMode Mode
		{
			get { return _mode; }
		}
	}
}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
