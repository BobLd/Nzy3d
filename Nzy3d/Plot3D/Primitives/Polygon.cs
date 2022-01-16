using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Maths;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives
{
	public class Polygon : AbstractWireframeable, ISingleColorable, IMultiColorable
	{
		public enum PolygonMode
		{
			FRONT,
			BACK,
			FRONT_AND_BACK
		}

		internal PolygonMode _polygonMode;
		internal bool _polygonOffsetFillEnable;
		internal ColorMapper _mapper;
		internal List<Point> _points;
		internal Color _color;

		internal Coord3d _center;
		public Polygon() : base()
		{
			_points = new List<Point>();
			_bbox = new BoundingBox3d();
			_center = new Coord3d();
			_polygonOffsetFillEnable = true;
			_polygonMode = PolygonMode.FRONT_AND_BACK;
		}

		public override void Draw(Rendering.View.Camera cam)
		{
			_transform?.Execute();

			if (_facestatus)
			{
				ApplyPolygonModeFill();
				if (_wfstatus && _polygonOffsetFillEnable)
				{
					EnablePolygonOffsetFill();
				}

				GL.Begin(PrimitiveType.Polygon);
				foreach (Point p in _points)
				{
					if (_mapper != null)
					{
						p.Color = _mapper.Color(p.XYZ);
					}
					GL.Color4(p.Color.R, p.Color.G, p.Color.B, p.Color.A);
					GL.Vertex3(p.XYZ.X, p.XYZ.Y, p.XYZ.Z);
				}

				GL.End();
				if (_wfstatus && _polygonOffsetFillEnable)
				{
					DisablePolygonOffsetFill();
				}
			}

			if (_wfstatus)
			{
				ApplyPolygonModeLine();
				if (_polygonOffsetFillEnable)
				{
					EnablePolygonOffsetFill();
				}

				GL.Color4(_wfcolor.R, _wfcolor.G, _wfcolor.B, _wfcolor.A);
				GL.LineWidth(_wfwidth);
				GL.Begin(PrimitiveType.Polygon);

				foreach (Point p in _points)
				{
					GL.Vertex3(p.XYZ.X, p.XYZ.Y, p.XYZ.Z);
				}

				GL.End();

				if (_polygonOffsetFillEnable)
				{
					DisablePolygonOffsetFill();
				}
			}
		}

		internal void ApplyPolygonModeLine()
		{
			switch (_polygonMode)
			{
				case PolygonMode.FRONT:
					GL.PolygonMode(MaterialFace.Front, OpenTK.Graphics.OpenGL.PolygonMode.Line);
					break;
				case PolygonMode.BACK:
					GL.PolygonMode(MaterialFace.Back, OpenTK.Graphics.OpenGL.PolygonMode.Line);
					break;
				case PolygonMode.FRONT_AND_BACK:
					GL.PolygonMode(MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
					break;
				default:
					throw new Exception("Unsupported Polygon Mode :" + _polygonMode);
			}
		}

		internal void ApplyPolygonModeFill()
		{
			switch (_polygonMode)
			{
				case PolygonMode.FRONT:
					GL.PolygonMode(MaterialFace.Front, OpenTK.Graphics.OpenGL.PolygonMode.Fill);
					break;
				case PolygonMode.BACK:
					GL.PolygonMode(MaterialFace.Back, OpenTK.Graphics.OpenGL.PolygonMode.Fill);
					break;
				case PolygonMode.FRONT_AND_BACK:
					GL.PolygonMode(MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Fill);
					break;
				default:
					throw new Exception("Unsupported Polygon Mode :" + _polygonMode);
			}
		}

		internal static void EnablePolygonOffsetFill()
		{
			GL.Enable(EnableCap.PolygonOffsetFill);
			GL.PolygonOffset(1, 1);
		}

		internal static void DisablePolygonOffsetFill()
		{
			GL.Disable(EnableCap.PolygonOffsetFill);
		}

		/// <summary>
		/// Add a point to the polygon
		/// </summary>
		/// <param name="point">Point to add</param>
		public void Add(Point point)
		{
			_points.Add(point);
			_bbox.Add(point);
			// Recompute Center
			_center = new Coord3d();
			foreach (Point p in _points)
			{
				_center.AddSelf(p.XYZ);
			}
			_center.DivideSelf(_points.Count);
		}

		/// <summary>
		/// Return the barycentre of this object, which is
		/// computed as the center of its bounding box. If the bounding
		/// box is not available, the returned value is <see cref=" Coord3d.INVALID"/>
		/// </summary>
		public override Coord3d Barycentre
		{
			get { return _center; }
		}

		public Point GetPoint(int p)
		{
			return _points[p];
		}

		public IEnumerable<Point> GetPoints
		{
			get { return _points; }
		}

		public int Size
		{
			get { return _points.Count; }
		}

		public override double GetDistance(Rendering.View.Camera camera)
		{
			return Barycentre.Distance(camera.Eye);
		}

		public override double GetShortestDistance(Rendering.View.Camera camera)
		{
			double min = double.MaxValue;
			double dist;
			foreach (Point p in _points)
			{
				dist = p.GetDistance(camera);
				if (dist < min)
				{
					min = dist;
				}
			}

			dist = Barycentre.Distance(camera.Eye);
			if (dist < min)
			{
				min = dist;
			}
			return min;
		}

		public override double GetLongestDistance(Rendering.View.Camera camera)
		{
			double max = 0;
			foreach (Point p in _points)
			{
				double dist = p.GetDistance(camera);
				if (dist > max)
				{
					max = dist;
				}
			}
			return max;
		}

		public PolygonMode Mode
		{
			get { return _polygonMode; }
			set { _polygonMode = value; }
		}

		/// <summary>
		/// Get/Set offset fill enable mode, which let a polygon with a wireframe render cleanly without weird
		/// depth incertainty between face and border.
		/// Default value is true.
		/// </summary>
		public bool PolygonOffsetFillEnable
		{
			get { return _polygonOffsetFillEnable; }
			set { _polygonOffsetFillEnable = value; }
		}

		/// <summary>
		/// A utility to change polygon offset fill status of a <see cref="AbstractComposite"/> containing <see cref="Polygon"/>s.
		/// </summary>
		/// <param name="composite"></param>
		/// <param name="polygonOffsetFillEnable">status to apply to all polygons contained in composite (and recursively to child composites)</param>
		public static void SetPolygonOffsetFillEnable(AbstractComposite composite, bool polygonOffsetFillEnable)
		{
			foreach (AbstractDrawable d in composite.Drawables)
			{
				if (d is Polygon dP)
				{
					dP.PolygonOffsetFillEnable = polygonOffsetFillEnable;
				}
				else if (d is AbstractComposite dC)
				{
					SetPolygonOffsetFillEnable(dC, polygonOffsetFillEnable);
				}
			}
		}

		public ColorMapper ColorMapper
		{
			get { return _mapper; }
			set
			{
				_mapper = value;
				FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
			}
		}

		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				foreach (Point p in _points)
				{
					p.Color = value;
				}
				FireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
			}
		}

		public override string ToString(int depth)
		{
			return $"{Utils.Blanks(depth)}(Polygon) #points={this.Size}";
		}
	}
}
