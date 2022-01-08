using Nzy3d.Maths;

namespace Nzy3d.Events
{
	public class ViewPointChangedEventArgs : ObjectEventArgs
	{
		public ViewPointChangedEventArgs(object objectChanged, Coord3d viewPoint) : base(objectChanged)
		{
			ViewPoint = viewPoint;
		}

		public Coord3d ViewPoint { get; }
	}
}
