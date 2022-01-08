namespace Nzy3d.Events.Mouse
{
	public class MouseEventArgs
	{
		private MouseButton _button;
		public MouseEventArgs(double x, double y, MouseButton button)
		{
			X = x;
			Y = y;
			_button = button;
		}

		public double X { get; }

		public double Y { get; }

		public MouseButton Button
		{
			get { return _button; }
		}
	}
}
