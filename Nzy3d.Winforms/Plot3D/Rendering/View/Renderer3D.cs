﻿using Nzy3d.Events;
using Nzy3d.Events.Keyboard;
using Nzy3d.Events.Mouse;
using Nzy3d.Plot3D.Rendering.Canvas;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using System.Drawing.Imaging;
using BaseView = Nzy3d.Plot3D.Rendering.View.View;

namespace Nzy3d.Winforms
{
    /// <summary>
    /// 
    /// </summary>
    public class Renderer3D : GLControl, ICanvas, IControllerEventListener
	{
		// TODO  : add trace add debug capabilities
		internal BaseView _view;
		internal int _width = 0;
		internal int _height = 0;
		internal bool _doScreenshotAtNextDisplay = false;
		internal bool _traceGL;
		internal bool _debugGL;

		internal Bitmap _image;
		//Public Sub New(view As View)
		//  Me.New(view, False, False)
		//End Sub

		//Public Sub New(view As View, traceGL As Boolean, debugGL As Boolean)
		//  _view = view
		//  _traceGL = traceGL
		//  _debugGL = debugGL
		//End Sub

		//Private Sub Renderer3D_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		//End Sub

		private void Renderer3D_Paint(object sender, PaintEventArgs e)
		{
			if ((_view != null))
			{
				this.MakeCurrent(); // BobLd
				_view.Clear();
				_view.Render();
				this.SwapBuffers();
				if (_doScreenshotAtNextDisplay)
				{
					GrabScreenshot2();
					_doScreenshotAtNextDisplay = false;
				}
			}
		}

		private void Renderer3D_Resize(object sender, EventArgs e)
		{
			//this.MakeCurrent(); // BobLd

			//https://github.com/opentk/GLControl/blob/master/OpenTK.WinForms.MultiControlTest/Form1.cs
			_width = this.ClientSize.Width;
			_height = this.ClientSize.Height;

			if ((_view != null))
			{
				_view.DimensionDirty = true;
			}
		}

		private void GrabScreenshot2()
		{
			if (_image == null || _image.Width != this.Width || _image.Height != this.Height)
			{
				_image = new Bitmap(ClientSize.Width, ClientSize.Height);
			}

			BitmapData data = _image.LockBits(this.ClientRectangle, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			//OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, OpenTK.Graphics.PixelFormat.Bgr, OpenTK.Graphics.PixelType.UnsignedByte, data.Scan0)
			OpenTK.Graphics.OpenGL.PixelFormat pxFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
			OpenTK.Graphics.OpenGL.PixelType pxType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;

			OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, pxFormat, pxType, data.Scan0);
			_image.UnlockBits(data);
			_image.RotateFlip(RotateFlipType.RotateNoneFlipY);
		}

		public void nextDisplayUpdateScreenshot()
		{
			_doScreenshotAtNextDisplay = true;
		}

		public Bitmap LastScreenshot
		{
			get { return _image; }
		}

		public new int Width
		{
			get { return _width; }
		}

		public new int Height
		{
			get { return _height; }
		}

		public void addKeyListener(IBaseKeyListener baseListener)
		{
			if (baseListener is not IKeyListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			KeyUp += listener.KeyReleased;
			KeyDown += listener.KeyPressed;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
			KeyPress += listener.KeyTyped;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
		}

		public void addMouseListener(IBaseMouseListener baseListener)
		{
			if (baseListener is not IMouseListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseClick += listener.MouseClicked;
			MouseDown += listener.MousePressed;
			MouseUp += listener.MouseReleased;
			MouseDoubleClick += listener.MouseDoubleClicked;
		}

		public void addMouseMotionListener(IBaseMouseMotionListener baseListener)
		{
			if (baseListener is not IMouseMotionListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseMove += listener.MouseMoved;
			// NOT AVAILABLE IN WinForms : AddHandler ???, AddressOf listener.MouseDragged
		}

		public void addMouseWheelListener(IBaseMouseWheelListener baseListener)
		{
			if (baseListener is not IMouseWheelListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseWheel += listener.MouseWheelMoved;
		}

		public void Dispose1()
		{
		}

		void ICanvas.Dispose()
		{
			Dispose1();
		}

		public void ForceRepaint()
		{
			this.Invalidate();
		}

		public void removeKeyListener(IBaseKeyListener baseListener)
		{
			if (baseListener is not IKeyListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			KeyUp -= listener.KeyReleased;
			KeyDown -= listener.KeyPressed;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
			KeyPress -= listener.KeyTyped;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
		}

		public void removeMouseListener(IBaseMouseListener baseListener)
		{
			if (baseListener is not IMouseListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseClick -= listener.MouseClicked;
			MouseDown -= listener.MousePressed;
			MouseUp -= listener.MouseReleased;
		}

		public void removeMouseMotionListener(IBaseMouseMotionListener baseListener)
		{
			if (baseListener is not IMouseMotionListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseMove -= listener.MouseMoved;
			// NOT AVAILABLE IN WinForms : RemoveHandler ???, AddressOf listener.MouseDragged
		}

		public void removeMouseWheelListener(IBaseMouseWheelListener baseListener)
		{
			if (baseListener is not IMouseWheelListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseWheel -= listener.MouseWheelMoved;
		}

		public int RendererHeight
		{
			get { return _height; }
		}

		public int RendererWidth
		{
			get { return _width; }
		}

		public object Screenshot() // Bitmap
		{
			//Throw New NotImplementedException()
			this.GrabScreenshot2();
			return _image;
		}

		public BaseView View
		{
			//Throw New NotImplementedException("Property View is not implemented in nzy3D renderer, should not be necessary")
			get { return _view; }
		}

		public void setView(BaseView value)
		{
			_view = value;
			_view.Init();
			_view.Scene.Graph.MountAllGLBindedResources();
			_view.BoundManual = _view.Scene.Graph.Bounds;
		}

		public void ControllerEventFired(ControllerEventArgs e)
		{
			this.ForceRepaint();
		}
		public Renderer3D()
		{
			Resize += Renderer3D_Resize;
			Paint += Renderer3D_Paint;
		}
	}
}
