namespace Nzy3d.Maths.Algorithms.Interpolation.Bernstein
{
	/// <summary>
	/// <para> This is a generic 3D B-Spline class for curves of arbitrary length, control
	/// handles and patches are created and joined automatically as described here:
	/// http://www.ibiblio.org/e-notes/Splines/Bint.htm">ibiblio.org/e-notes/Splines/Bint.htm
	/// </para>
	/// <para>
	/// Thanks to a bug report by Aaron Meyers (http://universaloscillation.com) the
	/// {@linkplain #computeVertices(int)} method has a slightly changed behaviour
	/// from version 0014 onwards. In earlier versions erroneous duplicate points
	/// would be added near each given control point, which lead to various weird
	/// results.
	/// </para>
	/// <para>
	/// The new behaviour of the curve interpolation/computation is described in the
	/// docs for the {@linkplain #computeVertices(int)} method below.
	/// </para>
	/// </summary>
	/// <remarks>
	/// Version 0014 Added user adjustable curve tightness control
	/// Version 0015 Added JAXB annotations and List support for dynamic building of spline
	/// </remarks>
	public class Spline3D
	{
		public const double DEFAULT_TIGHTNESS = 0.25;
		public List<Coord3d> MPointList;
		public List<Coord3d> Vertices;
		public BernsteinPolynomial Bernstein;
		public Coord3d[] Delta;
		public Coord3d[] CoeffA;
		public double[] Bi;

		internal Coord3d[] points;
		internal double mtightness;
		internal double invTightness;
		internal int numP;

		/// <summary>
		/// Constructor (default tightness, no control points and no predefined bernstein polynomial)
		/// </summary>
		public Spline3D() : this(new List<Coord3d>(), null, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">List of control point vectors</param>
		public Spline3D(List<Coord3d> rawPoints) : this(rawPoints, null, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">List of control point vectors</param>
		/// <param name="b">Predefined Bernstein polynomial (good for reusing)</param>
		/// <param name="tightness">Default curve tightness used for the interpolated vertices</param>
		public Spline3D(List<Coord3d> rawPoints, BernsteinPolynomial b, double tightness)
		{
			this.Tightness = tightness;
			MPointList = new List<Coord3d>();
			MPointList.AddRange(rawPoints);
			Bernstein = b;
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">Array of control point vectors</param>
		public Spline3D(Coord3d[] rawPoints) : this(rawPoints, null, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">Array of control point vectors</param>
		/// <param name="b">Predefined Bernstein polynomial (good for reusing)</param>
		/// <param name="tightness">Default curve tightness used for the interpolated vertices</param>
		public Spline3D(Coord3d[] rawPoints, BernsteinPolynomial b, double tightness) : this(new List<Coord3d>(rawPoints), b, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		/// Property to access (read/write) curve tightness used for the interpolated vertices
		/// of future curve interpolation calls. Default value is
		/// 0.25. A value of 0.0 equals linear interpolation between the given curve
		/// input points. Curve behaviour for values outside the 0.0 .. 0.5 interval
		/// is unspecified and becomes increasingly less intuitive. Negative values
		/// are possible too and create interesting results (in some cases).
		/// </summary>
		public double Tightness
		{
			get { return mtightness; }
			set
			{
				mtightness = value;
				invTightness = 1 / mtightness;
			}
		}

		public List<Coord3d> PointList
		{
			get { return MPointList; }
			set
			{
				MPointList.Clear();
				MPointList.AddRange(value);
			}
		}

		/// <summary>
		/// Returns the number of key points.
		/// </summary>
		public int NumPoints
		{
			get { return numP; }
		}

		/// <summary>
		/// Adds the given point to the list of control points.
		/// </summary>
		/// <returns>Itself</returns>
		public Spline3D Add(Coord3d p)
		{
			MPointList.Add(p);
			return this;
		}

		/// <summary>
		/// Adds the given point to the list of control points.
		/// </summary>
		/// <returns>Itself</returns>
		public Spline3D Add(double x, double y, double z)
		{
			return Add(new Coord3d(x, y, z));
		}

		public void UpdateCoefficients()
		{
			numP = PointList.Count;
			if (points == null || points.Length - 1 != numP)
			{
				CoeffA = new Coord3d[numP];
				Delta = new Coord3d[numP];
				Bi = new double[numP];
				for (int i = 0; i <= numP - 1; i++)
				{
					CoeffA[i] = new Coord3d();
					Delta[i] = new Coord3d();
				}
			}
			points = PointList.ToArray();
		}

		public List<Coord3d> ComputeVertices(int resolution)
		{
			UpdateCoefficients();
            resolution++;

			if (Bernstein == null || Bernstein.resolution != resolution)
			{
				Bernstein = new BernsteinPolynomial(resolution);
			}

			if (Vertices == null)
			{
				Vertices = new List<Coord3d>();
			}
			else
			{
				Vertices.Clear();
			}

			FindCPoints();
            resolution--;

			for (int i = 0; i <= numP - 2; i++)
			{
				Coord3d p = points[i];
				Coord3d q = points[i + 1];
                Coord3d deltaP = Delta[i].Add(p);
                Coord3d deltaQ = q.Substract(Delta[i + 1]);
                for (int k = 0; k <= resolution - 1; k++)
				{
					double x = p.X * Bernstein.b0[k] + deltaP.X * Bernstein.b1[k] + deltaQ.X * Bernstein.b2[k] + q.X * Bernstein.b3[k];
					double y = p.Y * Bernstein.b0[k] + deltaP.Y * Bernstein.b1[k] + deltaQ.Y * Bernstein.b2[k] + q.Y * Bernstein.b3[k];
					double z = p.Z * Bernstein.b0[k] + deltaP.Z * Bernstein.b1[k] + deltaQ.Z * Bernstein.b2[k] + q.Z * Bernstein.b3[k];
					Vertices.Add(new Coord3d(x, y, z));
				}
			}
			Vertices.Add(points[points.Length - 1]);
			return Vertices;
		}

		internal void FindCPoints()
		{
			Bi[1] = -Tightness;
			CoeffA[1].SetValues((points[2].X - points[0].X - Delta[0].X) * Tightness, (points[2].Y - points[0].Y - Delta[0].Y) * Tightness, (points[2].Z - points[0].Z - Delta[0].Z) * Tightness);
			// correction: original java code : for (int i = 2; i < numP - 1; i++)

			for (int i = 2; i <= numP - 2; i++)
			{
				Bi[i] = -1 / (invTightness + Bi[i - 1]);
				CoeffA[i].SetValues(-(points[i + 1].X - points[i - 1].X - CoeffA[i - 1].X) * Bi[i], -(points[i + 1].Y - points[i - 1].Y - CoeffA[i - 1].Y) * Bi[i], -(points[i + 1].Z - points[i - 1].Z - CoeffA[i - 1].Z) * Bi[i]);
			}

			for (int i = numP - 2; i >= 1; i += -1)
			{
				Delta[i].SetValues(CoeffA[i].X + Delta[i + 1].X * Bi[i], CoeffA[i].Y + Delta[i + 1].Y * Bi[i], CoeffA[i].Z + Delta[i + 1].Z * Bi[i]);
			}
		}

		public List<Coord3d> GetDecimatedVertices(double dstep)
		{
			var steps = new List<Coord3d>();
			int num = Vertices.Count;
			int i = 0;
			Coord3d b = Vertices[0];
			Coord3d curr = b.Clone();
			while (i < num - 1)
			{
				Coord3d a = b;
				b = Vertices[i + 1];
				Coord3d dir = b.Substract(a);
				double segLen = 1 / dir.MagSquared();
				Coord3d stepDir = dir.NormalizeTo(dstep);
				curr = a.InterpolateTo(b, curr.Substract(a).Dot(dir) * segLen);
				while (curr.Substract(a).Dot(dir) / segLen <= 1)
				{
					steps.Add(curr.Clone());
					curr.AddSelf(stepDir);
				}
				i++;
			}
			return steps;
		}
	}
}
