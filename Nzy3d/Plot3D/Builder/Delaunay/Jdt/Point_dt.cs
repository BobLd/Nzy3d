using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Builder.Delaunay.Jdt
{
	/// <summary>
	/// This class represents a 3D point, with some simple geometric methods (pointLineTest).
	/// </summary>
	public sealed class Point_dt
	{
		public const int ONSEGMENT = 0;
		public const int LEFT = 1;
		public const int RIGHT = 2;
		public const int INFRONTOFA = 3;
		public const int BEHINDB = 4;
		public const int ISERROR = 5;

		/// <summary>
		/// Default Constructor. Constructs a 3D point at (0,0,0).
		/// </summary>
		public Point_dt() : this(0, 0)
		{
		}

		/// <summary>
		/// Constructs a 3D point at (x,y,z)
		/// </summary>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		/// <param name="z">z coordinates</param>
		public Point_dt(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>
		/// Constructs a 3D point at (x,y,0)
		/// </summary>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		public Point_dt(double x, double y) : this(x, y, 0)
		{
		}

		/// <summary>
		/// Simple copy constructor
		/// </summary>
		/// <param name="p">Another point</param>
		public Point_dt(Point_dt p) : this(p.X, p.Y, p.Z)
		{
		}

		/// <summary>
		/// X coordinate.
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// Y coordinate.
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Z coordinate.
		/// </summary>
		public double Z { get; set; }

		/// <summary>
		/// (X, Y, Z) point.
		/// </summary>
		public Coord3d Coord3d
		{
			get { return new Coord3d(X, Y, Z); }
		}

		public double Distance2(Point_dt p)
		{
			return (p.X - X) * (p.X - X) + (p.Y - Y) * (p.Y - Y);
		}

		public double Distance2(double px, double py)
		{
			return (px - X) * (px - X) + (py - Y) * (py - Y);
		}

		public bool IsLess(Point_dt p)
		{
			return X < p.X || (X == p.X && Y < p.Y);
		}

		public bool IsGreater(Point_dt p)
		{
			return X > p.X || (X == p.X && Y > p.Y);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return "(Point_dt) [" + X + "," + Y + "," + Z + "]";
		}

		public double Distance(Point_dt p)
		{
			return Math.Sqrt((p.X - X) * (p.X - X) + (p.Y - Y) * (p.Y - Y));
		}

		public double Distance3D(Point_dt p)
		{
			return Math.Sqrt((p.X - X) * (p.X - X) + (p.Y - Y) * (p.Y - Y) + (p.Z - Z) * (p.Z - Z));
		}

		/// <summary>
		/// Tests the relation between current point (as a 2D [x,y] point) and a 2D
		/// segment a,b (the Z values are ignored), returns one of the following:
		/// LEFT, RIGHT, INFRONTOFA, BEHINDB, ONSEGMENT, ISERROR
		/// </summary>
		/// <param name="a">The first point of the segment</param>
		/// <param name="b">The second point of the segment</param>
		/// <returns>The value (flag) of the relation between this point and the a,b line-segment.</returns>
		public int PointLineTest(Point_dt a, Point_dt b)
		{
			double dx = b.X - a.X;
			double dy = b.Y - a.Y;
			double res = dy * (X - a.X) - dx * (Y - a.Y);

			if (res < 0)
			{
				return LEFT;
			}

			if (res > 0)
			{
				return RIGHT;
			}

			if (dx > 0)
			{
				if (X < a.X)
				{
					return INFRONTOFA;
				}

				if (b.X < X)
				{
					return BEHINDB;
				}
				return ONSEGMENT;
			}

			if (dx < 0)
			{
				if (X > a.X)
				{
					return INFRONTOFA;
				}

				if (b.X > X)
				{
					return BEHINDB;
				}
				return ONSEGMENT;
			}

			if (dy > 0)
			{
				if (Y < a.Y)
				{
					return INFRONTOFA;
				}

				if (b.Y < Y)
				{
					return BEHINDB;
				}
				return ONSEGMENT;
			}

			if (dy < 0)
			{
				if (Y > a.Y)
				{
					return INFRONTOFA;
				}

				if (b.Y > Y)
				{
					return BEHINDB;
				}
				return ONSEGMENT;
			}
			return ISERROR;
		}

		public Point_dt Circumcenter(Point_dt a, Point_dt b)
		{
			double u = ((a.X - b.X) * (a.X + b.X) + (a.Y - b.Y) * (a.Y + b.Y)) / 2.0;
			double v = ((b.X - X) * (b.X + X) + (b.Y - Y) * (b.Y + Y)) / 2.0;
			double den = (a.X - b.X) * (b.Y - Y) - (b.X - X) * (a.Y - b.Y);
			if (den == 0)
			{
				// oops
				System.Diagnostics.Debug.WriteLine("circumcenter, degenerate case");
			}
			return new Point_dt((u * (b.Y - Y) - v * (a.Y - b.Y)) / den, (v * (a.X - b.X) - u * (b.X - X)) / den);
		}

		public object Comparator(int flag)
		{
			return new Point_dt_Compare(flag);
		}

		public object Comparator()
		{
			return new Point_dt_Compare(0);
		}
	}

	internal class Point_dt_Compare
	{
		private readonly int m_flag;

		public Point_dt_Compare(int flag)
		{
			m_flag = flag;
		}

		public object Compare(Point_dt o1, Point_dt o2)
		{
			if (!(o1 == null || o2 == null))
			{
				if (m_flag == 0)
				{
					if (o1.X > o2.X)
					{
						return 1;
					}

					if (o1.X < o2.X)
					{
						return -1;
					}

					// x1 == x2
					if (o1.Y > o2.Y)
					{
						return 1;
					}

					if (o1.Y < o2.Y)
					{
						return -1;
					}
				}
				else if (m_flag == 1)
				{
					if (o1.X > o2.X)
					{
						return -1;
					}

					if (o1.X < o2.X)
					{
						return 1;
					}

					// x1 == x2
					if (o1.Y > o2.Y)
					{
						return -1;
					}

					if (o1.Y < o2.Y)
					{
						return 1;
					}
				}
				else if (m_flag == 2)
				{
					if (o1.Y > o2.Y)
					{
						return 1;
					}

					if (o1.Y < o2.Y)
					{
						return -1;
					}

					// y1 == y2
					if (o1.X > o2.X)
					{
						return 1;
					}

					if (o1.X < o2.X)
					{
						return -1;
					}
				}
                else if (m_flag == 3)
				{
					if (o1.Y > o2.Y)
					{
						return -1;
					}

					if (o1.Y < o2.Y)
					{
						return 1;
					}

					// y1 == y2
					if (o1.X > o2.X)
					{
						return -1;
					}

					if (o1.X < o2.X)
					{
						return 1;
					}
				}
			}
			else
			{
				if (o1 == null && o2 == null)
				{
					return 0;
				}

				if (o1 == null && (o2 != null))
				{
					return 1;
				}

				if ((o1 != null) && o2 == null)
				{
					return -1;
				}
			}
			throw new Exception("Unexpected behavior, comparer should never have reached this point");
		}
	}
}
