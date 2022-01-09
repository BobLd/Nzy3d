namespace Nzy3d.Maths
{
	/// <summary>
	/// <para>Storage for a 2 dimensional vector defined by two points.</para>
	/// <para>
	/// Provide the vector function that returns the vector
	/// as a Coord3d, as well as dot product and norm.
	/// </para>
	/// </summary>
	public struct Vector2d
	{
		#region "Members"
		private readonly float x1;
		private readonly float x2;
		private readonly float y1;
		private readonly float y2;
		#endregion

		#region "Constructors"
		public Vector2d(float x1, float x2, float y1, float y2)
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
		public float Dot(Vector2d v)
		{
			Coord2d v1 = this.Vector();
			Coord2d v2 = v.Vector();
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public float Norm()
		{
			return MathF.Sqrt(MathF.Pow(x2 - x1, 2) + MathF.Pow(y2 - y1, 2));
		}
		#endregion
	}
}
