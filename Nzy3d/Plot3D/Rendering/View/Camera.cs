using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View.Modes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nzy3d.Plot3D.Rendering.View
{
	/// <summary>
	/// <para>
	/// A <see cref="Camera"/> provides an easy control on the view and target points
	/// in a cartesian coordinate system.
	/// </para>
	/// <para>
	/// The <see cref="Camera"/> handles the following services:
	/// <ul>
	/// <li>allows setting perspective/orthogonal rendering mode through <see cref="CameraMode"/>.</li>
	/// <li>selects the appropriate clipping planes according to a given target box.</li>
	/// <li>ensure the modelview matrix is always available for GL2 calls related to anything else than projection.</li>
	/// <li>methods to convert screen coordinates into 3d coordinates and vice-versa</li>
	/// </ul>
	/// </para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public class Camera : AbstractViewport
	{
		internal Coord3d _eye;
		internal Coord3d _target;
		internal Coord3d _up;
		internal float _radius;
		internal float _near;
		internal float _far;

		internal bool _failOnException;

		static internal Coord3d DEFAULT_VIEW = new Coord3d(Math.PI / 3, Math.PI / 5, 500);

		public Camera(Coord3d target) : base()
		{
		}

		/// <summary>
		/// Eye position
		/// </summary>
		public Coord3d Eye
		{
			get { return _eye; }
			set { _eye = value; }
		}

		/// <summary>
		/// Target point of the camera
		/// </summary>
		public Coord3d Target
		{
			get { return _target; }
			set { _target = value; }
		}

		/// <summary>
		/// Top of the camera
		/// </summary>
		public Coord3d Up
		{
			get { return _up; }
			set { _up = value; }
		}

		public bool IsTiltUp
		{
			get { return Eye.Z < Target.Z; }
		}

		/// <summary>
		/// Get/Set the radius of the sphere that will be contained into the rendered view.
		/// When the radius is set, as a side effect, the "far" clipping plane is modified according to the eye-target distance, as well as
		/// the position of the "near" clipping plane.
		/// </summary>
		public float RenderingSphereRadius
		{
			get { return _radius; }
			set
			{
				_radius = value;
				_near = (float)_eye.Distance(_target) - _radius * 2;
				_far = (float)_eye.Distance(_target) + _radius * 2;
			}
		}

		public void SetRenderingDepth(float near, float far)
		{
			_near = near;
			_far = far;
		}

		/// <summary>
		/// Return the position of the "near" clipping plane
		/// </summary>
		public float Near
		{
			get { return _near; }
		}

		/// <summary>
		/// Return the position of the "far" clipping plane
		/// </summary>
		public float Far
		{
			get { return _far; }
		}

		/// <summary>
		/// Wether to raise or not an exception when an error occurs.
		/// </summary>
		public bool FailOnException
		{
			get { return _failOnException; }
			set { _failOnException = value; }
		}

		/// <summary>
		/// Return true if the given point is on the left of the vector eye->target.
		/// </summary>
		public bool Side(Coord3d point)
		{
			return 0 < ((point.X - Target.X) * (Eye.Y - Target.Y) - (point.Y - Target.Y) * (Eye.X - Target.X));
		}

		/// <summary>
		/// Transform a 2d screen coordinate into a 3d coordinate.
		/// The z component of the screen coordinate indicates a depth value between the
		/// near and far clipping plane of the {@link Camera}.
		/// </summary>
		/// <param name="screen">2D screen coordinate</param>
		/// <returns>3D model coordinate</returns>
		public Coord3d ScreenToModel(Coord3d screen)
		{
			Vector4d worldcoord = default;
			if (!Glut.Glut.UnProject(new Vector4d(screen.X, screen.Y, screen.Z, 0), GetModelViewAsMatrix4d(), GetProjectionAsMatrix4d(), GetViewPortAsDouble(), ref worldcoord))
			{
				FailedProjection("Could not retrieve screen coordinates in model.");
			}
			return new Coord3d(worldcoord.X, worldcoord.Y, worldcoord.Z);
		}

		/// <summary>
		/// Transform a 3d point coordinate into its screen position.
		/// </summary>
		/// <param name="point">3D model coordinate</param>
		/// <returns>2D screen coordinate</returns>
		public Coord3d ModelToScreen(Coord3d point)
		{
			Vector4d screencoord = default;
			if (!Glut.Glut.Project(new Vector4d(point.X, point.Y, point.Z, 0), GetModelViewAsMatrix4d(), GetProjectionAsMatrix4d(), GetViewPortAsDouble(), ref screencoord))
			{
				FailedProjection("Could not retrieve model coordinates in screen " + point.ToString() + ".");
			}
			return new Coord3d(screencoord.X, screencoord.Y, screencoord.Z);
		}

		/// <summary>
		/// Transform a set of 3d points coordinates into their screen positions.
		/// </summary>
		/// <param name="points">3D model coordinates</param>
		/// <returns>2D screen coordinates</returns>
		public Coord3d[] ModelToScreen(Coord3d[] points)
		{
			var screenCoords = new Coord3d[points.Length];
			Matrix4d modelMatrix = GetModelViewAsMatrix4d();
			Matrix4d projectionMatrix = GetProjectionAsMatrix4d();
			double[] viewport = GetViewPortAsDouble();
			Vector4d screenCoord = default;
			for (int i = 0; i <= points.Length - 1; i++)
			{
				if (!Glut.Glut.Project(new Vector4d(points[i].X, points[i].Y, points[i].Z, 0), modelMatrix, projectionMatrix, viewport, ref screenCoord))
				{
					FailedProjection("Could not retrieve model coordinates in screen for point #" + i + " " + points[i].ToString() + ".");
				}
				screenCoords[i] = new Coord3d(screenCoord.X, screenCoord.Y, screenCoord.Z);
			}
			return screenCoords;
		}

		/// <summary>
		/// Transform a set of 3d points coordinates into their screen positions.
		/// </summary>
		/// <param name="points">3D model coordinates</param>
		/// <returns>2D screen coordinates</returns>
		public Coord3d[,] ModelToScreen(Coord3d[,] points)
		{
			var screenCoords = new Coord3d[points.GetLength(0), points.GetLength(1)];
			Matrix4d modelMatrix = GetModelViewAsMatrix4d();
			Matrix4d projectionMatrix = GetProjectionAsMatrix4d();
			double[] viewport = GetViewPortAsDouble();
			Vector4d screenCoord = default;
			for (int i = 0; i <= points.GetLength(0) - 1; i++)
			{
				for (int j = 0; j <= points.GetLength(1) - 1; j++)
				{
					if (!Glut.Glut.Project(new Vector4d(points[i, j].X, points[i, j].Y, points[i, j].Z, 0), modelMatrix, projectionMatrix, viewport, ref screenCoord))
					{
						FailedProjection("Could not retrieve model coordinates in screen for point #" + i + " " + points[i, j].ToString() + ".");
					}
					screenCoords[i, j] = new Coord3d(screenCoord.X, screenCoord.Y, screenCoord.Z);
				}
			}
			return screenCoords;
		}

		/// <summary>
		/// Transform a set of 3d points coordinates into their screen positions.
		/// </summary>
		/// <param name="points">3D model coordinates</param>
		/// <returns>2D screen coordinates</returns>
		public List<Coord3d> ModelToScreen(List<Coord3d> points)
		{
			var screenCoords = new List<Coord3d>();
			Matrix4d modelMatrix = GetModelViewAsMatrix4d();
			Matrix4d projectionMatrix = GetProjectionAsMatrix4d();
			double[] viewport = GetViewPortAsDouble();
			Vector4d screenCoord = default;
			foreach (Coord3d aPoint in points)
			{
				if (!Glut.Glut.Project(new Vector4d(aPoint.X, aPoint.Y, aPoint.Z, 0), modelMatrix, projectionMatrix, viewport, ref screenCoord))
				{
					FailedProjection("Could not retrieve model coordinates in screen for point " + aPoint.ToString() + ".");
				}
				screenCoords.Add(new Coord3d(screenCoord.X, screenCoord.Y, screenCoord.Z));
			}
			return screenCoords;
		}

		/// <summary>
		/// Transform a polygon array of model coordinates into a polygon array of screen coordinates.
		/// </summary>
		/// <param name="polygon">Polygon array of model coordinates</param>
		/// <returns>Polygon array of screen coordinates</returns>
		public PolygonArray ModelToScreen(PolygonArray polygon)
		{
			//List<Coord3d> screenCoords = new List<Coord3d>();
			int len = polygon.Length;
			double[] x = new double[len];
			double[] y = new double[len];
			double[] z = new double[len];
			Matrix4d modelMatrix = GetModelViewAsMatrix4d();
			Matrix4d projectionMatrix = GetProjectionAsMatrix4d();
			double[] viewport = GetViewPortAsDouble();
			Vector4d screenCoord = default;
			for (int i = 0; i <= len - 1; i++)
			{
				if (!Glut.Glut.Project(new Vector4d(polygon.X[i], polygon.Y[i], polygon.Z[i], 0), modelMatrix, projectionMatrix, viewport, ref screenCoord))
				{
					FailedProjection("Could not retrieve model coordinates in screen for point #" + i + ".");
				}

				x[i] = screenCoord.X;
				y[i] = screenCoord.Y;
				z[i] = screenCoord.Z;
			}
			return new PolygonArray(x, y, z);
		}

		/// <summary>
		/// Transform a grid of model coordinates into a grid of screen coordinates.
		/// </summary>
		/// <param name="grid">Grid of model coordinates</param>
		/// <returns>Grid of screen coordinates</returns>
		public Grid ModelToScreen(Grid grid)
		{
			//List<Coord3d> screenCoords = new List<Coord3d>();
			int xlen = grid.X.Length;
			int ylen = grid.Y.Length;
			double[] x = new double[xlen];
			double[] y = new double[ylen];
			double[,] z = new double[xlen, ylen];
			Matrix4d modelMatrix = GetModelViewAsMatrix4d();
			Matrix4d projectionMatrix = GetProjectionAsMatrix4d();
			double[] viewport = GetViewPortAsDouble();
			Vector4d screenCoord = default;
			for (int i = 0; i <= xlen - 1; i++)
			{
				for (int j = 0; j <= ylen - 1; j++)
				{
					if (!Glut.Glut.Project(new Vector4d(grid.X[i], grid.Y[j], grid.Z[i, j], 0), modelMatrix, projectionMatrix, viewport, ref screenCoord))
					{
						FailedProjection("Could not retrieve model coordinates in screen for point #" + i + ".");
					}
					x[i] = screenCoord.X;
					y[j] = screenCoord.Y;
					z[i, j] = screenCoord.Z;
				}
			}
			return new Grid(x, y, z);
		}

		/// <summary>
		/// Transform a two dimensionnal array of polygon arrays of model coordinates into a two dimensionnal array of polygon arrays of screen coordinates.
		/// </summary>
		/// <param name="polygons">Two dimensionnal array of polygon arrays of model coordinates</param>
		/// <returns>Two dimensionnal array of polygon array of screen coordinates</returns>
		public PolygonArray[,] ModelToScreen(PolygonArray[,] polygons)
		{
			PolygonArray[,] screenCoords = new PolygonArray[polygons.GetLength(0), polygons.GetLength(1)];
			int len = polygons.Length;
			Matrix4d modelMatrix = GetModelViewAsMatrix4d();
			Matrix4d projectionMatrix = GetProjectionAsMatrix4d();
			double[] viewport = GetViewPortAsDouble();
			Vector4d screenCoord = default;
			for (int i = 0; i <= polygons.GetLength(0) - 1; i++)
			{
				for (int j = 0; j <= polygons.GetLength(1) - 1; j++)
				{
					double[] x = new double[len];
					double[] y = new double[len];
					double[] z = new double[len];
					PolygonArray polygon = polygons[i, j];
					for (int k = 0; k <= polygon.Length - 1; k++)
					{
						if (!Glut.Glut.Project(new Vector4d(polygon.X[k], polygon.Y[k], polygon.Z[k], 0), modelMatrix, projectionMatrix, viewport, ref screenCoord))
						{
							FailedProjection("Could not retrieve model coordinates in screen for point #" + k + " of polygon (" + i + "," + j + ").");
						}
						x[k] = screenCoord.X;
						y[k] = screenCoord.Y;
						z[k] = screenCoord.Z;
					}
					screenCoords[i, j] = new PolygonArray(x, y, z);
				}
			}
			return screenCoords;
		}

		internal int[] GetViewPortAsInt()
		{
			int[] viewport = new int[4];
			GL.GetInteger(GetPName.Viewport, viewport);
			return viewport;
		}

		internal double[] GetViewPortAsDouble()
		{
			double[] viewport = new double[4];
			GL.GetDouble(GetPName.Viewport, viewport);
			return viewport;
		}

		internal float[] GetModelViewAsFloat()
		{
			float[] modelview = new float[16];
			GL.GetFloat(GetPName.ModelviewMatrix, modelview);
			return modelview;
		}

		internal Matrix4 GetModelViewAsMatrix4()
		{
			GL.GetFloat(GetPName.ModelviewMatrix, out Matrix4 modelview);
			return modelview;
		}

		internal Matrix4d GetModelViewAsMatrix4d()
		{
			GL.GetDouble(GetPName.ModelviewMatrix, out Matrix4d modelview);
			return modelview;
		}

		internal float[] GetProjectionAsFloat()
		{
			float[] projection = new float[16];
			GL.GetFloat(GetPName.ProjectionMatrix, projection);
			return projection;
		}

		internal Matrix4 GetProjectionAsMatrix4()
		{
			GL.GetFloat(GetPName.ProjectionMatrix, out Matrix4 projection);
			return projection;
		}

		internal Matrix4d GetProjectionAsMatrix4d()
		{
			GL.GetDouble(GetPName.ProjectionMatrix, out Matrix4d projection);
			return projection;
		}

		internal void FailedProjection(string message)
		{
			if (FailOnException)
			{
				throw new Exception(message);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine(message);
			}
		}

		/// <summary>
		/// Sets the projection, and the mapping of 3d environement to 2d screen.
		/// The projection must be either Camera.PERSPECTIVE or Camera.ORTHOGONAL.
		/// <br/>
		/// Finally calls the GL2 function LookAt, according to the stored
		/// eye, target, up and scale values.
		/// <br/>
		/// Note that the Camera set by itselft the MatrixMode to model view
		/// at the end of a shoot().
		/// </summary>
		/// <param name="projection">Project mode</param>
		public void Shoot(CameraMode projection)
		{
			Shoot(projection, false);
		}

		public void Shoot(CameraMode projection, bool doPushMatrixBeforeShooting)
		{
			GL.MatrixMode(MatrixMode.Projection);
			if (doPushMatrixBeforeShooting)
			{
				GL.PushMatrix();
			}
			GL.LoadIdentity();
			DoShoot(projection);
		}

		public void DoShoot(CameraMode projection)
		{
			// Set Viewport
			ApplyViewPort();

			// Set perspective
			if (projection == CameraMode.PERSPECTIVE)
			{
				Glut.Glut.Perspective(ComputeFieldOfView(RenderingSphereRadius * 2, Eye.Distance(Target)), StretchToFill ? _screenWidth / _screenHeight : 1, Near, Far);
			}
			else if (projection == CameraMode.ORTHOGONAL)
			{
				GL.Ortho(-RenderingSphereRadius, +RenderingSphereRadius, -RenderingSphereRadius, +RenderingSphereRadius, Near, Far);
			}
			else
			{
				throw new Exception("Camera.shoot() : unsupported projection mode '" + projection + "'");
			}

			// Set camera position
			Glut.Glut.LookAt(Eye.X, Eye.Y, Eye.Z, Target.X, Target.Y, Target.Z, Up.X, Up.Y, Up.Z);
		}

		internal static double ComputeFieldOfView(double size, double distance)
		{
			double radtheta = 2 * Math.Atan2(size / 2, distance);
			return (double)(180 * radtheta / Math.PI); // degtheta
		}

		public override string ToString()
		{
			string output = "(Camera)";
			output += " lookFrom  = {" + Eye.X + ", " + Eye.Y + ", " + Eye.Z + "}";
			output += " lookTo    = {" + Target.X + ", " + Target.Y + ", " + Target.Z + "}";
			output += " topToward = {" + Up.X + ", " + Up.Y + ", " + Up.Z + "}";
			return output;
		}
	}
}
