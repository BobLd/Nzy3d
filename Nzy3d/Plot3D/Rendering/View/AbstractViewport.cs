using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Rendering.View
{
	/// <summary>
	/// <para>
	/// A <see cref="AbstractViewport"/> describes an element that occupies the whole
	/// rendering <see cref="ICanvas"/> or only a vertical slice of it.
	/// </para>
	/// <para>
	/// The {@link AbstractViewport} also provides a utility function for debugging the slices, that is
	/// the ability to display a 10*10 grid for checking the space occupied by the actual
	/// viewport definition.
	/// </para>
	/// <para>@author Martin Pernollet</para>
	/// </summary>
	public abstract class AbstractViewport
	{
		private const float AREA_LEFT = -100;
		private const float AREA_RIGHT = +100;
		private const float AREA_TOP = +100;
		private const float AREA_DOWN = -100;
		private const float GRID_STEPS = 10;
		private const float OFFSET = 0.1f;

		internal int _screenLeft;
		internal int _screenRight;
		internal int _screenXOffset;
		internal int _screenYOffset;
		internal int _screenWidth;
		internal int _screenHeight;
		internal int _screenSquaredDim;
		internal bool _screenGridDisplayed;
		internal bool _stretchToFill;
		internal float _ratioWidth;

		internal float _ratioHeight;

		internal ViewPort _lastViewPort;
		/// <summary>
		/// Set the status of the stretching mode (activated or not). Stretching consist in
		/// displaying the projection into the full screen slice (i.e. viewport).
		/// </summary>
		public bool StretchToFill
		{
			get { return _stretchToFill; }
			set { _stretchToFill = value; }
		}

		/// <summary>
		/// Set the view port (size of the renderer).
		/// </summary>
		/// <param name="width">the width of the target window</param>
		/// <param name="height">the height of the target window</param>
		public void SetViewPort(int width, int height)
		{
			SetViewPort(width, height, 0, 1);
		}

		/// <summary>
		/// Set the view port (size of the renderer).
		/// </summary>
		/// <param name="width">the width of the target window</param>
		/// <param name="height">the height of the target window</param>
		/// <param name="left">the width's ratio where this subscreen starts in the target window</param>
		/// <param name="right">the width's ratio where this subscreen stops in the target window</param>
		public virtual void SetViewPort(int width, int height, float left, float right)
		{
			if (left >= right)
			{
				throw new ArgumentException("left must be inferior to right");
			}
			_screenWidth = Convert.ToInt32((right - left) * width);
			_screenHeight = height;
			_screenLeft = Convert.ToInt32(left * width);
			_screenRight = _screenLeft + _screenWidth;
		}

		/// <summary>
		/// Set the view port (size of the renderer).
		/// </summary>
		/// <param name="viewport">Viewport</param>
		public virtual void SetViewPort(ViewPort viewport)
		{
			_screenWidth = viewport.Width;
			_screenHeight = viewport.Height;
			_screenLeft = viewport.X;
			_screenRight = viewport.Y;
		}

		public ViewPort LastViewPort
		{
			get { return _lastViewPort; }
		}

		internal void ApplyViewPort()
		{
			//Stretch projection on the whole viewport
			if (_stretchToFill)
			{
				_screenXOffset = _screenLeft;
				_screenYOffset = 0;
				GL.Viewport(_screenXOffset, _screenYOffset, _screenWidth, _screenHeight);
				_lastViewPort = new ViewPort(_screenWidth, _screenHeight, _screenXOffset, _screenYOffset);
				//Set the projection into the largest square area centered in the window slice
			}
			else
			{
				_screenSquaredDim = Math.Min(_screenWidth, _screenHeight);
				_screenXOffset = _screenLeft + _screenWidth / 2 - _screenSquaredDim / 2;
				_screenYOffset = _screenHeight / 2 - _screenSquaredDim / 2;
				GL.Viewport(_screenXOffset, _screenYOffset, _screenSquaredDim, _screenSquaredDim);
				_lastViewPort = new ViewPort(_screenSquaredDim, _screenSquaredDim, _screenXOffset, _screenYOffset);
			}
			if (_screenGridDisplayed)
			{
				RenderSubScreenGrid();
			}
		}

		public System.Drawing.Rectangle Rectange
		{
			get
			{
				if (_stretchToFill)
				{
					return new System.Drawing.Rectangle(_screenXOffset, _screenYOffset, _screenWidth, _screenHeight);
				}
				else
				{
					return new System.Drawing.Rectangle(_screenXOffset, _screenYOffset, _screenSquaredDim, _screenSquaredDim);
				}
			}
		}

		public bool ScreenGridDisplayed
		{
			set { _screenGridDisplayed = value; }
		}

		private void RenderSubScreenGrid()
		{
			if (_screenWidth <= 0) return;

			// Set a 2d projection
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			if (_stretchToFill)
			{
				GL.Viewport(_screenLeft, 0, _screenWidth, _screenHeight);
				//Set the projection into the largest square area centered in the window slice
			}
			else
			{
				int dimension = Math.Min(_screenWidth, _screenHeight);
				int screenXoffset = _screenLeft + _screenWidth / 2 - dimension / 2;
				int screenYoffset = _screenHeight / 2 - dimension / 2;
				GL.Viewport(screenXoffset, screenYoffset, dimension, dimension);
			}
			GL.Ortho(AREA_LEFT, AREA_RIGHT, AREA_DOWN, AREA_TOP, -1, 1);
			// Set a grid
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.Color3(1, 0.5, 0.5);
			GL.LineWidth(1);
			float sstep = (AREA_RIGHT - AREA_LEFT) / (GRID_STEPS + 0);
			for (float i = AREA_LEFT; i <= AREA_RIGHT; i += sstep)
			{
				float x = i;
				if (x == AREA_LEFT)
				{
					x += OFFSET;
				}
				GL.Begin(PrimitiveType.Lines);
				GL.Vertex3(x, AREA_DOWN, 1);
				GL.Vertex3(x, AREA_TOP, 1);
				GL.End();
			}
			sstep = (AREA_TOP - AREA_DOWN) / (GRID_STEPS + 0);
			for (float j = AREA_DOWN; j <= AREA_TOP; j += sstep)
			{
				float y = j;
				if (y == AREA_TOP)
				{
					y -= OFFSET;
				}
				GL.Begin(PrimitiveType.Lines);
				GL.Vertex3(AREA_LEFT, y, 1);
				GL.Vertex3(AREA_RIGHT, y, 1);
				GL.End();
			}
			// Restore matrices
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}
	}
}
