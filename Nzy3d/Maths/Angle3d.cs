namespace Nzy3d.Maths
{
	/// <summary>
	/// An Angle3d stores three 3d points, considering the angle is on the second one.
	/// An instance may return angle(), cos() and sin().
	/// </summary>
	public sealed class Angle3d
	{
		#region "Members"
		private readonly double X1;
		private readonly double X2;
		private readonly double X3;
		private readonly double Y1;
		private readonly double Y2;
		private readonly double Y3;
		private readonly double Z1;
		private readonly double Z2;
		#endregion

		private readonly double Z3;

		#region "Constructors"
		public Angle3d(double x1, double x2, double x3, double y1, double y2, double y3, double z1, double z2, double z3)
		{
			this.X1 = x1;
			this.X2 = x2;
			this.X3 = x3;
			this.Y1 = y1;
			this.Y2 = y2;
			this.Y3 = y3;
			this.Z1 = z1;
			this.Z2 = z2;
			this.Z3 = z3;
		}

		/// <summary>
		/// Create an angle, described by three coordinates.
		/// The angle is supposed to be on p2
		/// </summary>
		public Angle3d(Coord3d p1, Coord3d p2, Coord3d p3) : this(p1.X, p2.X, p3.X, p1.Y, p2.Y, p3.Y, p1.Z, p2.Z, p3.Z)
		{
		}
		#endregion

		#region "Functions"
		/// <summary>
		/// Computes the sinus of the angle
		/// </summary>
		public double Sin()
		{
			var c2 = new Coord3d(X2, Y2, Z2);
			var v1 = new Vector3d(X1, Y1, Z1, X2, Y2, Z2);
			var v3 = new Vector3d(X3, Y3, Z3, X2, Y2, Z2);
			var c4 = v1.Cross(v3).Add(c2);
			var v4 = new Vector3d(c4, c2);
			return (c4.Z >= 0 ? 1 : -1) * v4.Norm() / (v1.Norm() * v3.Norm());
		}

		/// <summary>
		/// Computes the cosinus of the angle
		/// </summary>
		public double Cos()
		{
			var v1 = new Vector3d(X1, Y1, Z1, X2, Y2, Z2);
			var v3 = new Vector3d(X3, Y3, Z3, X2, Y2, Z2);
			return v1.Dot(v3) / (v1.Norm() * v3.Norm());
		}

		/// <summary>
		/// Computes an angle between 0 and 2*PI
		/// </summary>
		public double Angle()
		{
			// between 0 and PI: Math.acos(cos());
			if (Sin() > 0)
			{
				return Math.Acos(Cos());
			}
			else
			{
				return Math.PI * 2 - Math.Acos(Cos());
			}
		}
		#endregion
	}
}