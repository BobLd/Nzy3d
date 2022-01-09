namespace Nzy3d.Maths
{
	/// <summary>
	/// A BoundingBox2d stores a couple of maximal and minimal limit on
	///  each dimension (x, y) in cartesian coordinates. It provides functions for enlarging
	///  the box by adding cartesian coordinates or an other
	///  BoundingBox2d (that is equivalent to computing the union of the
	///  current BoundingBox and another one).
	/// </summary>
	public class BoundingBox2d
	{
		private float m_xmin;
		private float m_xmax;
		private float m_ymin;
		private float m_ymax;

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method.
		/// </summary>
		public BoundingBox2d()
		{
			Reset();
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates
		/// </summary>
		public BoundingBox2d(List<Coord2d> lst)
		{
			Reset();
			foreach (Coord2d c in lst)
			{
				Add(c);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox with raw values.
		/// </summary>
		public BoundingBox2d(float xmin, float xmax, float ymin, float ymax)
		{
			m_xmin = xmin;
			m_xmax = xmax;
			m_ymin = ymin;
			m_ymax = ymax;
		}

		/// <summary>
		///  Initialize the bounding box with Float.MAX_VALUE as minimum
		/// value, and -Float.MAX_VALUE as maximum value for each dimension.
		/// </summary>
		public void Reset()
		{
			m_xmin = float.MaxValue;
			m_xmax = float.MinValue;
			m_ymin = float.MaxValue;
			m_ymax = float.MinValue;
		}

		/// <summary>
		/// Adds an x,y point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void Add(float x, float y)
		{
			if (x > m_xmax)
				m_xmax = x;
			if (x < m_xmin)
				m_xmin = x;
			if (y > m_xmax)
				m_ymax = y;
			if (y < m_xmin)
				m_ymin = y;
		}

		/// <summary>
		/// Adds a <see cref="Coord2d"/> point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void Add(Coord2d p)
		{
			this.Add(p.X, p.Y);
		}

		/// <summary>
		/// Adds another <see cref="BoundingBox2d"/> to the bounding box, and enlarge the bounding
		/// box if its points lies outside of it (i.e. merge other bounding box inside current one)
		/// </summary>
		public void Add(BoundingBox2d b)
		{
			this.Add(b.m_xmin, b.m_ymin);
			this.Add(b.m_xmax, b.m_ymax);
		}

		/// <summary>
		/// Compute and return the center point of the BoundingBox3d
		/// </summary>
		public Coord2d GetCenter()
		{
			return new Coord2d((m_xmax + m_xmin) / 2, (m_ymax + m_ymin) / 2);
		}

		/// <summary>
		/// Return the radius of the Sphere containing the Bounding Box,
		/// i.e., the distance between the center and the point (xmin, ymin).
		/// </summary>
		public double GetRadius()
		{
			return GetCenter().Distance(new Coord2d(m_xmin, m_ymin));
		}

		/// <summary>
		/// Return a copy of the current bounding box after scaling all limits relative to 0,0
		/// Scaling does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox2d Scale(Coord2d factors)
		{
			return new BoundingBox2d
			{
				m_xmax = m_xmax * factors.X,
				m_xmin = m_xmin * factors.X,
				m_ymax = m_ymax * factors.Y,
				m_ymin = m_ymin * factors.Y
			};
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> is contained in this box.
		/// </summary>
		/// <remarks>if b1.contains(b2), then b1.intersect(b2) as well.</remarks>
		public bool Contains(BoundingBox2d anotherBox)
		{
			return m_xmin <= anotherBox.m_xmin && anotherBox.m_xmax <= m_xmax && m_ymin <= anotherBox.m_ymin && anotherBox.m_ymax <= m_ymax;
		}

		/// <summary>
		/// Return true if <paramref name="aPoint"/> is contained in this box.
		/// </summary>
		public bool Contains(Coord2d aPoint)
		{
			return m_xmin <= aPoint.X && aPoint.X <= m_xmax && m_ymin <= aPoint.Y && aPoint.Y <= m_ymax;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> intersects with this box.
		/// </summary>
		public bool Intersect(BoundingBox2d anotherBox)
		{
			return (m_xmin <= anotherBox.m_xmin && anotherBox.m_xmin <= m_xmax) || (m_xmin <= anotherBox.m_xmax && anotherBox.m_xmax <= m_xmax) &&
				   (m_ymin <= anotherBox.m_ymin && anotherBox.m_ymin <= m_ymax) || (m_ymin <= anotherBox.m_ymax && anotherBox.m_ymax <= m_ymax);
		}

		/// <summary>
		/// Bounding box min x value
		/// </summary>
		public double XMin
		{
			get { return m_xmin; }
		}

		/// <summary>
		/// Bounding box max x value
		/// </summary>
		public double XMax
		{
			get { return m_xmax; }
		}

		/// <summary>
		/// Bounding box min y value
		/// </summary>
		public double YMin
		{
			get { return m_ymin; }
		}

		/// <summary>
		/// Bounding box max y value
		/// </summary>
		public double YMax
		{
			get { return m_ymax; }
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"(BoundingBox2d){XMin}<=x<={XMax} | {YMin}<=y<={YMax}";
		}
	}
}
