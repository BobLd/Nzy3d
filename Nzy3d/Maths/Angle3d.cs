namespace Nzy3d.Maths
{
	/// <summary>
	/// An Angle3d stores three 3d points, considering the angle is on the second one.
	/// An instance may return angle(), cos() and sin().
	/// </summary>
	public class Angle3d
	{
		#region "Members"
		private readonly float X1;
		private readonly float X2;
		private readonly float X3;
		private readonly float Y1;
		private readonly float Y2;
		private readonly float Y3;
		private readonly float Z1;
		private readonly float Z2;
		private readonly float Z3;
		#endregion

		#region "Constructors"
		public Angle3d(float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3)
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
		public float Sin()
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
		public float Cos()
		{
			var v1 = new Vector3d(X1, Y1, Z1, X2, Y2, Z2);
			var v3 = new Vector3d(X3, Y3, Z3, X2, Y2, Z2);
			return v1.Dot(v3) / (v1.Norm() * v3.Norm());
		}

		/// <summary>
		/// Computes an angle between 0 and 2*PI
		/// </summary>
		public float Angle()
		{
			// between 0 and PI: Math.acos(cos());
			if (Sin() > 0)
			{
				return MathF.Acos(Cos());
			}
			else
			{
				return MathF.PI * 2 - MathF.Acos(Cos());
			}
		}
		#endregion
	}
}