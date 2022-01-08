namespace Nzy3d.Maths
{
	/// <summary>
	/// <para>Storage for a 2 dimensional vector defined by two points.</para>
	/// <para>
	/// Provide the vector function that returns the vector
	/// as a Coord3d, as well as dot product and norm.
	/// </para>
	/// </summary>
	public class Vector2d
	{
		#region "Members"
		private readonly double x1;
		private readonly double x2;
		private readonly double y1;
		private readonly double y2;
		#endregion

		#region "Constructors"
		public Vector2d(double x1, double x2, double y1, double y2)
		{
			this.x1 = x1;
			this.x2 = x2;
			this.y1 = y1;
			this.y2 = y2;
		}

		public Vector2d(Coord2d p1, Coord2d p2)
			: this(p1.X, p2.X, p1.Y, p2.Y)
		{
		}
		#endregion

		#region "Functions"
		/// <summary>
		/// Return the vector (sizes) induced by this set of coordinates
		/// </summary>
		public Coord2d Vector()
		{
			return new Coord2d(x2 - x1, y2 - y1);
		}

		/// <summary>
		/// Compute the dot product between the current and given vector
		/// </summary>
		public double Dot(Vector2d v)
		{
			Coord2d v1 = this.Vector();
			Coord2d v2 = v.Vector();
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public double Norm()
		{
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		}
		#endregion
	}
}
