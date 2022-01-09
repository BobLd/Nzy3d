using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Maths
{
	/// <summary>
	/// A BoundingBox3d stores a couple of maximal and minimal limit on
	/// each dimension (x, y) in cartesian coordinates. It provides functions for enlarging
	/// the box by adding cartesian coordinates or an other
	/// BoundingBox3d (that is equivalent to computing the union of the
	/// current BoundingBox and another one).
	/// </summary>
	public class BoundingBox3d
	{
		/// <summary>
		/// Initialize a BoundingBox by calling its reset method.
		/// </summary>
		public BoundingBox3d()
		{
			Reset();
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates
		/// </summary>
		public BoundingBox3d(List<Coord3d> lst)
		{
			Reset();
			foreach (Coord3d c in lst)
			{
				Add(c);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates from a polygon
		/// </summary>
		public BoundingBox3d(Polygon pol)
		{
			Reset();
			foreach (Point p in pol.GetPoints)
			{
				Add(p);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox with given centre and edgeLength (equals in all directions)
		/// </summary>
		public BoundingBox3d(Coord3d center, float edgeLength)
			: this(center.X - edgeLength / 2, center.X + edgeLength / 2, center.Y - edgeLength / 2, center.Y + edgeLength / 2, center.Z - edgeLength / 2, center.Z + edgeLength / 2)
		{
		}

		/// <summary>
		/// Initialize a BoundingBox with another bounding box (i.e. performs a copy)
		/// </summary>
		public BoundingBox3d(BoundingBox3d anotherBox)
			: this(anotherBox.XMin, anotherBox.XMax, anotherBox.YMin, anotherBox.YMax, anotherBox.ZMin, anotherBox.ZMax)
		{
		}

		/// <summary>
		/// Initialize a BoundingBox with raw values.
		/// </summary>
		public BoundingBox3d(float xmin, float xmax, float ymin, float ymax, float zmin, float zmax)
		{
			this.XMin = xmin;
			this.XMax = xmax;
			this.YMin = ymin;
			this.YMax = ymax;
			this.ZMin = zmin;
			this.ZMax = zmax;
		}

		/// <summary>
		///  Initialize the bounding box with Double.MAX_VALUE as minimum
		/// value, and Double.MIN_VALUE as maximum value for each dimension.
		/// </summary>
		public void Reset()
		{
			XMin = float.MaxValue;
			XMax = float.MinValue;
			YMin = float.MaxValue;
			YMax = float.MinValue;
			ZMin = float.MaxValue;
			ZMax = float.MinValue;
		}

		/// <summary>
		/// Check if bounding box is valid (i.e. limits are consistents).
		/// </summary>
		public bool IsValid()
		{
			return XMin <= XMax && YMin <= YMax && ZMin <= ZMax;
		}

		/// <summary>
		/// Adds an x,y,z point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void Add(float x, float y, float z)
		{
			if (x > XMax)
			{
				XMax = x;
			}

			if (x < XMin)
			{
				XMin = x;
			}

			if (y > YMax)
			{
				YMax = y;
			}

			if (y < YMin)
			{
				YMin = y;
			}

			if (z > ZMax)
			{
				ZMax = z;
			}

			if (z < ZMin)
			{
				ZMin = z;
			}
		}

		/// <summary>
		/// Adds a <see cref="Coord3d"/> point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void Add(Coord3d p)
		{
			this.Add(p.X, p.Y, p.Z);
		}

		/// <summary>
		/// Adds a set of coordinates from a polygon to the bounding box
		/// </summary>
		public void Add(Polygon pol)
		{
			foreach (Point p in pol.GetPoints)
			{
				Add(p);
			}
		}

		/// <summary>
		/// Adds a point to the bounding box
		/// </summary>
		public void Add(Point p)
		{
			Add(p.XYZ.X, p.XYZ.Y, p.XYZ.Z);
		}

		/// <summary>
		/// Adds another <see cref="BoundingBox3d"/> to the bounding box, and enlarge the bounding
		/// box if its points lies outside of it (i.e. merge other bounding box inside current one)
		/// </summary>
		public void Add(BoundingBox3d b)
		{
			this.Add(b.XMin, b.YMin, b.ZMin);
			this.Add(b.XMax, b.YMax, b.ZMax);
		}

		/// <summary>
		/// Compute and return the center point of the BoundingBox3d
		/// </summary>
		public Coord3d GetCenter()
		{
			return new Coord3d((XMax + XMin) / 2, (YMax + YMin) / 2, (ZMax + ZMin) / 2);
		}

		/// <summary>
		/// Return the radius of the Sphere containing the Bounding Box,
		/// i.e., the distance between the center and the point (xmin, ymin, zmin).
		/// </summary>
		public double GetRadius()
		{
			return GetCenter().Distance(new Coord3d(XMin, YMin, ZMin));
		}

		/// <summary>
		/// Return a copy of the current bounding box after scaling all limits relative to 0,0,0
		/// Scaling does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d Scale(Coord3d factors)
		{
            return new BoundingBox3d
            {
                XMax = XMax * factors.X,
                XMin = XMin * factors.X,
                YMax = YMax * factors.Y,
                YMin = YMin * factors.Y,
                ZMax = ZMax * factors.Z,
                ZMin = ZMin * factors.Z
            };
		}

		/// <summary>
		/// Return a copy of the current bounding box after shitfing all limits
		/// Shifting does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d Shift(Coord3d offset)
		{
			return new BoundingBox3d
			{
				XMax = XMax + offset.X,
				XMin = XMin + offset.X,
				YMax = YMax + offset.Y,
				YMin = YMin + offset.Y,
				ZMax = ZMax + offset.Z,
				ZMin = ZMin + offset.Z
			};
		}

		/// <summary>
		/// Return a copy of the current bounding box after adding a margin to all limits (positiv to max limits, negativ to min limits)
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d Margin(float marg)
		{
			return new BoundingBox3d
			{
				XMax = XMax + marg,
				XMin = XMin - marg,
				YMax = YMax + marg,
				YMin = YMin - marg,
				ZMax = ZMax + marg,
				ZMin = ZMin - marg
			};
		}

		/// <summary>
		/// <para>Add a margin to max values and substract a margin to min values, where the margin is ratio of the current range of each dimension.</para>
		/// <para>Adding a margin of 10% for each dimension is done with <see cref="MarginRatio(0.1)"/></para>
		/// </summary>
		/// <param name="marginRatio"></param>
		/// <returns>a new bounding box</returns>
		public BoundingBox3d MarginRatio(float marginRatio)
		{
			float xMargin = (float)(XMax - XMin) * marginRatio;
			float yMargin = (float)(YMax - YMin) * marginRatio;
			float zMargin = (float)(ZMax - ZMin) * marginRatio;

            return new BoundingBox3d
            {
                XMax = XMax + xMargin,
                XMin = XMin - xMargin,
                YMax = YMax + yMargin,
                YMin = YMin - yMargin,
                ZMax = ZMax + zMargin,
                ZMin = ZMin - zMargin
            };
		}

		/// <summary>
		/// Return a copy of the current bounding box after adding a margin to all limits (positiv to max limits, negativ to min limits)
		/// </summary>
		/// <remarks>Modify current object.</remarks>
		public void SelfMargin(float marg)
		{
			XMax += marg;
			XMin -= marg;
			YMax += marg;
			YMin -= marg;
			ZMax += marg;
			ZMin -= marg;
		}

		public void SelfMarginRatio(float marginRatio)
		{
			float xMargin = (float)(XMax - XMin) * marginRatio;
			float yMargin = (float)(YMax - YMin) * marginRatio;
			float zMargin = (float)(ZMax - ZMin) * marginRatio;

			XMax += xMargin;
			XMin -= xMargin;
			YMax += yMargin;
			YMin -= yMargin;
			ZMax += zMargin;
			ZMin -= zMargin;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> is contained in this box.
		/// </summary>
		/// <remarks>if b1.contains(b2), then b1.intersect(b2) as well.</remarks>
		public bool Contains(BoundingBox3d anotherBox)
		{
			return XMin <= anotherBox.XMin && anotherBox.XMax <= XMax && YMin <= anotherBox.YMin && anotherBox.YMax <= YMax && ZMin <= anotherBox.ZMin && anotherBox.ZMax <= ZMax;
		}

		/// <summary>
		/// Return true if <paramref name="aPoint"/> is contained in this box.
		/// </summary>
		public bool Contains(Coord3d aPoint)
		{
			return XMin <= aPoint.X && aPoint.X <= XMax && YMin <= aPoint.Y && aPoint.Y <= YMax && ZMin <= aPoint.Z && aPoint.Z <= ZMax;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> intersects with this box.
		/// </summary>
		public bool Intersect(BoundingBox3d anotherBox)
		{
			//return (XMin <= anotherBox.XMin && anotherBox.XMin <= XMax)
			//|| XMin <= anotherBox.XMax && anotherBox.XMax <= XMax && YMin <= anotherBox.YMin && anotherBox.YMin <= YMax ||
			//		YMin <= anotherBox.YMax && anotherBox.YMax <= YMax && ZMin <= anotherBox.ZMin && anotherBox.ZMin <= ZMax || (ZMin <= anotherBox.ZMax && anotherBox.ZMax <= ZMax);

			return ((XMin <= anotherBox.XMin && anotherBox.XMin <= XMax) || (XMin <= anotherBox.XMax && anotherBox.XMax <= XMax)) &&
				   ((YMin <= anotherBox.YMin && anotherBox.YMin <= YMax) || (YMin <= anotherBox.YMax && anotherBox.YMax <= YMax)) &&
				   ((ZMin <= anotherBox.ZMin && anotherBox.ZMin <= ZMax) || (ZMin <= anotherBox.ZMax && anotherBox.ZMax <= ZMax));
		}

		/// <summary>
		/// Bounding box min x value
		/// </summary>
		public float XMin { get; set; }

		/// <summary>
		/// Bounding box max x value
		/// </summary>
		public float XMax { get; set; }

		/// <summary>
		/// Bounding box min y value
		/// </summary>
		public float YMin { get; set; }

		/// <summary>
		/// Bounding box max y value
		/// </summary>
		public float YMax { get; set; }

		/// <summary>
		/// Bounding box min z value
		/// </summary>
		public float ZMin { get; set; }

		/// <summary>
		/// Bounding box max z value
		/// </summary>
		public float ZMax { get; set; }

		public List<Coord3d> Vertices
		{
			get
			{
				return new List<Coord3d>
				{
					new Coord3d(XMin, YMin, ZMin),
					new Coord3d(XMin, YMax, ZMin),
					new Coord3d(XMax, YMax, ZMin),
					new Coord3d(XMax, YMin, ZMin),
					new Coord3d(XMin, YMin, ZMax),
					new Coord3d(XMin, YMax, ZMax),
					new Coord3d(XMax, YMax, ZMax),
					new Coord3d(XMax, YMin, ZMax)
				};
			}
		}

		public static BoundingBox3d NewBoundsAtOrign()
		{
			return new BoundingBox3d(Coord3d.ORIGIN, 0);
		}

		public override string ToString()
		{
			return "(BoundingBox3d)" + XMin + "<=x<=" + XMax + " | " + YMin + "<=y<=" + YMax + " | " + ZMin + "<=y<=" + ZMax;
		}
	}
}
