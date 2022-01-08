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
			reset();
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates
		/// </summary>
		public BoundingBox3d(List<Coord3d> lst)
		{
			reset();
			foreach (Coord3d c in lst)
			{
				@add(c);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates from a polygon
		/// </summary>
		public BoundingBox3d(Polygon pol)
		{
			reset();
			foreach (Point p in pol.GetPoints)
			{
				@add(p);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox with given centre and edgeLength (equals in all directions)
		/// </summary>
		public BoundingBox3d(Coord3d center, double edgeLength)
			: this(center.x - edgeLength / 2, center.x + edgeLength / 2, center.y - edgeLength / 2, center.y + edgeLength / 2, center.z - edgeLength / 2, center.z + edgeLength / 2)
		{
		}

		/// <summary>
		/// Initialize a BoundingBox with another bounding box (i.e. performs a copy)
		/// </summary>
		public BoundingBox3d(BoundingBox3d anotherBox)
			: this(anotherBox.xmin, anotherBox.xmax, anotherBox.ymin, anotherBox.ymax, anotherBox.zmin, anotherBox.zmax)
		{
		}

		/// <summary>
		/// Initialize a BoundingBox with raw values.
		/// </summary>
		public BoundingBox3d(double xmin, double xmax, double ymin, double ymax, double zmin, double zmax)
		{
			this.xmin = xmin;
			this.xmax = xmax;
			this.ymin = ymin;
			this.ymax = ymax;
			this.zmin = zmin;
			this.zmax = zmax;
		}

		/// <summary>
		///  Initialize the bounding box with Double.MAX_VALUE as minimum
		/// value, and Double.MIN_VALUE as maximum value for each dimension.
		/// </summary>
		public void reset()
		{
			xmin = double.MaxValue;
			xmax = double.MinValue;
			ymin = double.MaxValue;
			ymax = double.MinValue;
			zmin = double.MaxValue;
			zmax = double.MinValue;
		}

		/// <summary>
		/// Check if bounding box is valid (i.e. limits are consistents).
		/// </summary>
		public bool valid()
		{
			return (xmin <= xmax & ymin <= ymax & zmin <= zmax);
		}

		/// <summary>
		/// Adds an x,y,z point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void @add(double x, double y, double z)
		{
			if (x > xmax)
				xmax = x;
			if (x < xmin)
				xmin = x;
			if (y > ymax)
				ymax = y;
			if (y < ymin)
				ymin = y;
			if (z > zmax)
				zmax = z;
			if (z < zmin)
				zmin = z;
		}

		/// <summary>
		/// Adds a <see cref="Coord3d"/> point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void @add(Coord3d p)
		{
			this.@add(p.x, p.y, p.z);
		}

		/// <summary>
		/// Adds a set of coordinates from a polygon to the bounding box
		/// </summary>
		public void @add(Polygon pol)
		{
			foreach (Point p in pol.GetPoints)
			{
				@add(p);
			}
		}

		/// <summary>
		/// Adds a point to the bounding box
		/// </summary>
		public void @add(Point p)
		{
			@add(p.xyz.x, p.xyz.y, p.xyz.z);
		}

		/// <summary>
		/// Adds another <see cref="BoundingBox3d"/> to the bounding box, and enlarge the bounding
		/// box if its points lies outside of it (i.e. merge other bounding box inside current one)
		/// </summary>
		public void Add(BoundingBox3d b)
		{
			this.@add(b.xmin, b.ymin, b.zmin);
			this.@add(b.xmax, b.ymax, b.zmax);
		}

		/// <summary>
		/// Compute and return the center point of the BoundingBox3d
		/// </summary>
		public Coord3d getCenter()
		{
			return new Coord3d((xmax + xmin) / 2, (ymax + ymin) / 2, (zmax + zmin) / 2);
		}

		/// <summary>
		/// Return the radius of the Sphere containing the Bounding Box,
		/// i.e., the distance between the center and the point (xmin, ymin, zmin).
		/// </summary>
		public double getRadius()
		{
			return getCenter().distance(new Coord3d(xmin, ymin, zmin));
		}

		/// <summary>
		/// Return a copy of the current bounding box after scaling all limits relative to 0,0,0
		/// Scaling does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d scale(Coord3d factors)
		{
			var b = new BoundingBox3d();
			b.xmax = xmax * factors.x;
			b.xmin = xmin * factors.x;
			b.ymax = ymax * factors.y;
			b.ymin = ymin * factors.y;
			b.zmax = zmax * factors.z;
			b.zmin = zmin * factors.z;
			return b;
		}

		/// <summary>
		/// Return a copy of the current bounding box after shitfing all limits
		/// Shifting does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d shift(Coord3d offset)
		{
			var b = new BoundingBox3d();
			b.xmax = xmax + offset.x;
			b.xmin = xmin + offset.x;
			b.ymax = ymax + offset.y;
			b.ymin = ymin + offset.y;
			b.zmax = zmax + offset.z;
			b.zmin = zmin + offset.z;
			return b;
		}

		/// <summary>
		/// Return a copy of the current bounding box after adding a margin to all limits (positiv to max limits, negativ to min limits)
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d margin(double marg)
		{
			var b = new BoundingBox3d();
			b.xmax = xmax + marg;
			b.xmin = xmin - marg;
			b.ymax = ymax + marg;
			b.ymin = ymin - marg;
			b.zmax = zmax + marg;
			b.zmin = zmin - marg;
			return b;
		}

		/// <summary>
		/// Return a copy of the current bounding box after adding a margin to all limits (positiv to max limits, negativ to min limits)
		/// </summary>
		/// <remarks>Modify current object.</remarks>
		public void selfMargin(double marg)
		{
			var b = new BoundingBox3d();
			xmax += marg;
			xmin -= marg;
			ymax += marg;
			ymin -= marg;
			zmax += marg;
			zmin -= marg;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> is contained in this box.
		/// </summary>
		/// <remarks>if b1.contains(b2), then b1.intersect(b2) as well.</remarks>
		public bool contains(BoundingBox3d anotherBox)
		{
			return xmin <= anotherBox.xmin & anotherBox.xmax <= xmax & ymin <= anotherBox.ymin & anotherBox.ymax <= ymax & zmin <= anotherBox.zmin & anotherBox.zmax <= zmax;
		}

		/// <summary>
		/// Return true if <paramref name="aPoint"/> is contained in this box.
		/// </summary>
		public bool contains(Coord3d aPoint)
		{
			return xmin <= aPoint.x & aPoint.x <= xmax & ymin <= aPoint.y & aPoint.y <= ymax & zmin <= aPoint.z & aPoint.z <= zmax;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> intersects with this box.
		/// </summary>
		public bool intersect(BoundingBox3d anotherBox)
		{
			return (xmin <= anotherBox.xmin & anotherBox.xmin <= xmax) | (xmin <= anotherBox.xmax & anotherBox.xmax <= xmax) & (ymin <= anotherBox.ymin & anotherBox.ymin <= ymax) | (ymin <= anotherBox.ymax & anotherBox.ymax <= ymax) & (zmin <= anotherBox.zmin & anotherBox.zmin <= zmax) | (zmin <= anotherBox.zmax & anotherBox.zmax <= zmax);
		}

		/// <summary>
		/// Bounding box min x value
		/// </summary>
		public double xmin { get; set; }

		/// <summary>
		/// Bounding box max x value
		/// </summary>
		public double xmax { get; set; }

		/// <summary>
		/// Bounding box min y value
		/// </summary>
		public double ymin { get; set; }

		/// <summary>
		/// Bounding box max y value
		/// </summary>
		public double ymax { get; set; }

		/// <summary>
		/// Bounding box min z value
		/// </summary>
		public double zmin { get; set; }

		/// <summary>
		/// Bounding box max z value
		/// </summary>
		public double zmax { get; set; }

		public List<Coord3d> Vertices
		{
			get
			{
				List<Coord3d> edges = new List<Coord3d>();
				edges.Add(new Coord3d(xmin, ymin, zmin));
				edges.Add(new Coord3d(xmin, ymax, zmin));
				edges.Add(new Coord3d(xmax, ymax, zmin));
				edges.Add(new Coord3d(xmax, ymin, zmin));
				edges.Add(new Coord3d(xmin, ymin, zmax));
				edges.Add(new Coord3d(xmin, ymax, zmax));
				edges.Add(new Coord3d(xmax, ymax, zmax));
				edges.Add(new Coord3d(xmax, ymin, zmax));
				return edges;
			}
		}

		public static BoundingBox3d newBoundsAtOrign()
		{
			return new BoundingBox3d(Coord3d.ORIGIN, 0);
		}

		public override string ToString()
		{
			return "(BoundingBox3d)" + xmin + "<=x<=" + xmax + " | " + ymin + "<=y<=" + ymax + " | " + zmin + "<=y<=" + zmax;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
