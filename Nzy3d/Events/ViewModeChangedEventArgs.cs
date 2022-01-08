using Nzy3d.Plot3D.Rendering.View.Modes;

namespace Nzy3d.Events
{
	public class ViewModeChangedEventArgs : ObjectEventArgs
	{
		public ViewModeChangedEventArgs(object objectChanged, ViewPositionMode mode) : base(objectChanged)
		{
			Mode = mode;
		}

		public ViewPositionMode Mode { get; }
	}
}
