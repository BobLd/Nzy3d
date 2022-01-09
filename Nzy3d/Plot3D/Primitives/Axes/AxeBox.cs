using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using Nzy3d.Plot3D.Rendering.View;
using Nzy3d.Plot3D.Rendering.View.Modes;
using Nzy3d.Plot3D.Text;
using Nzy3d.Plot3D.Text.Align;
using Nzy3d.Plot3D.Text.Renderers;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives.Axes
{
	/// <summary>
	/// The AxeBox displays a box with front face invisible and ticks labels.
	/// @author Martin Pernollet
	/// </summary>
	public class AxeBox : IAxe
	{
		public enum AxeDirection
		{
			AxeX,
			AxeY,
			AxeZ
		}

		static internal int PRECISION = 6;
		internal View _view;
		// use this text renderer to get occupied volume by text
		internal ITextRenderer _txt = new TextBitmapRenderer();
		//Friend TextOverlay  txtRenderer;	' keep it null in order to not use it
		//'Friend TextBillboard txt = new TextBillboard();	
		internal IAxeLayout _layout;
		internal BoundingBox3d _boxBounds;
		internal BoundingBox3d _wholeBounds;
		internal Coord3d _center;
		internal Coord3d _scale;
		internal float _xrange;
		internal float _yrange;
		internal float _zrange;
		internal float[,] _quadx;
		internal float[,] _quady;
		internal float[,] _quadz;
		internal float[] _normx;
		internal float[] _normy;
		internal float[] _normz;
		internal float[,] _axeXx;
		internal float[,] _axeXy;
		internal float[,] _axeXz;
		internal float[,] _axeYx;
		internal float[,] _axeYy;
		internal float[,] _axeYz;
		internal float[,] _axeZx;
		internal float[,] _axeZy;
		internal float[,] _axeZz;
		internal int[,] _axeXquads;
		internal int[,] _axeYquads;
		internal int[,] _axeZquads;

		internal bool[] _quadIsHidden;
		public AxeBox(BoundingBox3d bbox) : this(bbox, new AxeBoxLayout())
		{
		}

		public AxeBox(BoundingBox3d bbox, IAxeLayout layout)
		{
			_layout = layout;
			if (bbox.IsValid())
			{
				SetAxe(bbox);
			}
			else
			{
				SetAxe(new BoundingBox3d(-1, 1, -1, 1, -1, 1));
			}
			_wholeBounds = new BoundingBox3d();
			Init();
		}

		public void Init()
		{
			this.Scale = new Coord3d(1, 1, 1);
		}

		public void Dispose()
		{
			//If Not IsNothing(_txtRenderer) Then
			//  _txtRenderer.Dispose()
			//End If
		}

		public ITextRenderer TextRenderer
		{
			get { return _txt; }
			set { _txt = value; }
		}

		public View View
		{
			get { return _view; }
			set { _view = value; }
		}

		public IAxeLayout Layout
		{
			get { return _layout; }
		}

		public void Draw(Camera camera)
		{
			// Set scaling
			GL.LoadIdentity();
			GL.Scale(_scale.X, _scale.Y, _scale.Z);

			// Set culling
			GL.Enable(EnableCap.CullFace);
			GL.FrontFace(FrontFaceDirection.Ccw);
			GL.CullFace(CullFaceMode.Front);

			// Draw cube in feedback buffer for computing hidden quads
			_quadIsHidden = GetHiddenQuads(camera);

			// Plain part of quad making the surrounding box
			if (_layout.FaceDisplayed)
			{
				Color quadcolor = _layout.QuadColor;
				GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);
				GL.Color4(quadcolor.R, quadcolor.G, quadcolor.B, quadcolor.A);
				GL.LineWidth(1);
				GL.Enable(EnableCap.PolygonOffsetFill);
				GL.PolygonOffset(1, 1);

				// handle stippling
				DrawCube(RenderingMode.Render);
				GL.Disable(EnableCap.PolygonOffsetFill);
			}

			// Edge part of quads making the surrounding box
			Color gridcolor = _layout.GridColor;
			GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
			GL.Color4(gridcolor.R, gridcolor.G, gridcolor.B, gridcolor.A);
			GL.LineWidth(1);
			DrawCube(RenderingMode.Render);

			// Draw grids on non hidden quads
			GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
			GL.Color4(gridcolor.R, gridcolor.G, gridcolor.B, gridcolor.A);
			GL.LineWidth(1);
			GL.LineStipple(1, 0xaaaa);
			GL.Enable(EnableCap.LineStipple);

			for (int quad = 0; quad <= 5; quad++)
			{
				if (!_quadIsHidden[quad])
				{
					DrawGridOnQuad(quad);
				}
			}

			GL.Disable(EnableCap.LineStipple);

			// Draw ticks on the closest axes
			_wholeBounds.Reset();
			_wholeBounds.Add(_boxBounds);
			//gl.glPolygonMode(GL2.GL_FRONT_AND_BACK, GL2.GL_LINE);

			// Display x axis ticks
			if (_xrange > 0 && _layout.XTickLabelDisplayed)
			{
				// If we are on top, we make direct axe placement
				if (_view?.ViewMode == ViewPositionMode.TOP)
				{
					BoundingBox3d bbox = DrawTicks(camera, 1, AxeDirection.AxeX, _layout.XTickColor, Halign.LEFT, Valign.TOP);
					// setup tick labels for X on the bottom
					_wholeBounds.Add(bbox);
				}
				else
				{
					// otherwise computed placement
					int xselect = FindClosestXaxe(camera);
					if (xselect >= 0)
					{
						BoundingBox3d bbox = DrawTicks(camera, xselect, AxeDirection.AxeX, _layout.XTickColor);
						_wholeBounds.Add(bbox);
					}
					else
					{
						//System.err.println("no x axe selected: " + Arrays.toString(quadIsHidden));
						// HACK: handles "on top" view, when all face of cube are drawn, which forbid to select an axe automatically
						BoundingBox3d bbox = DrawTicks(camera, 2, AxeDirection.AxeX, _layout.XTickColor, Halign.CENTER, Valign.TOP);
						_wholeBounds.Add(bbox);
					}
				}
			}

			// Display y axis ticks
			if (_yrange > 0 && _layout.YTickLabelDisplayed)
			{
				if (_view?.ViewMode == ViewPositionMode.TOP)
				{
					BoundingBox3d bbox = DrawTicks(camera, 2, AxeDirection.AxeY, _layout.YTickColor, Halign.LEFT, Valign.GROUND);
					// setup tick labels for Y on the left
					_wholeBounds.Add(bbox);
				}
				else
				{
					int yselect = FindClosestYaxe(camera);
					if (yselect >= 0)
					{
						BoundingBox3d bbox = DrawTicks(camera, yselect, AxeDirection.AxeY, _layout.YTickColor);
						_wholeBounds.Add(bbox);
					}
					else
					{
						//System.err.println("no y axe selected: " + Arrays.toString(quadIsHidden));
						// HACK: handles "on top" view, when all face of cube are drawn, which forbid to select an axe automatically
						BoundingBox3d bbox = DrawTicks(camera, 1, AxeDirection.AxeY, _layout.YTickColor, Halign.RIGHT, Valign.GROUND);
						_wholeBounds.Add(bbox);
					}
				}
			}

			// Display z axis ticks
			if (_zrange > 0 && _layout.ZTickLabelDisplayed)
			{
				if (_view?.ViewMode == ViewPositionMode.TOP)
				{
					// ?
				}
				else
				{
					int zselect = FindClosestZaxe(camera);
					if (zselect >= 0)
					{
						BoundingBox3d bbox = DrawTicks(camera, zselect, AxeDirection.AxeZ, _layout.ZTickColor);
						_wholeBounds.Add(bbox);
					}
				}
			}

			// Unset culling
			GL.Disable(EnableCap.CullFace);
		}

		internal void SetAxeBox(float xmin, float xmax, float ymin, float ymax, float zmin, float zmax)
		{
			// Compute center
			_center = new Coord3d((xmax + xmin) / 2, (ymax + ymin) / 2, (zmax + zmin) / 2);
			_xrange = xmax - xmin;
			_yrange = ymax - ymin;
			_zrange = zmax - zmin;

			// Define configuration of 6 quads (faces of the box)
			_quadx = new float[6, 4];
			_quady = new float[6, 4];
			_quadz = new float[6, 4];

			// x near
			_quadx[0, 0] = xmax;
			_quady[0, 0] = ymin;
			_quadz[0, 0] = zmax;

			_quadx[0, 1] = xmax;
			_quady[0, 1] = ymin;
			_quadz[0, 1] = zmin;

			_quadx[0, 2] = xmax;
			_quady[0, 2] = ymax;
			_quadz[0, 2] = zmin;

			_quadx[0, 3] = xmax;
			_quady[0, 3] = ymax;
			_quadz[0, 3] = zmax;

			// x far
			_quadx[1, 0] = xmin;
			_quady[1, 0] = ymax;
			_quadz[1, 0] = zmax;

			_quadx[1, 1] = xmin;
			_quady[1, 1] = ymax;
			_quadz[1, 1] = zmin;

			_quadx[1, 2] = xmin;
			_quady[1, 2] = ymin;
			_quadz[1, 2] = zmin;

			_quadx[1, 3] = xmin;
			_quady[1, 3] = ymin;
			_quadz[1, 3] = zmax;

			// y near
			_quadx[2, 0] = xmax;
			_quady[2, 0] = ymax;
			_quadz[2, 0] = zmax;

			_quadx[2, 1] = xmax;
			_quady[2, 1] = ymax;
			_quadz[2, 1] = zmin;

			_quadx[2, 2] = xmin;
			_quady[2, 2] = ymax;
			_quadz[2, 2] = zmin;

			_quadx[2, 3] = xmin;
			_quady[2, 3] = ymax;
			_quadz[2, 3] = zmax;

			// y far
			_quadx[3, 0] = xmin;
			_quady[3, 0] = ymin;
			_quadz[3, 0] = zmax;

			_quadx[3, 1] = xmin;
			_quady[3, 1] = ymin;
			_quadz[3, 1] = zmin;

			_quadx[3, 2] = xmax;
			_quady[3, 2] = ymin;
			_quadz[3, 2] = zmin;

			_quadx[3, 3] = xmax;
			_quady[3, 3] = ymin;
			_quadz[3, 3] = zmax;

			// z top
			_quadx[4, 0] = xmin;
			_quady[4, 0] = ymin;
			_quadz[4, 0] = zmax;

			_quadx[4, 1] = xmax;
			_quady[4, 1] = ymin;
			_quadz[4, 1] = zmax;

			_quadx[4, 2] = xmax;
			_quady[4, 2] = ymax;
			_quadz[4, 2] = zmax;

			_quadx[4, 3] = xmin;
			_quady[4, 3] = ymax;
			_quadz[4, 3] = zmax;

			// z down
			_quadx[5, 0] = xmax;
			_quady[5, 0] = ymin;
			_quadz[5, 0] = zmin;

			_quadx[5, 1] = xmin;
			_quady[5, 1] = ymin;
			_quadz[5, 1] = zmin;

			_quadx[5, 2] = xmin;
			_quady[5, 2] = ymax;
			_quadz[5, 2] = zmin;

			_quadx[5, 3] = xmax;
			_quady[5, 3] = ymax;
			_quadz[5, 3] = zmin;

			// Define configuration of each quad's normal
			_normx = new float[6];
			_normy = new float[6];
			_normz = new float[6];

			_normx[0] = xmax;
			_normy[0] = 0;
			_normz[0] = 0;

			_normx[1] = xmin;
			_normy[1] = 0;
			_normz[1] = 0;

			_normx[2] = 0;
			_normy[2] = ymax;
			_normz[2] = 0;

			_normx[3] = 0;
			_normy[3] = ymin;
			_normz[3] = 0;

			_normx[4] = 0;
			_normy[4] = 0;
			_normz[4] = zmax;

			_normx[5] = 0;
			_normy[5] = 0;
			_normz[5] = zmin;

			// Define quad intersections that generate an axe
			// axe{A}quads[i][q]
			// A = axe direction (X, Y, or Z)
			// i = axe id (0 to 4)
			// q = quad id (0 to 1: an intersection is made of two quads)
			const int na = 4;

			// n axes per dimension
			const int np = 2;

			// n points for an axe
			const int nq = 2;

			// axe id
			_axeXquads = new int[na, nq];
			_axeYquads = new int[na, nq];
			_axeZquads = new int[na, nq];

			// quads making axe x0
			int i = 0;
			_axeXquads[i, 0] = 4;
			_axeXquads[i, 1] = 3;

			// quads making axe x1
			i = 1;
			_axeXquads[i, 0] = 3;
			_axeXquads[i, 1] = 5;

			// quads making axe x2
			i = 2;
			_axeXquads[i, 0] = 5;
			_axeXquads[i, 1] = 2;

			// quads making axe x3
			i = 3;
			_axeXquads[i, 0] = 2;
			_axeXquads[i, 1] = 4;

			// quads making axe y0
			i = 0;
			_axeYquads[i, 0] = 4;
			_axeYquads[i, 1] = 0;

			// quads making axe y1
			i = 1;
			_axeYquads[i, 0] = 0;
			_axeYquads[i, 1] = 5;

			// quads making axe y2
			i = 2;
			_axeYquads[i, 0] = 5;
			_axeYquads[i, 1] = 1;

			// quads making axe y3
			i = 3;
			_axeYquads[i, 0] = 1;
			_axeYquads[i, 1] = 4;

			// quads making axe z0
			i = 0;
			_axeZquads[i, 0] = 3;
			_axeZquads[i, 1] = 0;

			// quads making axe z1
			i = 1;
			_axeZquads[i, 0] = 0;
			_axeZquads[i, 1] = 2;

			// quads making axe z2
			i = 2;
			_axeZquads[i, 0] = 2;
			_axeZquads[i, 1] = 1;

			// quads making axe z3
			i = 3;
			_axeZquads[i, 0] = 1;
			_axeZquads[i, 1] = 3;

			// Define configuration of 4 axe per dimension:
			//  axe{A}d[i][p], where
			//
			//  A = axe direction (X, Y, or Z)
			//  d = dimension (x coordinate, y coordinate or z coordinate)
			//  i = axe id (0 to 4)
			//  p = point id (0 to 1)
			//
			// Note: the points making an axe are from - to +
			//       (i.e. direction is given by p0->p1)
			_axeXx = new float[na, np];
			_axeXy = new float[na, np];
			_axeXz = new float[na, np];
			_axeYx = new float[na, np];
			_axeYy = new float[na, np];
			_axeYz = new float[na, np];
			_axeZx = new float[na, np];
			_axeZy = new float[na, np];
			_axeZz = new float[na, np];

			i = 0;
			// axe x0
			_axeXx[i, 0] = xmin;
			_axeXy[i, 0] = ymin;
			_axeXz[i, 0] = zmax;
			_axeXx[i, 1] = xmax;
			_axeXy[i, 1] = ymin;
			_axeXz[i, 1] = zmax;

			i = 1;
			// axe x1
			_axeXx[i, 0] = xmin;
			_axeXy[i, 0] = ymin;
			_axeXz[i, 0] = zmin;
			_axeXx[i, 1] = xmax;
			_axeXy[i, 1] = ymin;
			_axeXz[i, 1] = zmin;

			i = 2;
			// axe x2
			_axeXx[i, 0] = xmin;
			_axeXy[i, 0] = ymax;
			_axeXz[i, 0] = zmin;
			_axeXx[i, 1] = xmax;
			_axeXy[i, 1] = ymax;
			_axeXz[i, 1] = zmin;

			i = 3;
			// axe x3
			_axeXx[i, 0] = xmin;
			_axeXy[i, 0] = ymax;
			_axeXz[i, 0] = zmax;
			_axeXx[i, 1] = xmax;
			_axeXy[i, 1] = ymax;
			_axeXz[i, 1] = zmax;

			i = 0;
			// axe y0
			_axeYx[i, 0] = xmax;
			_axeYy[i, 0] = ymin;
			_axeYz[i, 0] = zmax;
			_axeYx[i, 1] = xmax;
			_axeYy[i, 1] = ymax;
			_axeYz[i, 1] = zmax;

			i = 1;
			// axe y1
			_axeYx[i, 0] = xmax;
			_axeYy[i, 0] = ymin;
			_axeYz[i, 0] = zmin;
			_axeYx[i, 1] = xmax;
			_axeYy[i, 1] = ymax;
			_axeYz[i, 1] = zmin;

			i = 2;
			// axe y2
			_axeYx[i, 0] = xmin;
			_axeYy[i, 0] = ymin;
			_axeYz[i, 0] = zmin;
			_axeYx[i, 1] = xmin;
			_axeYy[i, 1] = ymax;
			_axeYz[i, 1] = zmin;

			i = 3;
			// axe y3
			_axeYx[i, 0] = xmin;
			_axeYy[i, 0] = ymin;
			_axeYz[i, 0] = zmax;
			_axeYx[i, 1] = xmin;
			_axeYy[i, 1] = ymax;
			_axeYz[i, 1] = zmax;

			i = 0;
			// axe z0
			_axeZx[i, 0] = xmax;
			_axeZy[i, 0] = ymin;
			_axeZz[i, 0] = zmin;
			_axeZx[i, 1] = xmax;
			_axeZy[i, 1] = ymin;
			_axeZz[i, 1] = zmax;

			i = 1;
			// axe z1
			_axeZx[i, 0] = xmax;
			_axeZy[i, 0] = ymax;
			_axeZz[i, 0] = zmin;
			_axeZx[i, 1] = xmax;
			_axeZy[i, 1] = ymax;
			_axeZz[i, 1] = zmax;

			i = 2;
			// axe z2
			_axeZx[i, 0] = xmin;
			_axeZy[i, 0] = ymax;
			_axeZz[i, 0] = zmin;
			_axeZx[i, 1] = xmin;
			_axeZy[i, 1] = ymax;
			_axeZz[i, 1] = zmax;

			i = 3;
			// axe z3
			_axeZx[i, 0] = xmin;
			_axeZy[i, 0] = ymin;
			_axeZz[i, 0] = zmin;
			_axeZx[i, 1] = xmin;
			_axeZy[i, 1] = ymin;
			_axeZz[i, 1] = zmax;

			_layout.XTicks(xmin, xmax);
			// prepare ticks to display in the layout tick buffer
			_layout.YTicks(ymin, ymax);
			_layout.ZTicks(zmin, zmax);
			//setXTickMode(TICK_REGULAR, 3);5
			//setYTickMode(TICK_REGULAR, 3);5
			//setZTickMode(TICK_REGULAR, 5);6
		}

		/// <summary>
		/// Make all GL2 calls allowing to build a cube with 6 separate quads.
		/// Each quad is indexed from 0.0f to 5.0f using glPassThrough,
		/// and may be traced in feedback mode when mode=<see cref="RenderingMode.Feedback"/>
		/// </summary>
		public void DrawCube(RenderingMode mode)
		{
			for (int q = 0; q <= 5; q++)
			{
				if (mode == RenderingMode.Feedback)
				{
					GL.PassThrough(q);
				}

				GL.Begin(PrimitiveType.Quads);
				for (int v = 0; v <= 3; v++)
				{
					GL.Vertex3(_quadx[q, v], _quady[q, v], _quadz[q, v]);
				}
				GL.End();
			}
		}

		/// <summary>
		/// Draw a grid on the desired quad.
		/// </summary>
		/// <param name="quad">Quad number, from 0 to 5</param>
		internal void DrawGridOnQuad(int quad)
		{
			// Draw X grid along X axis
			if ((quad != 0) && (quad != 1))
			{
				float[] xticks = _layout.XTicks();
				for (int t = 0; t <= xticks.Length - 1; t++)
				{
					GL.Begin(PrimitiveType.Lines);
					GL.Vertex3(xticks[t], _quady[quad, 0], _quadz[quad, 0]);
					GL.Vertex3(xticks[t], _quady[quad, 2], _quadz[quad, 2]);
					GL.End();
				}
			}

			// Draw Y grid along Y axis
			if ((quad != 2) && (quad != 3))
			{
				float[] yticks = _layout.YTicks();
				for (int t = 0; t <= yticks.Length - 1; t++)
				{
					GL.Begin(PrimitiveType.Lines);
					GL.Vertex3(_quadx[quad, 0], yticks[t], _quadz[quad, 0]);
					GL.Vertex3(_quadx[quad, 2], yticks[t], _quadz[quad, 2]);
					GL.End();
				}
			}

			// Draw Z grid along Z axis
			if ((quad != 4) && (quad != 5))
			{
				float[] zticks = _layout.ZTicks();
				if (zticks != null)
				{
					for (int t = 0; t <= zticks.Length - 1; t++)
					{
						GL.Begin(PrimitiveType.Lines);
						GL.Vertex3(_quadx[quad, 0], _quady[quad, 0], zticks[t]);
						GL.Vertex3(_quadx[quad, 2], _quady[quad, 2], zticks[t]);
						GL.End();
					}
				}
			}
		}

		internal BoundingBox3d DrawTicks(Camera cam, int axis, AxeDirection direction, Color color)
		{
			return DrawTicks(cam, axis, direction, color, Halign.DEFAULT, Valign.DEFAULT);
		}

		internal BoundingBox3d DrawTicks(Camera cam, int axis, AxeDirection direction, Color color, Halign hal, Valign val)
		{
			const float tickLength = 20.0f; // with respect to range
			const float axeLabelDist = 2.5f;

			var ticksTxtBounds = new BoundingBox3d();

			int quad_0;
			int quad_1;
			// Retrieve the quads that intersect and create the selected axe
			switch (direction)
			{
				case AxeDirection.AxeX:
					quad_0 = _axeXquads[axis, 0];
					quad_1 = _axeXquads[axis, 1];
					break;

				case AxeDirection.AxeY:
					quad_0 = _axeYquads[axis, 0];
					quad_1 = _axeYquads[axis, 1];
					break;

				case AxeDirection.AxeZ:
					quad_0 = _axeZquads[axis, 0];
					quad_1 = _axeZquads[axis, 1];
					break;

				default:
					throw new Exception("Unsupported axe direction");
			}

			// Computes PoSition of ticks lying on the selected axe
			// (i.e. 1st point of the tick line)
			float xpos = _normx[quad_0] + _normx[quad_1];
			float ypos = _normy[quad_0] + _normy[quad_1];
			float zpos = _normz[quad_0] + _normz[quad_1];

			// Computes the DIRection of the ticks
			// assuming initial vector point is the center
			float xdir = (float)(_normx[quad_0] + _normx[quad_1] - _center.X);
			float ydir = (float)(_normy[quad_0] + _normy[quad_1] - _center.Y);
			float zdir = (float)(_normz[quad_0] + _normz[quad_1] - _center.Z);
			xdir = xdir == 0 ? 0 : xdir / MathF.Abs(xdir); // so that direction as length 1
			ydir = ydir == 0 ? 0 : ydir / MathF.Abs(ydir);
			zdir = zdir == 0 ? 0 : zdir / MathF.Abs(zdir);
			const int dist = 1;

			// Draw the label for axis
			string axeLabel;
			float zlab;
			float ylab;
			float xlab;

			// Variables for storing the position of the Label position
			// (2nd point on the tick line)
			switch (direction)
			{
				case AxeDirection.AxeX:
					xlab = (float)_center.X;
					ylab = axeLabelDist * (_yrange / tickLength) * dist * ydir + ypos;
					zlab = axeLabelDist * (_zrange / tickLength) * dist * zdir + zpos;
					axeLabel = _layout.XAxeLabel;
					break;

				case AxeDirection.AxeY:
					xlab = axeLabelDist * (_xrange / tickLength) * dist * xdir + xpos;
					ylab = (float)_center.Y;
					zlab = axeLabelDist * (_zrange / tickLength) * dist * zdir + zpos;
					axeLabel = _layout.YAxeLabel;
					break;

				case AxeDirection.AxeZ:
					xlab = axeLabelDist * (_xrange / tickLength) * dist * xdir + xpos;
					ylab = axeLabelDist * (_yrange / tickLength) * dist * ydir + ypos;
					zlab = (float)_center.Z;
					axeLabel = _layout.ZAxeLabel;
					break;

				default:
					throw new Exception("Unsupported axe direction");
			}

			DrawAxisLabel(cam, direction, color, ticksTxtBounds, xlab, ylab, zlab, axeLabel);
			DrawAxisTicks(cam, direction, color, hal, val, tickLength, ticksTxtBounds, xpos, ypos, zpos, xdir, ydir, zdir, GetAxisTicks(direction));
			return ticksTxtBounds;
		}

		public void DrawAxisLabel(Camera cam, AxeDirection direction, Color color, BoundingBox3d ticksTxtBounds, float xlab, float ylab, float zlab, String axeLabel)
		{
			if ((direction == AxeDirection.AxeX && _layout.XAxeLabelDisplayed)
			 || (direction == AxeDirection.AxeY && _layout.YAxeLabelDisplayed)
			 || (direction == AxeDirection.AxeZ && _layout.ZAxeLabelDisplayed))
			{
				var labelPosition = new Coord3d(xlab, ylab, zlab);
				BoundingBox3d labelBounds = _txt.DrawText(cam, axeLabel, labelPosition, Halign.CENTER, Valign.CENTER, color);
				if (labelBounds != null)
					ticksTxtBounds.Add(labelBounds);
			}
		}

		public float[] GetAxisTicks(AxeDirection direction)
		{
			float[] ticks;
			switch (direction)
			{
				case AxeDirection.AxeX:
					ticks = _layout.XTicks();
					break;

				case AxeDirection.AxeY:
					ticks = _layout.YTicks();
					break;

				case AxeDirection.AxeZ:
					ticks = _layout.ZTicks();
					break;

				default:
					throw new Exception("Unsupported axe direction");
			}
			return ticks;
		}

		public void DrawAxisTicks(Camera cam, AxeDirection direction, Color color, Halign hal, Valign val, float tickLength, BoundingBox3d ticksTxtBounds, float xpos,
			float ypos, float zpos, float xdir, float ydir, float zdir, float[] ticks)
		{
			float xlab;
			float ylab;
			float zlab;
			for (int t = 0; t < ticks.Length; t++)
			{
				string tickLabel;
				// Shift the tick vector along the selected axis
				// and set the tick length
				switch (direction)
				{
					case AxeDirection.AxeX:
						xpos = ticks[t];
						xlab = xpos;
						ylab = _yrange / tickLength * ydir + ypos;
						zlab = _zrange / tickLength * zdir + zpos;
						tickLabel = _layout.XTickRenderer.Format(xpos);
						break;

					case AxeDirection.AxeY:
						ypos = ticks[t];
						xlab = _xrange / tickLength * xdir + xpos;
						ylab = ypos;
						zlab = _zrange / tickLength * zdir + zpos;
						tickLabel = _layout.YTickRenderer.Format(ypos);
						break;

					case AxeDirection.AxeZ:
						zpos = ticks[t];
						xlab = _xrange / tickLength * xdir + xpos;
						ylab = _yrange / tickLength * ydir + ypos;
						zlab = zpos;
						tickLabel = _layout.ZTickRenderer.Format(zpos);
						break;

					default:
						throw new Exception("Unsupported axe direction");
				}

				var tickPosition = new Coord3d(xlab, ylab, zlab);

				if (_layout.TickLineDisplayed)
				{
					DrawTickLine(color, xpos, ypos, zpos, xlab, ylab, zlab);
				}

				// Select the alignement of the tick label
				Halign hAlign = LayoutHorizontal(direction, cam, hal, tickPosition);
				Valign vAlign = LayoutVertical(direction, val, zdir);

				// Draw the text label of the current tick
				DrawAxisTickNumericLabel(direction, cam, color, hAlign, vAlign, ticksTxtBounds, tickLabel, tickPosition);
			}
		}

		public void DrawAxisTickNumericLabel(AxeDirection direction, Camera cam, Color color,
			Halign hAlign, Valign vAlign, BoundingBox3d ticksTxtBounds, String tickLabel, Coord3d tickPosition)
		{
			GL.LoadIdentity();
			GL.Scale(_scale.X, _scale.Y, _scale.Z);

			BoundingBox3d tickBounds = _txt.DrawText(cam, tickLabel, tickPosition, hAlign, vAlign, color);
			if (tickBounds != null)
			{
				ticksTxtBounds.Add(tickBounds);
			}
		}

		public Valign LayoutVertical(AxeDirection direction, Valign val, float zdir)
		{
			Valign vAlign;
			if (val == null || val == Valign.DEFAULT)
			{
				if (direction == AxeDirection.AxeZ)
				{
					vAlign = Valign.CENTER;
				}
				else
				{
					if (zdir > 0)
					{
						vAlign = Valign.TOP;
					}
					else
					{
						vAlign = Valign.BOTTOM;
					}
				}
			}
			else
			{
				vAlign = val;
			}

			return vAlign;
		}

		public Halign LayoutHorizontal(AxeDirection direction, Camera cam, Halign hal, Coord3d tickPosition)
		{
			if (hal == null || hal == Halign.DEFAULT)
			{
				return cam.Side(tickPosition) ? Halign.LEFT : Halign.RIGHT;
			}
			else
			{
				return hal;
			}
		}

		public void DrawTickLine(Color color, double xpos, double ypos, double zpos, double xlab, double ylab, double zlab)
		{
			GL.Color3(color.R, color.G, color.B);
			GL.LineWidth(1);

			// Draw the tick line
			GL.Begin(PrimitiveType.Lines);
			GL.Vertex3(xpos, ypos, zpos);
			GL.Vertex3(xlab, ylab, zlab);
			GL.End();
		}

		/// <summary>
		/// Selects the closest displayable X axe from camera
		/// </summary>
		internal int FindClosestXaxe(Camera cam)
		{
			const int na = 4;
			double[] distAxeX = new double[na];

			// keeps axes that are not at intersection of 2 quads
			for (int a = 0; a <= na - 1; a++)
			{
				if (_quadIsHidden[_axeXquads[a, 0]] ^ _quadIsHidden[_axeXquads[a, 1]])
				{
					distAxeX[a] = new Vector3d(_axeXx[a, 0], _axeXy[a, 0], _axeXz[a, 0], _axeXx[a, 1], _axeXy[a, 1], _axeXz[a, 1]).Distance(cam.Eye);
				}
				else
				{
					distAxeX[a] = double.MaxValue;
				}
			}

			// prefers the lower one
			for (int a = 0; a <= na - 1; a++)
			{
				if (distAxeX[a] < double.MaxValue && (Center.Z > (_axeXz[a, 0] + _axeXz[a, 1]) / 2))
				{
					distAxeX[a] *= -1;
				}
			}

			return Min(distAxeX);
		}

		/// <summary>
		/// Selects the closest displayable Y axe from camera
		/// </summary>
		internal int FindClosestYaxe(Camera cam)
		{
			const int na = 4;
			double[] distAxeY = new double[na];

			// keeps axes that are not at intersection of 2 quads
			for (int a = 0; a <= na - 1; a++)
			{
				if (_quadIsHidden[_axeYquads[a, 0]] ^ _quadIsHidden[_axeYquads[a, 1]])
				{
					distAxeY[a] = new Vector3d(_axeYx[a, 0], _axeYy[a, 0], _axeYz[a, 0], _axeYx[a, 1], _axeYy[a, 1], _axeYz[a, 1]).Distance(cam.Eye);
				}
				else
				{
					distAxeY[a] = double.MaxValue;
				}
			}

			// prefers the lower one
			for (int a = 0; a <= na - 1; a++)
			{
				if (distAxeY[a] < double.MaxValue && Center.Z > (_axeYz[a, 0] + _axeYz[a, 1]) / 2)
				{
					distAxeY[a] *= -1;
				}
			}

			return Min(distAxeY);
		}

		/// <summary>
		/// Selects the closest displayable Z axe from camera
		/// </summary>
		internal int FindClosestZaxe(Camera cam)
		{
			const int na = 4;
			double[] distAxeZ = new double[na];

			// keeps axes that are not at intersection of 2 quads
			for (int a = 0; a <= na - 1; a++)
			{
				if (_quadIsHidden[_axeZquads[a, 0]] ^ _quadIsHidden[_axeZquads[a, 1]])
				{
					distAxeZ[a] = new Vector3d(_axeZx[a, 0], _axeZy[a, 0], _axeZz[a, 0], _axeZx[a, 1], _axeZy[a, 1], _axeZz[a, 1]).Distance(cam.Eye);
				}
				else
				{
					distAxeZ[a] = double.MaxValue;
				}
			}

			// prefers the lower one
			for (int a = 0; a <= na - 1; a++)
			{
				if (distAxeZ[a] < double.MaxValue)
				{
					var axeCEnter = new Coord3d((_axeZx[a, 0] + _axeZx[a, 1]) / 2, (_axeZy[a, 0] + _axeZy[a, 1]) / 2, (_axeZz[a, 0] + _axeZz[a, 1]) / 2);
					if (!cam.Side(axeCEnter))
					{
						distAxeZ[a] *= -1;
					}
				}
			}

			return Min(distAxeZ);
		}

		protected static int Min(double[] values)
		{
			double minv = double.MaxValue;
			int index = -1;
			for (int i = 0; i <= values.Length - 1; i++)
			{
				if (values[i] < minv)
				{
					minv = values[i];
					index = i;
				}
			}
			return index;
		}

		/// <summary>
		/// Computes the visibility of each cube face.
		/// </summary>
		internal bool[] GetHiddenQuads(Camera cam)
		{
			bool[] status = new bool[6];
			Coord3d se = cam.Eye.Divide(_scale);

			if (se.X <= _center.X)
			{
				status[0] = false;
				status[1] = true;
			}
			else
			{
				status[0] = true;
				status[1] = false;
			}

			if (se.Y <= _center.Y)
			{
				status[2] = false;
				status[3] = true;
			}
			else
			{
				status[2] = true;
				status[3] = false;
			}

			if (se.Z <= _center.Z)
			{
				status[4] = false;
				status[5] = true;
			}
			else
			{
				status[4] = true;
				status[5] = false;
			}

			return status;
		}

		/// <summary>
		/// Print out parameters of a gl call in 3dColor mode
		/// </summary>
		internal static int Print3DcolorVertex(int size, int count, float[] buffer)
		{
			int id = size - count;
			const int veclength = 7;
			System.Diagnostics.Debug.WriteLine("  [" + id + "]");
			for (int i = 0; i <= veclength - 1; i++)
			{
				System.Diagnostics.Debug.WriteLine(" " + buffer[size - count]);
				count--;
			}
			System.Diagnostics.Debug.WriteLine("");
			return count;
		}

		internal void PrintHiddenQuads()
		{
			for (int t = 0; t <= _quadIsHidden.Length - 1; t++)
			{
				if (_quadIsHidden[t])
				{
					System.Diagnostics.Debug.WriteLine("Quad[" + t + "] is not displayed");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Quad[" + t + "] is displayed");
				}
			}
		}

		public BoundingBox3d GetBoxBounds()
		{
			return _boxBounds;
		}

		public Coord3d GetCenter()
		{
			return this.Center;
		}

		public IAxeLayout GetLayout()
		{
			return _layout;
		}

		public void SetAxe(BoundingBox3d box)
		{
			_boxBounds = box;
			SetAxeBox((float)box.XMin, (float)box.XMax, (float)box.YMin, (float)box.YMax, (float)box.ZMin, (float)box.ZMax);
		}

		public BoundingBox3d WholeBounds
		{
			get { return _wholeBounds; }
		}

		public Coord3d Center
		{
			get { return _center; }
		}

		public Coord3d Scale
		{
			set { _scale = value; }
		}

		public void SetScale(Coord3d scale)
		{
			this.Scale = scale;
		}
	}
}
