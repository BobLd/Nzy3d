namespace Nzy3d.Plot3D.Builder.Delaunay.Jdt
{
	/// <summary>
	/// <para>
	/// This class represents a Delaunay Triangulation. The class was written for a
	/// large scale triangulation (1000 - 200,000 vertices). The application main use
	/// is 3D surface (terrain) presentation.
	/// The class main properties are the following:
	/// - fast point location. (O(n^0.5)), practical runtime is often very fast.
	/// - handles degenerate cases and none general position input (ignores duplicate
	/// points).
	/// - save &amp; load from\to text file in TSIN format.
	/// - 3D support: including z value approximation.
	/// - standard java (1.5 generic) iterators for the vertices and triangles.
	/// - smart iterator to only the updated triangles - for terrain simplification
	/// </para>
	/// <para>
	/// Testing (done in early 2005): Platform java 1.5.02 windows XP (SP2), AMD
	/// laptop 1.6G sempron CPU 512MB RAM. Constructing a triangulation of 100,000
	/// vertices takes ~ 10 seconds. point location of 100,000 points on a
	/// triangulation of 100,000 vertices takes ~ 5 seconds.
	/// </para>
	/// <para>
	/// Note: constructing a triangulation with 200,000 vertices and more requires
	/// extending java heap size (otherwise an exception will be thrown).
	/// </para>
	/// <para>
	/// Bugs: if U find a bug or U have an idea as for how to improve the code,
	/// please send me an email to: benmo@ariel.ac.il
	/// </para>
	/// <para>
	/// @author Boaz Ben Moshe 5/11/05
	///         The project uses some ideas presented in the VoroGuide project,
	///         written by Klasse f?r Kreise (1996-1997), For the original applet
	///         see: http://www.pi6.fernuni-hagen.de/GeomLab/VoroGlide/.
	/// </para>
	/// </summary>
	public class Delaunay_Triangulation : ITriangulation
	{
		// The first and last points (used only for first step construction)
		private Point_dt firstP;
		private Point_dt lastP;

		// for degenerate case!
		private bool allColinear;

		//the first and last triangles (used only for first step construction)
		private Triangle_dt firstT;
		private Triangle_dt lastT;
		private Triangle_dt currT;

		// the triangle the fond (search start from
		private Triangle_dt startTriangle;

		// the triangle the convex hull starts from
		public Triangle_dt startTriangleHull;

		// number of points
		private int nPoints;

		// additional data 4/8/05 used by the iterators
		private readonly List<Point_dt> _vertices;
		private List<Triangle_dt> _triangles;

		// The triangles that were deleted in the last deletePoint iteration.
		//Private deletedTriangles As List(Of Triangle_dt)
		// The triangles that were added in the last deletePoint iteration.
		//Private addedTriangles As List(Of Triangle_dt)
		private int _modCount;
		private int _modCount2;

		// the Bounding Box, {{x0,y0,z0} , {x1,y1,z1}}
		private Point_dt _bb_min;
		private Point_dt _bb_max;

		// Index for faster point location searches

		private GridIndex gridIndex = null;

		public Delaunay_Triangulation() : this(Array.Empty<Point_dt>())
		{
		}

		public Delaunay_Triangulation(Point_dt[] ps)
		{
			_modCount = 0;
			_modCount2 = 0;
			_bb_min = null;
			_bb_max = null;
			_vertices = new List<Point_dt>();
			_triangles = new List<Triangle_dt>();
			//deletedTriangles = Nothing
			//addedTriangles = New List(Of Triangle_dt)
			allColinear = true;

			for (int i = 0; i <= ps.Length - 1; i++)
			{
				InsertPoint(ps[i]);
			}
		}

		/// <summary>
		/// Returns he number of vertices in this triangulation.
		/// </summary>
		public int Size()
		{
			if (_vertices == null) return 0;
			return _vertices.Count;
		}

		/// <summary>
		/// Returns the number of triangles in the triangulation, including infinite faces
		/// </summary>
		public int TrianglesSize()
		{
			InitTriangles();
			return _triangles.Count;
		}

		int ITriangulation.TrianglesSize()
		{
			return TrianglesSize();
		}

		/// <summary>
		/// Returns the changes counter for this triangulation
		/// </summary>
		public int ModeCounter
		{
			get { return _modCount; }
		}

		public void InsertPoint(Point_dt p)
		{
			if (_vertices.Contains(p)) return;

			_modCount++;
			UpdateBoundingBox(p);
			_vertices.Add(p);
			Triangle_dt t = InsertPointSimple(p);

			if (t == null) return;

			Triangle_dt tt = t;
			currT = t;

			// recall the last point for fast (last) update iterator
			do
			{
				Flip(tt, _modCount);
				tt = tt.CANext;
			} while (tt.Equals(t) && !tt.IsHalfplane);

			gridIndex?.UpdateIndex(GetLastUpdatedTriangles());
		}

		public IEnumerator<Triangle_dt> GetLastUpdatedTriangles()
		{
			var tmp = new List<Triangle_dt>();
			if (this.TrianglesSize() > 1)
			{
				Triangle_dt t = currT;
				AllTriangles(t, tmp, this._modCount);
			}
			return tmp.GetEnumerator();
		}

		private void AllTriangles(Triangle_dt curr, List<Triangle_dt> front, int mc)
		{
			if ((curr != null) && curr.Mc == mc && (!front.Contains(curr)))
			{
				front.Add(curr);
				AllTriangles(curr.ABNext, front, mc);
				AllTriangles(curr.BCNext, front, mc);
				AllTriangles(curr.CANext, front, mc);
			}
		}

		private Triangle_dt InsertPointSimple(Point_dt p)
		{
			nPoints++;
			if (!allColinear)
			{
				Triangle_dt t = Find(startTriangle, p);
				if (t.IsHalfplane)
				{
					startTriangle = ExtendOutside(t, p);
				}
				else
				{
					startTriangle = ExtendInside(t, p);
				}
				return startTriangle;
			}

			if (nPoints == 1)
			{
				firstP = p;
				return null;
			}

			if (nPoints == 2)
			{
				StartTriangulation(firstP, p);
				return null;
			}

			switch (p.PointLineTest(firstP, lastP))
			{
				case Point_dt.LEFT:
					startTriangle = ExtendOutside(firstT.ABNext, p);
					allColinear = false;
					break; // TODO: might not be correct. Was : Exit Select

				case Point_dt.RIGHT:
					startTriangle = ExtendOutside(firstT, p);
					allColinear = false;
					break; // TODO: might not be correct. Was : Exit Select

				case Point_dt.ONSEGMENT:
					InsertCollinear(p, Point_dt.ONSEGMENT);
					break; // TODO: might not be correct. Was : Exit Select

				case Point_dt.INFRONTOFA:
					InsertCollinear(p, Point_dt.INFRONTOFA);
					break; // TODO: might not be correct. Was : Exit Select

				case Point_dt.BEHINDB:
					InsertCollinear(p, Point_dt.BEHINDB);
					break; // TODO: might not be correct. Was : Exit Select
			}
			return null;
		}

		private void InsertCollinear(Point_dt p, int res)
		{
			Triangle_dt t;
			Triangle_dt tp;

			switch (res)
			{
				case Point_dt.INFRONTOFA:
					t = new Triangle_dt(firstP, p);
					tp = new Triangle_dt(p, firstP);
					t.ABNext = tp;
					tp.ABNext = t;
					t.BCNext = tp;
					tp.CANext = t;
					t.CANext = firstT;
					firstT.BCNext = t;
					tp.BCNext = firstT.ABNext;
					firstT.ABNext.CANext = tp;
					firstT = t;
					firstP = p;
					break; // TODO: might not be correct. Was : Exit Select

				case Point_dt.BEHINDB:
					t = new Triangle_dt(p, lastP);
					tp = new Triangle_dt(lastP, p);
					t.ABNext = tp;
					tp.ABNext = t;
					t.BCNext = lastT;
					lastT.CANext = t;
					t.CANext = tp;
					tp.BCNext = t;
					tp.CANext = lastT.ABNext;
					lastT.ABNext.BCNext = tp;
					lastT = t;
					lastP = p;
					break; // TODO: might not be correct. Was : Exit Select

				case Point_dt.ONSEGMENT:
					Triangle_dt u = firstT;
					while (p.IsGreater(u.A))
					{
						u = u.CANext;
					}
					t = new Triangle_dt(p, u.B);
					tp = new Triangle_dt(u.B, p);
					u.B = p;
					u.ABNext.A = p;
					t.ABNext = tp;
					tp.ABNext = t;
					t.BCNext = u.BCNext;
					u.BCNext.CANext = t;
					t.CANext = u;
					u.BCNext = t;
					tp.CANext = u.ABNext.CANext;
					u.ABNext.CANext.BCNext = tp;
					tp.BCNext = u.ABNext;
					u.ABNext.CANext = tp;

					if (firstT.Equals(u))
					{
						firstT = t;
					}

					break; // TODO: might not be correct. Was : Exit Select
			}
		}

		private void StartTriangulation(Point_dt p1, Point_dt p2)
		{
			Point_dt pb;
			Point_dt ps;

			if (p1.IsLess(p2))
			{
				ps = p1;
				pb = p2;
			}
			else
			{
				ps = p2;
				pb = p1;
			}

			firstT = new Triangle_dt(pb, ps);
			lastT = firstT;
			Triangle_dt t = new Triangle_dt(ps, pb);
			firstT.ABNext = t;
			t.ABNext = firstT;
			firstT.BCNext = t;
			t.CANext = firstT;
			firstT.CANext = t;
			t.BCNext = firstT;
			firstP = firstT.B;
			lastP = lastT.A;
			startTriangleHull = firstT;
		}

		private Triangle_dt ExtendInside(Triangle_dt t, Point_dt p)
		{
			Triangle_dt h1;
			Triangle_dt h2;

			h1 = TreatDegeneracyInside(t, p);

			if (h1 != null) return h1;

			h1 = new Triangle_dt(t.C, t.A, p);
			h2 = new Triangle_dt(t.B, t.C, p);
			t.C = p;
			t.Circumcircle();
			h1.ABNext = t.CANext;
			h1.BCNext = t;
			h1.CANext = h2;
			h2.ABNext = t.BCNext;
			h2.BCNext = h1;
			h2.CANext = t;
			h1.ABNext.SwitchNeighbors(t, h1);
			h2.ABNext.SwitchNeighbors(t, h2);
			t.BCNext = h2;
			t.CANext = h1;
			return t;
		}

		private Triangle_dt TreatDegeneracyInside(Triangle_dt t, Point_dt p)
		{
			if (t.ABNext.IsHalfplane && p.PointLineTest(t.B, t.A) == Point_dt.ONSEGMENT)
			{
				return ExtendOutside(t.ABNext, p);
			}

			if (t.BCNext.IsHalfplane && p.PointLineTest(t.C, t.B) == Point_dt.ONSEGMENT)
			{
				return ExtendOutside(t.BCNext, p);
			}

			if (t.CANext.IsHalfplane && p.PointLineTest(t.A, t.C) == Point_dt.ONSEGMENT)
			{
				return ExtendOutside(t.CANext, p);
			}

			return null;
		}

		private Triangle_dt ExtendOutside(Triangle_dt t, Point_dt p)
		{
			if (p.PointLineTest(t.A, t.B) == Point_dt.ONSEGMENT)
			{
				Triangle_dt dg = new Triangle_dt(t.A, t.B, p);
				Triangle_dt hp = new Triangle_dt(p, t.B);
				t.B = p;
				dg.ABNext = t.ABNext;
				dg.ABNext.SwitchNeighbors(t, dg);
				dg.BCNext = hp;
				hp.ABNext = dg;
				dg.CANext = t;
				t.ABNext = dg;
				hp.BCNext = t.BCNext;
				hp.BCNext.CANext = hp;
				hp.CANext = t;
				t.BCNext = hp;
				return dg;
			}

			Triangle_dt ccT = Extendcounterclock(t, p);
			Triangle_dt cT = ExtendClock(t, p);
			ccT.BCNext = cT;
			cT.CANext = ccT;
			startTriangleHull = cT;
			return cT.ABNext;
		}

		private Triangle_dt Extendcounterclock(Triangle_dt t, Point_dt p)
		{
			t.IsHalfplane = false;
			t.C = p;
			t.Circumcircle();
			Triangle_dt tca = t.CANext;

			if (p.PointLineTest(tca.A, tca.B) >= Point_dt.RIGHT)
			{
				Triangle_dt nT = new Triangle_dt(t.A, p)
				{
					ABNext = t
				};
				t.CANext = nT;
				nT.CANext = tca;
				tca.BCNext = nT;
				return nT;
			}

			return Extendcounterclock(tca, p);
		}

		private Triangle_dt ExtendClock(Triangle_dt t, Point_dt p)
		{
			t.IsHalfplane = false;
			t.C = p;
			t.Circumcircle();
			Triangle_dt tbc = t.BCNext;

			if (p.PointLineTest(tbc.A, tbc.B) >= Point_dt.RIGHT)
			{
				Triangle_dt nT = new Triangle_dt(p, t.B)
				{
					ABNext = t
				};
				t.BCNext = nT;
				nT.BCNext = tbc;
				tbc.CANext = nT;
				return nT;
			}

			return ExtendClock(tbc, p);
		}

		private void Flip(Triangle_dt t, int mc)
		{
			Triangle_dt u = t.ABNext;
			t.Mc = mc;

			if (u.IsHalfplane || (!u.CircumcircleContains(t.C))) return;

			Triangle_dt v;
			if (t.A.Equals(u.A))
			{
				v = new Triangle_dt(u.B, t.B, t.C)
				{
					ABNext = u.BCNext
				};
				t.ABNext = u.ABNext;
			}
			else if (t.A.Equals(u.B))
			{
				v = new Triangle_dt(u.C, t.B, t.C)
				{
					ABNext = u.CANext
				};
				t.ABNext = u.BCNext;
			}
			else if (t.A.Equals(u.C))
			{
				v = new Triangle_dt(u.A, t.B, t.C)
				{
					ABNext = u.ABNext
				};
				t.ABNext = u.CANext;
			}
			else
			{
				throw new Exception("Error in flip.");
			}

			v.Mc = mc;
			v.BCNext = t.BCNext;
			v.ABNext.SwitchNeighbors(u, v);
			v.BCNext.SwitchNeighbors(t, v);
			t.BCNext = v;
			v.CANext = t;
			t.B = v.A;
			t.ABNext.SwitchNeighbors(u, t);
			t.Circumcircle();
			currT = v;
			Flip(t, mc);
			Flip(v, mc);
		}

		/// <summary>
		/// finds the triangle the query point falls in, note if out-side of this
		/// triangulation a half plane triangle will be returned (see contains), the
		/// search has expected time of O(n^0.5), and it starts form a fixed triangle
		/// (me.startTriangle).
		/// </summary>
		/// <param name="p">Query point</param>
		/// <returns>The triangle that point <paramref name="p"/> is in.</returns>
		public Triangle_dt Find(Point_dt p)
		{
			// If triangulation has a spatial index try to use it as the starting
			//triangle
			Triangle_dt searchTriangle = startTriangle;
			if (gridIndex != null)
			{
				Triangle_dt indexTriangle = gridIndex.FindCellTriangleOf(p);
				if (indexTriangle != null)
				{
					searchTriangle = indexTriangle;
				}
			}

			// Search for the point's triangle starting from searchTriangle
			return Find(searchTriangle, p);
		}

		/// <summary>
		/// finds the triangle the query point falls in, note if out-side of this
		/// triangulation a half plane triangle will be returned (see contains). the
		/// search starts from the start triangle
		/// </summary>
		/// <param name="p">Query point</param>
		/// <param name="start">The triangle the search starts at.</param>
		/// <returns>The triangle that point <paramref name="p"/> is in.</returns>
		public Triangle_dt Find(Point_dt p, Triangle_dt start)
		{
			if (start == null)
			{
				start = startTriangle;
			}

			return Find(start, p);
		}

		private static Triangle_dt Find(Triangle_dt curr, Point_dt p)
		{
			if (p == null) return null;

			Triangle_dt next_t;
			if (curr.IsHalfplane)
			{
				next_t = FindNext2(p, curr);
				if (next_t?.IsHalfplane != false) return curr;
				curr = next_t;
			}

			while (true)
			{
				next_t = FindNext1(p, curr);
				if (next_t == null) return curr;
				if (next_t.IsHalfplane) return next_t;
				curr = next_t;
			}

			return null;
			// Never supposed to get here
		}

		/// <summary>
		/// assumes v is NOT an halfplane! returns the next triangle for find.
		/// </summary>
		private static Triangle_dt FindNext1(Point_dt p, Triangle_dt v)
		{
			if (p.PointLineTest(v.A, v.B) == Point_dt.RIGHT && (!v.ABNext.IsHalfplane))
			{
				return v.ABNext;
			}

			if (p.PointLineTest(v.B, v.C) == Point_dt.RIGHT && (!v.BCNext.IsHalfplane))
			{
				return v.BCNext;
			}

			if (p.PointLineTest(v.C, v.A) == Point_dt.RIGHT && (!v.CANext.IsHalfplane))
			{
				return v.CANext;
			}

			if (p.PointLineTest(v.A, v.B) == Point_dt.RIGHT)
			{
				return v.ABNext;
			}

			if (p.PointLineTest(v.B, v.C) == Point_dt.RIGHT)
			{
				return v.BCNext;
			}

			if (p.PointLineTest(v.C, v.A) == Point_dt.RIGHT)
			{
				return v.CANext;
			}

			return null;
		}

		/// <summary>
		/// assumes v is an halfplane! - returns another (none halfplane) triangle
		/// </summary>
		private static Triangle_dt FindNext2(Point_dt p, Triangle_dt v)
		{
			if (v.ABNext?.IsHalfplane == false)
			{
				return v.ABNext;
			}

			if (v.BCNext?.IsHalfplane == false)
			{
				return v.BCNext;
			}

			if (v.CANext?.IsHalfplane == false)
			{
				return v.CANext;
			}

			return null;
		}

		/// <summary>
		/// Search for p within current triangulation
		/// </summary>
		/// <param name="p">Query point</param>
		/// <returns>Return true if p is within current triangulation (in its 2D convex hull).</returns>
		public bool Contains(Point_dt p)
		{
			Triangle_dt tt = Find(p);
			return !tt.IsHalfplane;
		}

		/// <summary>
		/// Search for x/y point within current triangulation
		/// </summary>
		/// <param name="x">Query point x coordinate</param>
		/// <param name="y">Query point y coordinate</param>
		/// <returns>Return true if x/y is within current triangulation (in its 2D convex hull).</returns>
		public bool Contains(double x, double y)
		{
			Triangle_dt tt = Find(new Point_dt(x, y));
			return !tt.IsHalfplane;
		}

		/// <summary>
		/// Return point with x/y and updated Z value (z value is as given by the triangulation)
		/// </summary>
		/// <param name="p">Query point (x/y=</param>
		public Point_dt Z(Point_dt p)
		{
			Triangle_dt t = Find(p);
			return t.Z(p);
		}

		/// <summary>
		/// Return point with x/y and updated Z value (z value is as given by the triangulation)
		/// </summary>
		/// <param name="x">Query point x coordinate</param>
		/// <param name="y">Query point y coordinate</param>
		public Point_dt Z(double x, double y)
		{
			return this.Z(new Point_dt(x, y));
		}

		private void UpdateBoundingBox(Point_dt p)
		{
			double x = p.X;
			double y = p.Y;
			double z = p.Z;

			if (_bb_min == null)
			{
				_bb_min = new Point_dt(p);
				_bb_max = new Point_dt(p);
			}
			else
			{
				if (x < _bb_min.X)
				{
					_bb_min.X = x;
				}
				else if (x > _bb_max.X)
				{
					_bb_max.X = x;
				}
				if (y < _bb_min.Y)
				{
					_bb_min.Y = y;
				}
				else if (y > _bb_max.Y)
				{
					_bb_max.Y = y;
				}
				if (z < _bb_min.Z)
				{
					_bb_min.Z = z;
				}
				else if (z > _bb_max.Z)
				{
					_bb_max.Z = z;
				}
			}
		}

		/// <summary>
		/// Returns the bounding rectange between the minimum and maximum coordinates
		/// </summary>
		/// <returns>The bounding rectange between the minimum and maximum coordinates</returns>
		public BoundingBox BoundingBox
		{
			get { return new BoundingBox(_bb_min, _bb_max); }
		}

		/// <summary>
		/// Returns the min point of the bounding box of this triangulation
		/// </summary>
		/// <returns>The min point of the bounding box of this triangulation</returns>
		public Point_dt MinBoundingBox
		{
			get { return _bb_min; }
		}

		/// <summary>
		/// Returns the max point of the bounding box of this triangulation
		/// </summary>
		/// <returns>The max point of the bounding box of this triangulation</returns>
		public Point_dt MaxBoundingBox
		{
			get { return _bb_max; }
		}

		/// <summary>
		/// computes the current set of all triangles and return an iterator to them.
		/// </summary>
		/// <returns>An iterator to the current set of all triangles</returns>
		public IEnumerator<Triangle_dt> TrianglesIterator()
		{
			if (this.Size() <= 2)
			{
				_triangles = new List<Triangle_dt>();
			}
			InitTriangles();
			return _triangles.GetEnumerator();
		}

		/// <summary>
		/// Returns an iterator to the set of points composing this triangulation
		/// </summary>
		/// <returns>An iterator to the set of points composing this triangulation</returns>
		public IEnumerator<Point_dt> VerticesIterator()
		{
			return _vertices.GetEnumerator();
		}

		private void InitTriangles()
		{
			if (_modCount == _modCount2) return;

			if (this.Size() > 2)
			{
				_modCount2 = _modCount;
				List<Triangle_dt> front = new List<Triangle_dt>();
				_triangles = new List<Triangle_dt>();
				front.Add(this.startTriangle);
				while (front.Count > 0)
				{
					Triangle_dt t = front[0];
					front.RemoveAt(0);
					if (!t.Mark)
					{
						t.Mark = true;
						_triangles.Add(t);

						if (t.ABNext?.Mark == false)
						{
							front.Add(t.ABNext);
						}

						if (t.BCNext?.Mark == false)
						{
							front.Add(t.BCNext);
						}

						if (t.CANext?.Mark == false)
						{
							front.Add(t.BCNext);
						}
					}
				}

				foreach (Triangle_dt aTriangle in _triangles)
				{
					aTriangle.Mark = false;
				}
			}
		}

		/// <summary>
		/// Index the triangulation using a grid index
		/// </summary>
		/// <param name="xCellCount">number of grid cells in a row</param>
		/// <param name="yCellCount">number of grid cells in a column</param>
		public void IndexData(int xCellCount, int yCellCount)
		{
			gridIndex = new GridIndex(this, xCellCount, yCellCount);
		}

		/// <summary>
		/// Remove any existing spatial indexing
		/// </summary>
		public void RemoveIndex()
		{
			gridIndex = null;
		}
	}
}
