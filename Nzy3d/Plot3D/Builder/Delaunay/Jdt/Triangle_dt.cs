namespace Nzy3d.Plot3D.Builder.Delaunay.Jdt
{
	/// <summary>
	/// This class represents a 3D triangle in a Triangulation
	/// </summary>
	public class Triangle_dt
	{
		private Circle_dt _circum;

		//private int _counter = 0;

		//private int _c2 = 0;

		/// <summary>
		/// Constructs a triangle form 3 point - store it in counterclockwised order.
		/// A should be before B and B before C in counterclockwise order.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		public Triangle_dt(Point_dt a, Point_dt b, Point_dt c)
		{
			this.A = a;
			int res = c.pointLineTest(a, b);
			if (res <= Point_dt.LEFT || res == Point_dt.INFRONTOFA || res == Point_dt.BEHINDB)
			{
				this.B = b;
				this.C = c;
				//RIGHT
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Warning, Triangle_dt(A,B,C) expects points in counterclockwise order.");
				System.Diagnostics.Debug.WriteLine(a.ToString() + b.ToString() + c.ToString());
				this.B = c;
				this.C = b;
			}
			Circumcircle();
		}

		/// <summary>
		/// Creates a half plane using the segment (A,B).
		/// </summary>
		public Triangle_dt(Point_dt a, Point_dt b)
		{
			this.A = a;
			this.B = b;
			IsHalfplane = true;
		}

		/// <summary>
		/// Returns true if this triangle is actually a half plane
		/// </summary>
		public bool IsHalfplane { get; set; }

		/// <summary>
		/// tag - for bfs algorithms
		/// </summary>
		public bool Mark { get; set; }

		/// <summary>
		/// Returns the first vertex of this triangle.
		/// </summary>
		public Point_dt P1
		{
			get { return A; }
		}

		/// <summary>
		/// Returns the second vertex of this triangle.
		/// </summary>
		public Point_dt P2
		{
			get { return B; }
		}

		/// <summary>
		/// Returns the third vertex of this triangle.
		/// </summary>
		public Point_dt P3
		{
			get { return C; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p1,p2 edge.
		/// </summary>
		public Triangle_dt Next12
		{
			get { return ABNext; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p2,p3 edge.
		/// </summary>
		public Triangle_dt Next23
		{
			get { return BCNext; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p3,p1 edge.
		/// </summary>
		public Triangle_dt Next31
		{
			get { return CANext; }
		}

		/// <summary>
		/// The bounding rectange between the minimum and maximum coordinates of the triangle.
		/// </summary>
		public BoundingBox BoundingBox
		{
			get
			{
				var lowerLeft = new Point_dt(Math.Min(A.x, Math.Min(B.x, C.x)), Math.Min(A.y, Math.Min(B.y, C.y)));
				var upperRight = new Point_dt(Math.Max(A.x, Math.Max(B.x, C.x)), Math.Max(A.y, Math.Max(B.y, C.y)));
				return new BoundingBox(lowerLeft, upperRight);
			}
		}

		public void SwitchNeighbors(Triangle_dt old_t, Triangle_dt new_t)
		{
			if (ABNext.Equals(old_t))
			{
				ABNext = new_t;
			}
			else if (BCNext.Equals(old_t))
			{
				BCNext = new_t;
			}
			else if (CANext.Equals(old_t))
			{
				CANext = new_t;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Error, switchneighbors can't find Old.");
			}
		}

		public Triangle_dt Neighbor(Point_dt p)
		{
			if (A.Equals(p))
			{
				return CANext;
			}
			else if (B.Equals(p))
			{
				return ABNext;
			}
			else if (C.Equals(p))
			{
				return BCNext;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Error, neighbors can't find p.");
				return null;
			}
		}

		/// <summary>
		/// Returns the neighbors that shares the given corner and is not the previous triangle.
		/// </summary>
		/// <param name="p">The given corner.</param>
		/// <param name="prevTriangle">The previous triangle.</param>
		/// <returns>The neighbors that shares the given corner and is not the previous triangle.</returns>
		public object NextNeighbor(Point_dt p, Triangle_dt prevTriangle)
		{
			Triangle_dt neighbor = null;

			if (A.Equals(p))
			{
				neighbor = CANext;
			}
			else if (B.Equals(p))
			{
				neighbor = ABNext;
			}
			else if (C.Equals(p))
			{
				neighbor = BCNext;
			}

			if (neighbor.Equals(prevTriangle) || neighbor.IsHalfplane)
			{
				if (A.Equals(p))
				{
					neighbor = ABNext;
				}
				else if (B.Equals(p))
				{
					neighbor = BCNext;
				}
				else if (C.Equals(p))
				{
					neighbor = CANext;
				}
			}

			return neighbor;
		}

		public Circle_dt Circumcircle()
		{
			double u = ((A.x - B.x) * (A.x + B.x) + (A.y - B.y) * (A.y + B.y)) / 2.0;
			double v = ((B.x - C.x) * (B.x + C.x) + (B.y - C.y) * (B.y + C.y)) / 2.0;
			double den = (A.x - B.x) * (B.y - C.y) - (B.x - C.x) * (A.y - B.y);
			// oops, degenerate case
			if (den == 0)
			{
				_circum = new Circle_dt(A, double.PositiveInfinity);
			}
			else
			{
				Point_dt cen = new Point_dt((u * (B.y - C.y) - v * (A.y - B.y)) / den, (v * (A.x - B.x) - u * (B.x - C.x)) / den);
				_circum = new Circle_dt(cen, cen.distance2(A));
			}
			return _circum;
		}

		public bool CircumcircleContains(Point_dt p)
		{
			// Fix from https://github.com/rtrusso/nzy3d-api/commit/b39a673c522ac49a6727f9b7890a154824581d42
			if (IsHalfplane)
			{
				return false;
			}
			// End

			return _circum.Radius > _circum.Center.distance2(p);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			string res = "";
			res += "A: " + A.ToString() + " B: " + B.ToString();
			if ((!IsHalfplane))
			{
				res += " C: " + C.ToString();
			}
			return res;
		}

		/// <summary>
		/// Determines if this triangle contains the point p.
		/// </summary>
		/// <param name="p">The query point</param>
		/// <returns>True if p is not null and is inside this triangle</returns>
		/// <remarks>Note: on boundary is considered inside</remarks>
		public bool Contains(Point_dt p)
		{
			if (IsHalfplane || p == null)
			{
				return false;
			}

			if (IsCorner(p))
			{
				return true;
			}

			int a12 = p.pointLineTest(A, B);
			int a23 = p.pointLineTest(B, C);
			int a31 = p.pointLineTest(C, A);
			return (a12 == Point_dt.LEFT && a23 == Point_dt.LEFT && a31 == Point_dt.LEFT)
				|| (a12 == Point_dt.RIGHT && a23 == Point_dt.RIGHT && a31 == Point_dt.RIGHT)
				|| (a12 == Point_dt.ONSEGMENT || a23 == Point_dt.ONSEGMENT || a31 == Point_dt.ONSEGMENT);
		}

		/// <summary>
		/// Determines if this triangle contains the point p.
		/// </summary>
		/// <param name="p">The query point</param>
		/// <returns>True if p is not null and is inside this triangle</returns>
		/// <remarks>Note: on boundary is considered outside</remarks>
		public bool ContainsBoundaryIsOutside(Point_dt p)
		{
			if (IsHalfplane || p == null)
			{
				return false;
			}

			if (IsCorner(p))
			{
				return false;
			}

			int a12 = p.pointLineTest(A, B);
			int a23 = p.pointLineTest(B, C);
			int a31 = p.pointLineTest(C, A);
			return (a12 == Point_dt.LEFT && a23 == Point_dt.LEFT && a31 == Point_dt.LEFT)
				|| (a12 == Point_dt.RIGHT && a23 == Point_dt.RIGHT && a31 == Point_dt.RIGHT);
		}

		/// <summary>
		/// Checks if the given point is a corner of this triangle.
		/// </summary>
		/// <param name="p">The given point.</param>
		/// <returns>True if the given point is a corner of this triangle.</returns>
		public bool IsCorner(Point_dt p)
		{
			return (p.x == A.x && p.y == A.y) || (p.x == B.x && p.y == B.y) || (p.x == C.x && p.y == C.y);
		}

		/// <summary>
		/// compute the Z value for the X, Y values of q.
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <param name="q">A x/y point.</param>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public double ZValue(Point_dt q)
		{
			if (q == null)
			{
				throw new ArgumentException("Input point cannot be Nothing", "q");
			}

			if (IsHalfplane)
			{
				throw new Exception("Cannot approximate the z value from a halfplane triangle");
			}

			if (q.x == A.x && q.y == A.y)
			{
				return A.z;
			}

			if (q.x == B.x && q.y == B.y)
			{
				return B.z;
			}

			if (q.x == C.x && q.y == C.y)
			{
				return C.z;
			}

			double X = 0;
			double x0 = q.x;
			double x1 = A.x;
			double x2 = B.x;
			double x3 = C.x;
			double Y = 0;
			double y0 = q.y;
			double y1 = A.y;
			double y2 = B.y;
			double y3 = C.y;
			double Z = 0;
			double m01 = 0;
			double k01 = 0;
			double m23 = 0;
			double k23 = 0;

			// 0 - regular, 1-horizontal , 2-vertical
			int flag01 = 0;
			if (x0 != x1)
			{
				m01 = (y0 - y1) / (x0 - x1);
				k01 = y0 - m01 * x0;
				if (m01 == 0)
				{
					flag01 = 1;
				}
				//2-vertical
			}
			else
			{
				flag01 = 2;
				//x01 = x0
			}

			int flag23 = 0;
			if (x2 != x3)
			{
				m23 = (y2 - y3) / (x2 - x3);
				k23 = y2 - m23 * x2;
				if (m23 == 0)
				{
					flag23 = 1;
				}
				//2-vertical
			}
			else
			{
				flag23 = 2;
				//x01 = x0
			}

			if (flag01 == 2)
			{
				X = x0;
				Y = m23 * X + k23;
			}
			else if (flag23 == 2)
			{
				X = x2;
				Y = m01 * X + k01;
			}
			else
			{
				X = (k23 - k01) / (m01 - m23);
				Y = m01 * X + k01;
			}

			double r = 0;
			if (flag23 == 2)
			{
				r = (y2 - Y) / (y2 - y3);
			}
			else
			{
				r = (x2 - X) / (x2 - x3);
			}

			Z = B.z + (C.z - B.z) * r;
			if (flag01 == 2)
			{
				r = (y1 - y0) / (y1 - Y);
			}
			else
			{
				r = (x1 - x0) / (x1 - X);
			}

			double qZ = A.z + (Z - A.z) * r;
			return qZ;
		}

		/// <summary>
		/// Compute the Z value for the X, Y values 
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <returns></returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public double Z(double x, double y)
		{
			return ZValue(new Point_dt(x, y));
		}

		/// <summary>
		/// compute the Z value for the X, Y values of q.
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <param name="q">A x/y point.</param>
		/// <returns>A new <see cref="Point_dt"/> with same x/y than <paramref name="q"/> and computed z value</returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public Point_dt Z(Point_dt q)
		{
			double newz = ZValue(q);
			return new Point_dt(q.x, q.y, newz);
		}

		public Point_dt A { get; set; }

		public Point_dt B { get; set; }

		public Point_dt C { get; set; }

		public Triangle_dt ABNext { get; set; }

		public Triangle_dt BCNext { get; set; }

		public Triangle_dt CANext { get; set; }

		public int Mc { get; set; }

		public Circle_dt Circum
		{
			get { return _circum; }
		}
	}
}
