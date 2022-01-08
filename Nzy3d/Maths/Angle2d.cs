namespace Nzy3d.Maths
{
	/// <summary>
	/// An Angle2d stores three 2d points, considering the angle is on the second one.
	/// An instance may return angle(), cos() and sin()
	/// </summary>
	public class Angle2d
	{
		#region "Members"
		private readonly double X1;
		private readonly double X2;
		private readonly double X3;
		private readonly double Y1;
		private readonly double Y2;
		private readonly double Y3;
		#endregion

		#region "Constructors"
		public Angle2d(double x1, double x2, double x3, double y1, double y2, double y3)
		{
			this.X1 = x1;
			this.X2 = x2;
			this.X3 = x3;
			this.Y1 = y1;
			this.Y2 = y2;
			this.Y3 = y3;
		}

		public Angle2d(Coord2d p1, Coord2d p2, Coord2d p3) : this(p1.X, p2.X, p3.X, p1.Y, p2.Y, p3.Y)
		{
		}
		#endregion

		#region "Functions"
		/// <summary>
		/// Computes the sinus of the angle
		/// </summary>
		public double Sin()
		{
			const double x4 = 0;
			//(y1-y2)*(z3-z2) - (z1-z2)*(y3-y2);

			const double y4 = 0;
			//(z1-z2)*(x3-x2) - (x1-x2)*(z3-z2);
			double z4 = (X1 - X2) * (Y3 - Y2) - (Y1 - Y2) * (X3 - X2);

			var v1 = new Vector3d(X1, Y1, 0, X2, Y2, 0);
			var v3 = new Vector3d(X3, Y3, 0, X2, Y2, 0);
			var v4 = new Vector3d(x4, y4, z4, X2, Y2, 0);
			return (z4 >= 0 ? 1 : -1) * v4.Norm() / (v1.Norm() * v3.Norm());
		}

		/// <summary>
		/// Computes the cosinus of the angle
		/// </summary>
		public double Cos()
		{
			var v1 = new Vector2d(X1, Y1, X2, Y2);
			var v3 = new Vector2d(X3, Y3, X2, Y2);
			return v1.Dot(v3) / (v1.Norm() * v3.Norm());
		}

		/// <summary>
		/// Computes the angle
		/// </summary>
		public double Angle()
		{
			var v1 = new Vector2d(X1, Y1, X2, Y2);
			var v3 = new Vector2d(X3, Y3, X2, Y2);
			return Math.Acos(v1.Dot(v3));
		}
		#endregion
	}
}
