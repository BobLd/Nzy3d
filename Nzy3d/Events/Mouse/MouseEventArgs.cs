namespace Nzy3d.Events.Mouse
{
	public class MouseEventArgs
	{
        public MouseEventArgs(double x, double y, MouseButton button)
		{
			X = x;
			Y = y;
			Button = button;
		}

		public double X { get; }

		public double Y { get; }

        public MouseButton Button { get; }
    }
}
