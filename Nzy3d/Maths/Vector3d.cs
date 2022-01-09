namespace Nzy3d.Maths
{
	/// <summary>
	/// <para>Storage for a 3 dimensional vector defined by two points.</para>
	/// <para>
	/// Provide the vector function that returns the vector
	/// as a Coord3d, as well as dot product and norm.
	/// </para>
	/// </summary>
	public struct Vector3d
	{
		private readonly float m_x1;
		private readonly float m_x2;
		private readonly float m_y1;
		private readonly float m_y2;
		private readonly float m_z1;
		private readonly float m_z2;

		public Vector3d(float x1, float x2, float y1, float y2, float z1, float z2)
		{
			m_x1 = x1;
			m_x2 = x2;
			m_y1 = y1;
			m_y2 = y2;
			m_z1 = z1;
			m_z2 = z2;
		}

		public Vector3d(Coord3d p1, Coord3d p2)
			: this(p1.X, p2.X, p1.Y, p2.Y, p1.Z, p2.Z)
		{
		}

		/// <summary>
		/// Return the vector induced by this set of coordinates
		/// </summary>
		public Coord3d Vector
		{
			get { return new Coord3d(m_x2 - m_x1, m_y2 - m_y1, m_z2 - m_z1); }
		}

		/// <summary>
		/// Compute the dot product between and current and given vector.
		/// </summary>
		/// <remarks>Remind that the dot product is 0 if vectors are perpendicular</remarks>
		public float Dot(Vector3d v)
		{
			return this.Vector.Dot(v.Vector);
		}

		/// <summary>
		/// Computes the vectorial product of the current and the given vector.
		/// The result is a vector defined as a Coord3d, that is perpendicular to
		/// the plan induced by current vector and vector V.
		/// </summary>
		public Coord3d Cross(Vector3d v)
		{
			Coord3d v1 = this.Vector;
			Coord3d v2 = v.Vector;
            return new Coord3d
            {
                X = v1.Y * v2.Z - v1.Z * v2.Y,
                Y = v1.Z * v2.X - v1.X * v2.Z,
                Z = v1.X * v2.Y - v1.Y * v2.X
            };
		}

		/// <summary>
		/// Compute the norm of this vector.
		/// </summary>
		public float Norm()
		{
			return MathF.Sqrt(this.Vector.MagSquared());
		}

		/// <summary>
		/// Compute the distance between two coordinates.
		/// </summary>
		public float Distance(Coord3d c)
		{
			return this.Center.Distance(c);
		}

		public Coord3d Center
		{
			get { return new Coord3d((m_x1 + m_x2) / 2, (m_y1 + m_y2) / 2, (m_z1 + m_z2) / 2); }
		}
	}
}
