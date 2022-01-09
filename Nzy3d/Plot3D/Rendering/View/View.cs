using Nzy3d.Colors;
using Nzy3d.Events;
using Nzy3d.Factories;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives;
using Nzy3d.Plot3D.Primitives.Axes;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View.Modes;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Rendering.View
{
	public class View
	{
		public const float STRETCH_RATIO = 0.25f;

		// force to have all object maintained in screen, meaning axebox won't always keep the same size.
		internal bool MAINTAIN_ALL_OBJECTS_IN_VIEW = false;

		// display a magenta parallelepiped (debug)
#if DEBUG
		internal bool DISPLAY_AXE_WHOLE_BOUNDS = true;
#else
		internal bool DISPLAY_AXE_WHOLE_BOUNDS = false;
#endif

		internal bool _axeBoxDisplayed = true;
		internal bool _squared = true;
		internal Camera _cam;
		internal IAxe _axe;
		internal Quality _quality;
		// TODO : Implement overlay
		// Friend _overlay As Overlay 
		internal Scene _scene;
		internal ICanvas _canvas;
		internal Coord3d _viewpoint;
		internal Coord3d _center;
		internal Coord3d _scaling;
		internal BoundingBox3d _viewbounds;
		internal CameraMode _cameraMode;
		internal ViewPositionMode _viewmode;
		internal ViewBoundMode _boundmode;
		internal ImageViewport _bgViewport;
		//internal System.Drawing.Bitmap _bgImg = null;
		internal BoundingBox3d _targetBox;
		internal Color _bgColor = Color.BLACK;
		internal Color _bgOverlay = new Color(0, 0, 0, 0);
		// TODO : Implement overlay
		//Friend _tooltips As List(Of ITooltipRenderer) 
		internal List<IBaseRenderer2D> _renderers;
		internal List<IViewPointChangedListener> _viewPointChangedListeners;
		internal List<IViewIsVerticalEventListener> _viewOnTopListeners;
		internal bool _wasOnTopAtLastRendering;
		static internal float PI_div2 = MathF.PI / 2f;
		public static readonly Coord3d DEFAULT_VIEW = new Coord3d(MathF.PI / 3f, MathF.PI / 3f, 2000);
		internal bool _dimensionDirty = false;
		internal bool _viewDirty = false;

		static internal View Current;
		public View(Scene scene, ICanvas canvas, Quality quality)
		{
			BoundingBox3d sceneBounds = scene.Graph.Bounds;
			_viewpoint = DEFAULT_VIEW.Clone();
			_center = sceneBounds.GetCenter();
			_scaling = Coord3d.IDENTITY.Clone();
			_viewmode = ViewPositionMode.FREE;
			_boundmode = ViewBoundMode.AUTO_FIT;
			_cameraMode = CameraMode.PERSPECTIVE;
			_axe = (IAxe)AxeFactory.GetInstance(sceneBounds, this);
			_cam = CameraFactory.GetInstance(_center);
			_scene = scene;
			_canvas = canvas;
			_quality = quality;
			_renderers = new List<IBaseRenderer2D>();
			//_tooltips = New List(Of ITooltipRenderer)
			_bgViewport = new ImageViewport();
			_viewOnTopListeners = new List<IViewIsVerticalEventListener>();
			_viewPointChangedListeners = new List<IViewPointChangedListener>();
			_wasOnTopAtLastRendering = false;
			//_overlay = New Overlay
			View.Current = this;
		}

		public void Dispose()
		{
			_axe.Dispose();
			_cam = null;
			_renderers.Clear();
			_viewOnTopListeners.Clear();
			_scene = null;
			_canvas = null;
			_quality = null;
		}

		public void Shoot()
		{
			_canvas.ForceRepaint();
		}

		public void Project()
		{
			_scene.Graph.Project(_cam);
		}

		public Coord3d ProjectMouse(int x, int y)
		{
			return _cam.ScreenToModel(new Coord3d(x, y, 0));
		}

		#region "GENERAL DISPLAY CONTROLS"
		public void Rotate(Coord2d move)
		{
			Rotate(move, true);
		}

		public void Rotate(Coord2d move, bool updateView)
		{
			Coord3d eye = this.ViewPoint;
			eye.X -= move.X;
			eye.Y += move.Y;
			SetViewPoint(eye, updateView);
			//fireControllerEvent(ControllerType.ROTATE, eye);
		}

		public void Shift(float factor)
		{
			Shift(factor, true);
		}

		public void Shift(float factor, bool updateView)
		{
			Scale current = this.Scale;
			Scale newScale = current.Add(factor * current.Range);
			SetScale(newScale, updateView);
			//fireControllerEvent(ControllerType.SHIFT, newScale);
		}

		public void Zoom(float factor)
		{
			Zoom(factor, true);
		}

		public void Zoom(float factor, bool updateView)
		{
			Scale current = this.Scale;
			float range = current.Max - current.Min;

			if (range <= 0)
			{
				return;
			}

			float center = (current.Max + current.Min) / 2;
			float zmin = center + (current.Min - center) * factor;
			float zmax = center + (current.Max - center) * factor;

			// set min/max according to bounds
			Scale scale = null;
			if (zmin < zmax)
			{
				scale = new Scale(zmin, zmax);
			}
			else
			{
				// forbid to have zmin = zmax if we zoom in
				if (factor < 1)
				{
					scale = new Scale(center, center);
				}
			}

			if (scale != null)
			{
				SetScale(scale, updateView);
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomX(float factor)
		{
			ZoomX(factor, true);
		}

		public void ZoomX(float factor, bool updateView)
		{
			float range = this.Bounds.XMax - this.Bounds.XMin;
			if (range <= 0)
			{
				return;
			}
			float center = (this.Bounds.XMax + this.Bounds.XMin) / 2;
			float min = center + (this.Bounds.XMin - center) * factor;
			float max = center + (this.Bounds.XMax - center) * factor;

			// set min/max according to bounds
			Scale scale = null;
			if (min < max)
			{
				scale = new Scale(min, max);
			}
			else
			{
				// forbid to have min = max if we zoom in
				if (factor < 1)
				{
					scale = new Scale(center, center);
				}
			}

			if (scale != null)
			{
				BoundingBox3d bounds = this.Bounds;
				bounds.XMin = scale.Min;
				bounds.XMax = scale.Max;
				this.BoundManual = bounds;
				if (updateView)
				{
					Shoot();
				}
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomY(float factor)
		{
			ZoomY(factor, true);
		}

		public void ZoomY(float factor, bool updateView)
		{
			float range = this.Bounds.YMax - this.Bounds.YMin;
			if (range <= 0)
			{
				return;
			}
			float center = (this.Bounds.YMax + this.Bounds.YMin) / 2;
			float min = center + (this.Bounds.YMin - center) * factor;
			float max = center + (this.Bounds.YMax - center) * factor;

			// set min/max according to bounds
			Scale scale = null;
			if (min < max)
			{
				scale = new Scale(min, max);
			}
			else
			{
				// forbid to have min = max if we zoom in
				if (factor < 1)
				{
					scale = new Scale(center, center);
				}
			}

			if (scale != null)
			{
				BoundingBox3d bounds = this.Bounds;
				bounds.YMin = scale.Min;
				bounds.YMax = scale.Max;
				this.BoundManual = bounds;
				if (updateView)
				{
					Shoot();
				}
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomZ(float factor)
		{
			ZoomZ(factor, true);
		}

		public void ZoomZ(float factor, bool updateView)
		{
			float range = this.Bounds.ZMax - this.Bounds.ZMin;
			if (range <= 0)
			{
				return;
			}
			float center = (this.Bounds.ZMax + this.Bounds.ZMin) / 2;
			float min = center + (this.Bounds.ZMin - center) * factor;
			float max = center + (this.Bounds.ZMax - center) * factor;

			// set min/max according to bounds
			Scale scale = null;
			if (min < max)
			{
				scale = new Scale(min, max);
			}
			else
			{
				// forbid to have min = max if we zoom in
				if (factor < 1)
				{
					scale = new Scale(center, center);
				}
			}

			if (scale != null)
			{
				BoundingBox3d bounds = this.Bounds;
				bounds.ZMin = scale.Min;
				bounds.ZMax = scale.Max;
				this.BoundManual = bounds;
				if (updateView)
				{
					Shoot();
				}
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomXYZ(float factor)
		{
			ZoomXYZ(factor, true);
		}

		public void ZoomXYZ(float factor, bool updateView)
		{
			Scale scaleX = null;
			Scale scaleY = null;
			Scale scaleZ = null;

			// X
			float rangeX = this.Bounds.XMax - this.Bounds.XMin;
			if (rangeX > 0)
			{
				float centerX = (this.Bounds.XMax + this.Bounds.XMin) / 2;
				float minX = centerX + (this.Bounds.XMin - centerX) * factor;
				float maxX = centerX + (this.Bounds.XMax - centerX) * factor;

				// set min/max according to bounds
				if (minX < maxX)
				{
					scaleX = new Scale(minX, maxX);
				}
				else
				{
					// forbid to have min = max if we zoom in
					if (factor < 1)
					{
						scaleX = new Scale(centerX, centerX);
					}
				}
			}

			// Y
			float rangeY = this.Bounds.YMax - this.Bounds.YMin;
			if (rangeY > 0)
			{
				float centerY = (this.Bounds.YMax + this.Bounds.YMin) / 2;
				float minY = centerY + (this.Bounds.YMin - centerY) * factor;
				float maxY = centerY + (this.Bounds.YMax - centerY) * factor;

				// set min/max according to bounds
				if (minY < maxY)
				{
					scaleY = new Scale(minY, maxY);
				}
				else
				{
					// forbid to have min = max if we zoom in
					if (factor < 1)
					{
						scaleY = new Scale(centerY, centerY);
					}
				}
			}

			// Z
			float rangeZ = this.Bounds.ZMax - this.Bounds.ZMin;
			if (rangeZ > 0)
			{
				float centerZ = (this.Bounds.ZMax + this.Bounds.ZMin) / 2;
				float minZ = centerZ + (this.Bounds.ZMin - centerZ) * factor;
				float maxZ = centerZ + (this.Bounds.ZMax - centerZ) * factor;

				// set min/max according to bounds
				if (minZ < maxZ)
				{
					scaleZ = new Scale(minZ, maxZ);
				}
				else
				{
					// forbid to have min = max if we zoom in
					if (factor < 1)
					{
						scaleZ = new Scale(centerZ, centerZ);
					}
				}
			}

			// Apply
			if (scaleX == null && scaleY == null && scaleZ == null) return;

			BoundingBox3d bounds = this.Bounds;
			if (scaleX != null)
			{
				bounds.XMin = scaleX.Min;
				bounds.XMax = scaleX.Max;
			}

			if (scaleY != null)
			{
				bounds.YMin = scaleY.Min;
				bounds.YMax = scaleY.Max;
			}

			if (scaleZ != null)
			{
				bounds.ZMin = scaleZ.Min;
				bounds.ZMax = scaleZ.Max;
			}

			this.BoundManual = bounds;
			if (updateView)
			{
				Shoot();
			}
		}

		public bool DimensionDirty
		{
			get { return _dimensionDirty; }
			set { _dimensionDirty = value; }
		}

		public Scale Scale
		{
			get { return new Scale(this.Bounds.ZMin, this.Bounds.ZMax); }
			set { SetScale(value, true); }
		}

		public void SetScale(Scale scale, bool notify)
		{
			BoundingBox3d bounds = this.Bounds;
			bounds.ZMin = scale.Min;
			bounds.ZMax = scale.Max;
			this.BoundManual = bounds;
			if (notify)
			{
				Shoot();
			}
		}

		/// <summary>
		/// Set the surrounding AxeBox dimensions and the Camera target, and the
		/// colorbar range.
		/// </summary>
		public void LookToBox(BoundingBox3d box)
		{
			_center = box.GetCenter();
			_axe.SetAxe(box);
			_targetBox = box;
		}

		/// <summary>
		/// Get the <see cref="AxeBox"/>'s bounds
		/// </summary>
		public BoundingBox3d Bounds
		{
			get { return _axe.GetBoxBounds(); }
		}

		public ViewBoundMode BoundsMode
		{
			get { return _boundmode; }
		}

		/// <summary>
		/// Set the ViewPositionMode applied to this view.
		/// </summary>
		public ViewPositionMode ViewMode
		{
			get { return _viewmode; }
			set { _viewmode = value; }
		}

		public Coord3d ViewPoint
		{
			get { return _viewpoint; }
			set { SetViewPoint(value, true); }
		}

		public void SetViewPoint(Coord3d polar, bool updateView)
		{
			_viewpoint = polar;
			_viewpoint.Y = (_viewpoint.Y < -PI_div2 ? -PI_div2 : _viewpoint.Y);
			_viewpoint.Y = (_viewpoint.Y > PI_div2 ? PI_div2 : _viewpoint.Y);
			if (updateView)
			{
				Shoot();
			}
			FireViewPointChangedEvent(new ViewPointChangedEventArgs(this, polar));
		}

		public Coord3d GetLastViewScaling()
		{
			return _scaling;
		}

		public IAxe Axe
		{
			get { return _axe; }
			set
			{
				_axe = value;
				UpdateBounds();
			}
		}

		public bool Squared
		{
			get { return _squared; }
			set { _squared = value; }
		}

		public bool AxeBoxDisplayed
		{
			get { return _axeBoxDisplayed; }
			set { _axeBoxDisplayed = value; }
		}

		public Color BackgroundColor
		{
			get { return _bgColor; }
			set { _bgColor = value; }
		}

		//public System.Drawing.Bitmap BackgroundImage {
		//	get { return _bgImg; }
		//	set {
		//		_bgImg = value;
		//		_bgViewport.Image = _bgImg;
		//	}
		//}

		public Camera Camera
		{
			get { return _cam; }
		}

		/// <summary>
		/// Get the projection of this view, either CameraMode.ORTHOGONAL or CameraMode.PERSPECTIVE.
		/// </summary>
		public CameraMode CameraMode
		{
			get { return _cameraMode; }
			set { _cameraMode = value; }
		}

		public bool Maximized
		{
			get { return _cam.StretchToFill; }
			set { _cam.StretchToFill = value; }
		}

		public Scene Scene
		{
			get { return _scene; }
		}

		public System.Drawing.Rectangle SceneViewportRectangle
		{
			get { return _cam.Rectange; }
		}

		public ICanvas Canvas
		{
			get { return _canvas; }
		}

		public void AddRenderer2d(IBaseRenderer2D renderer)
		{
			_renderers.Add(renderer);
		}

		public void RemoveRenderer2d(IBaseRenderer2D renderer)
		{
			_renderers.Remove(renderer);
		}

		public void AddViewOnTopEventListener(IViewIsVerticalEventListener listener)
		{
			_viewOnTopListeners.Add(listener);
		}

		public void RemoveViewOnTopEventListener(IViewIsVerticalEventListener listener)
		{
			_viewOnTopListeners.Remove(listener);
		}

		internal void FireViewOnTopEvent(bool isOnTop)
		{
			var e = new ViewIsVerticalEventArgs(this);
			if (isOnTop)
			{
				foreach (IViewIsVerticalEventListener listener in _viewOnTopListeners)
				{
					listener.ViewVerticalReached(e);
				}
			}
			else
			{
				foreach (IViewIsVerticalEventListener listener in _viewOnTopListeners)
				{
					listener.ViewVerticalLeft(e);
				}
			}
		}

		public void AddViewPointChangedListener(IViewPointChangedListener listener)
		{
			_viewPointChangedListeners.Add(listener);
		}

		public void RemoveViewPointChangedListener(IViewPointChangedListener listener)
		{
			_viewPointChangedListeners.Remove(listener);
		}

		internal void FireViewPointChangedEvent(ViewPointChangedEventArgs e)
		{
			foreach (IViewPointChangedListener vp in _viewPointChangedListeners)
			{
				vp.ViewPointChanged(e);
			}
		}

		/// <summary>
		/// Select between an automatic bounding (that allows fitting the entire scene graph), or a custom bounding.
		/// </summary>
		public ViewBoundMode BoundMode
		{
			set
			{
				_boundmode = value;
				UpdateBounds();
			}
		}

		/// <summary>
		/// Set the bounds of the view according to the current BoundMode, and orders a Camera.shoot().
		/// </summary>
		public void UpdateBounds()
		{
			switch (_boundmode)
			{
				case ViewBoundMode.AUTO_FIT:
					LookToBox(Scene.Graph.Bounds);
					// set axe and camera
					break;
				case ViewBoundMode.MANUAL:
					LookToBox(_viewbounds);
					// set axe and camera
					break;
				default:
					throw new Exception("Unsupported bound mode : " + _boundmode);
			}
			Shoot();
		}

		/// <summary>
		/// Update the bounds according to the scene graph whatever is the current
		/// BoundMode, and orders a shoot() if refresh is True
		/// </summary>
		/// <param name="refresh">Wether to order a shoot() or not.</param>
		public void UpdateBoundsForceUpdate(bool refresh)
		{
			LookToBox(Scene.Graph.Bounds);
			if (refresh)
			{
				Shoot();
			}
		}

		/// <summary>
		/// Set a manual bounding box and switch the bounding mode to
		/// ViewBoundMode.MANUAL, meaning that any call to updateBounds()
		/// will update view bounds to the current bounds.
		/// </summary>
		/// <remarks>The camero.shoot is not called in this case</remarks>
		public BoundingBox3d BoundManual
		{
			set
			{
				_viewbounds = value;
				_boundmode = ViewBoundMode.MANUAL;
				LookToBox(_viewbounds);
			}
		}

		/// <summary>
		/// <para>
		/// Return a 3d scaling factor that allows scaling the scene into a square
		/// box, according to the current ViewBoundMode.
		/// <p/>
		/// If the scene bounds are Infinite, NaN or zero, for a given dimension, the
		/// scaler will be set to 1 on the given dimension.
		/// </para>
		/// <para>@return a scaling factor for each dimension.</para>
		/// </summary>
		internal Coord3d Squarify()
		{
			// Get the view bounds
			BoundingBox3d bounds;
			switch (_boundmode)
			{
				case ViewBoundMode.AUTO_FIT:
					bounds = Scene.Graph.Bounds;
					break;
				case ViewBoundMode.MANUAL:
					bounds = _viewbounds;
					break;
				default:
					throw new Exception("Unsupported bound mode : " + _boundmode);
			}

			// Compute factors
			float xLen = (float)(bounds.XMax - bounds.XMin);
			float yLen = (float)(bounds.YMax - bounds.YMin);
			float zLen = (float)(bounds.ZMax - bounds.ZMin);
			float lmax = MathF.Max(MathF.Max(xLen, yLen), zLen);
			if (float.IsInfinity(xLen) || float.IsNaN(xLen) || xLen == 0)
			{
				xLen = 1;
				// throw new ArithmeticException("x scale is infinite, nan or 0");
			}

			if (float.IsInfinity(yLen) || float.IsNaN(yLen) || yLen == 0)
			{
				yLen = 1;
				// throw new ArithmeticException("y scale is infinite, nan or 0");
			}

			if (float.IsInfinity(zLen) || float.IsNaN(zLen) || zLen == 0)
			{
				zLen = 1;
				// throw new ArithmeticException("z scale is infinite, nan or 0");
			}

			if (float.IsInfinity(lmax) || float.IsNaN(lmax) || lmax == 0)
			{
				lmax = 1;
				// throw new ArithmeticException("lmax is infinite, nan or 0");
			}

			return new Coord3d(lmax / xLen, lmax / yLen, lmax / zLen);
		}
		#endregion

		#region "GL2"
		/// <summary>
		/// The init function specifies general GL settings that impact the rendering
		/// quality and performance (computation speed).
		/// <p/>
		/// The rendering settings are set by the Quality instance given in
		/// the constructor parameters.
		/// </summary>
		public void Init()
		{
			InitQuality();
			InitLights();
		}

		public void InitQuality()
		{
			// Activate Depth buffer
			if (_quality.DepthActivated)
			{
				GL.Enable(EnableCap.DepthTest);
				GL.DepthFunc(DepthFunction.Lequal);
			}
			else
			{
				GL.Disable(EnableCap.DepthTest);
			}

			// Blending
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			// on/off is handled by each viewport (camera or image)

			// Activate transparency
			if (_quality.AlphaActivated)
			{
				GL.Enable(EnableCap.AlphaTest);
				if (_quality.DisableDepthBufferWhenAlpha)
				{
					GL.Disable(EnableCap.DepthTest);
				}
			}
			else
			{
				GL.Disable(EnableCap.AlphaTest);
			}

			// Make smooth colors for polygons (interpolate color between points)
			if (_quality.SmoothColor)
			{
				GL.ShadeModel(ShadingModel.Smooth);
			}
			else
			{
				GL.ShadeModel(ShadingModel.Flat);
			}

			// Make smoothing setting
			if (_quality.SmoothLine)
			{
				GL.Enable(EnableCap.LineSmooth);
				GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
			}
			else
			{
				GL.Disable(EnableCap.LineSmooth);
			}

			if (_quality.SmoothPoint)
			{
				GL.Enable(EnableCap.PointSmooth);
				GL.Hint(HintTarget.PointSmoothHint, HintMode.Fastest);
			}
			else
			{
				GL.Disable(EnableCap.PointSmooth);
			}
		}

		public void InitLights()
		{
			// Init light
			Scene.LightSet.Init();
			Scene.LightSet.Enable();
		}

		// Clear color and depth buffer (same as ClearColorAndDepth)
		public void Clear()
		{
			ClearColorAndDepth();
		}

		// Clear color and depth buffer (same as Clear)
		public void ClearColorAndDepth()
		{
			GL.ClearColor((float)_bgColor.R, (float)_bgColor.G, (float)_bgColor.B, (float)_bgColor.A);
			// clear with background
			GL.ClearDepth(1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public virtual void Render()
		{
			RenderBackground(0, 1);
			RenderScene();
			RenderOverlay();
			if (_dimensionDirty)
			{
				_dimensionDirty = false;
			}
		}

		public void RenderBackground(float left, float right)
		{
			//if ((_bgImg != null)) {
			//	_bgViewport.SetViewPort(_canvas.RendererWidth, _canvas.RendererHeight, left, right);
			//	_bgViewport.Render();
			//}
		}

		public void RenderBackground(ViewPort viewport)
		{
			//if ((_bgImg != null)) {
			//	_bgViewport.SetViewPort(viewport);
			//	_bgViewport.Render();
			//}
		}

		public void RenderScene()
		{
			RenderScene(new ViewPort(_canvas.RendererWidth, _canvas.RendererHeight));
		}

		public void RenderScene(float left, float right)
		{
			RenderScene(new ViewPort(_canvas.RendererWidth, _canvas.RendererHeight, (int)left, (int)right));
		}

		public void RenderScene(ViewPort viewport)
		{
			UpdateQuality();
			UpdateCamera(viewport, ComputeScaling());
			RenderAxeBox();
			RenderSceneGraph();
		}

		public void UpdateQuality()
		{
			if (_quality.AlphaActivated)
			{
				GL.Enable(EnableCap.Blend);
			}
			else
			{
				GL.Disable(EnableCap.Blend);
			}
		}

		public BoundingBox3d ComputeScaling()
		{
			//-- Scale the scene's view -------------------
			if (Squared)
			{
				_scaling = Squarify();
			}
			else
			{
				_scaling = (Coord3d)Coord3d.IDENTITY.Clone();
			}

			// Compute the bounds for computing cam distance, clipping planes, etc...
			if (_targetBox == null)
			{
				_targetBox = new BoundingBox3d(0, 1, 0, 1, 0, 1);
			}

			var boundsScaled = new BoundingBox3d();
			boundsScaled.Add(_targetBox.Scale(_scaling));
			if (MAINTAIN_ALL_OBJECTS_IN_VIEW)
			{
				boundsScaled.Add(Scene.Graph.Bounds.Scale(_scaling));
			}
			return boundsScaled;
		}

		public void UpdateCamera(ViewPort viewport, BoundingBox3d boundsScaled)
		{
			UpdateCamera(viewport, boundsScaled, (float)boundsScaled.GetRadius());
		}

		public void UpdateCamera(ViewPort viewport, BoundingBox3d boundsScaled, float sceneRadiusScaled)
		{
			Coord3d target = _center.Multiply(_scaling);
			_viewpoint.Z = sceneRadiusScaled * 2;

			Coord3d eye;
			// maintain a reasonnable distance to the scene for viewing it
			switch (_viewmode)
			{
				case ViewPositionMode.FREE:
					eye = _viewpoint.Cartesian().Add(target);
					break;
				case ViewPositionMode.TOP:
					eye = _viewpoint;
					eye.X = -PI_div2;
					// on x
					eye.Y = PI_div2;
					// on top
					eye = eye.Cartesian().Add(target);
					break;
				case ViewPositionMode.PROFILE:
					eye = _viewpoint;
					eye.Y = 0;
					eye = eye.Cartesian().Add(target);
					break;
				default:
					throw new Exception("Unsupported viewmode : " + _viewmode);
			}

			Coord3d up;
			if (MathF.Abs(_viewpoint.Y) == PI_div2)
			{
				// handle up vector
				Coord2d direction = new Coord2d(_viewpoint.X, _viewpoint.Y).Cartesian();
				if (_viewpoint.Y > 0)
				{
					// on top
					up = new Coord3d(-direction.X, -direction.Y, 0);
				}
				else
				{
					up = new Coord3d(direction.X, direction.Y, 0);
				}

				// handle "on-top" events
				if (!_wasOnTopAtLastRendering)
				{
					_wasOnTopAtLastRendering = true;
					FireViewOnTopEvent(true);
				}
			}
			else
			{
				// handle up vector
				up = new Coord3d(0, 0, 1);
				// handle "on-top" events
				if (_wasOnTopAtLastRendering)
				{
					_wasOnTopAtLastRendering = false;
					FireViewOnTopEvent(false);
				}
			}

			// Apply camera settings
			_cam.Target = target;
			_cam.Up = up;
			_cam.Eye = eye;

			// Set rendering volume
			if (_viewmode == ViewPositionMode.TOP)
			{
				_cam.RenderingSphereRadius = (MathF.Max(boundsScaled.XMax - boundsScaled.XMin, boundsScaled.YMax - boundsScaled.YMin) / 2);
				// correctCameraPositionForIncludingTextLabels(viewport) ' quite experimental !
			}
			else
			{
				_cam.RenderingSphereRadius = sceneRadiusScaled;
			}

			// Setup camera (i.e. projection matrix)
			//cam.setViewPort(canvas.getRendererWidth(),
			// canvas.getRendererHeight(), left, right);
			_cam.SetViewPort(viewport);
			_cam.Shoot(_cameraMode);
		}

		public void RenderAxeBox()
		{
			if (_axeBoxDisplayed)
			{
				GL.MatrixMode(MatrixMode.Modelview);
				_scene.LightSet.Disable();
				_axe.SetScale(_scaling);
				_axe.Draw(_cam);

				// for debug
				if (DISPLAY_AXE_WHOLE_BOUNDS)
				{
					var abox = (AxeBox)_axe;
					BoundingBox3d box = abox.WholeBounds;
                    var p = new Parallelepiped(box)
                    {
                        FaceDisplayed = false,
                        WireframeColor = Color.MAGENTA,
                        WireframeDisplayed = true
                    };
                    p.Draw(_cam);
				}
				_scene.LightSet.Enable();
			}
		}

		public void RenderSceneGraph()
		{
			RenderSceneGraph(true);
		}

		public void RenderSceneGraph(bool light)
		{
			if (light)
			{
				Scene.LightSet.apply(_scaling);
				// gl.glEnable(GL2.GL_LIGHTING);
				// gl.glEnable(GL2.GL_LIGHT0);
				// gl.glDisable(GL2.GL_LIGHTING);
			}

			Scene.Graph.Transform = new Transform.Transform(new Transform.Scale(_scaling));
			Scene.Graph.Draw(_cam);
		}

		public void RenderOverlay()
		{
			RenderOverlay(new ViewPort(0, 0, _canvas.RendererWidth, _canvas.RendererHeight));
		}

		/// <summary>
		/// <para>
		/// Renders all provided Tooltips and Renderer2ds on top of
		/// the scene.
		/// </para>
		/// <para>
		/// Due to the behaviour of the Overlay implementation, Java2d
		/// geometries must be drawn relative to the Chart's
		/// IScreenCanvas, BUT will then be stretched to fit in the
		/// Camera's viewport. This bug is very important to consider, since
		/// the Camera's viewport may not occupy the full IScreenCanvas.
		/// Indeed, when View is not maximized (like the default behaviour), the
		/// viewport remains square and centered in the canvas, meaning the Overlay
		/// won't cover the full canvas area.
		/// </para>
		/// <para>
		/// In other words, the following piece of code draws a border around the
		/// View, and not around the complete chart canvas, although queried
		/// to occupy chart canvas dimensions:
		/// </para>
		/// <para>
		/// g2d.drawRect(1, 1, chart.getCanvas().getRendererWidth()-2,
		/// chart.getCanvas().getRendererHeight()-2);
		/// </para>
		/// <para>
		/// renderOverlay() must be called while the OpenGL2 context for the
		/// drawable is current, and after the OpenGL2 scene has been rendered.
		/// </para>
		/// </summary>
		/// <param name="viewport"></param>
		public void RenderOverlay(ViewPort viewport)
		{
			// NOT Implemented so far
		}

		internal void CorrectCameraPositionForIncludingTextLabels(ViewPort viewport)
		{
			_cam.SetViewPort(viewport);
			_cam.Shoot(_cameraMode);
			_axe.Draw(_cam);
			Clear();
			AxeBox abox = (AxeBox)_axe;
			BoundingBox3d newBounds = abox.WholeBounds.Scale(_scaling);

			if (_viewmode == ViewPositionMode.TOP)
			{
				float radius = MathF.Max(newBounds.XMax - newBounds.XMin, newBounds.YMax - newBounds.YMin);
				radius += radius * STRETCH_RATIO;
				_cam.RenderingSphereRadius = radius;
			}
			else
			{
				_cam.RenderingSphereRadius = (float)newBounds.GetRadius();
				Coord3d target = newBounds.GetCenter();
				Coord3d eye = _viewpoint.Cartesian().Add(target);
				_cam.Target = target;
				_cam.Eye = eye;
			}
		}
		#endregion
	}
}
