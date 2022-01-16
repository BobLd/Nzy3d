namespace Nzy3d.Maths
{
	/// <summary>
	/// A BoundingBox2d stores a couple of maximal and minimal limit on
	/// each dimension (x, y) in cartesian coordinates. It provides functions for enlarging
	/// the box by adding cartesian coordinates or an other
	/// BoundingBox2d (that is equivalent to computing the union of the
	/// current BoundingBox and another one).
	/// </summary>
	public sealed class BoundingBox2d
	{
		private double m_minx;
		private double m_maxx;
		private double m_miny;
		private double m_maxy;

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
		public BoundingBox2d(double xmin, double xmax, double ymin, double ymax)
		{
			m_minx = xmin;
			m_maxx = xmax;
			m_miny = ymin;
			m_maxy = ymax;
		}

		/// <summary>
		///  Initialize the bounding box with Float.MAX_VALUE as minimum
		/// value, and -Float.MAX_VALUE as maximum value for each dimension.
		/// </summary>
		public void Reset()
		{
			m_minx = double.MaxValue;
			m_maxx = double.MinValue;
			m_miny = double.MaxValue;
			m_maxy = double.MinValue;
		}

		/// <summary>
		/// Adds an x,y point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void Add(double x, double y)
		{
			if (x > m_maxx)
            {
                m_maxx = x;
            }

            if (x < m_minx)
            {
                m_minx = x;
            }

            if (y > m_maxx)
            {
                m_maxy = y;
            }

            if (y < m_minx)
            {
                m_miny = y;
            }
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
			this.Add(b.m_minx, b.m_miny);
			this.Add(b.m_maxx, b.m_maxy);
		}

		/// <summary>
		/// Compute and return the center point of the BoundingBox3d
		/// </summary>
		public Coord2d GetCenter()
		{
			return new Coord2d((m_maxx + m_minx) / 2, (m_maxy + m_miny) / 2);
		}

		/// <summary>
		/// Return the radius of the Sphere containing the Bounding Box,
		/// i.e., the distance between the center and the point (xmin, ymin).
		/// </summary>
		public double GetRadius()
		{
			return GetCenter().Distance(new Coord2d(m_minx, m_miny));
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
                m_maxx = m_maxx * factors.X,
                m_minx = m_minx * factors.X,
                m_maxy = m_maxy * factors.Y,
                m_miny = m_miny * factors.Y
            };
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> is contained in this box.
		/// </summary>
		/// <remarks>if b1.contains(b2), then b1.intersect(b2) as well.</remarks>
		public bool Contains(BoundingBox2d anotherBox)
		{
			return m_minx <= anotherBox.m_minx && anotherBox.m_maxx <= m_maxx && m_miny <= anotherBox.m_miny && anotherBox.m_maxy <= m_maxy;
		}

		/// <summary>
		/// Return true if <paramref name="aPoint"/> is contained in this box.
		/// </summary>
		public bool Contains(Coord2d aPoint)
		{
			return m_minx <= aPoint.X && aPoint.X <= m_maxx && m_miny <= aPoint.Y && aPoint.Y <= m_maxy;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> intersects with this box.
		/// </summary>
		public bool Intersect(BoundingBox2d anotherBox)
		{
			return (m_minx <= anotherBox.m_minx && anotherBox.m_minx <= m_maxx) ||
				   (m_minx <= anotherBox.m_maxx && anotherBox.m_maxx <= m_maxx) &&
				   (m_miny <= anotherBox.m_miny && anotherBox.m_miny <= m_maxy) ||
				   (m_miny <= anotherBox.m_maxy && anotherBox.m_maxy <= m_maxy);
		}

		/// <summary>
		/// Bounding box min x value
		/// </summary>
		public double MinX
		{
			get { return m_minx; }
		}

		/// <summary>
		/// Bounding box max x value
		/// </summary>
		public double MaxX
		{
			get { return m_maxx; }
		}

		/// <summary>
		/// Bounding box min y value
		/// </summary>
		public double MinY
		{
			get { return m_miny; }
		}

		/// <summary>
		/// Bounding box max y value
		/// </summary>
		public double MaxY
		{
			get { return m_maxy; }
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return "(BoundingBox2d)" + MinX + "<=x<=" + MaxX + " | " + MinY + "<=y<=" + MaxY;
		}
	}
}
