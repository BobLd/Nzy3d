using Nzy3d.Events;
using Nzy3d.Events.Keyboard;
using Nzy3d.Events.Mouse;
using Nzy3d.Plot3D.Rendering.Canvas;
using OpenTK.Graphics.OpenGL;
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
		internal int _width;
		internal int _height;
		internal bool _doScreenshotAtNextDisplay = false;
		internal bool _traceGL;
		internal bool _debugGL;

		internal Bitmap _image;

		public bool ForceUpdate { get; set; }

		public string GetGpuInfo()
		{
			return $"{GL.GetString(StringName.Renderer)} - {GL.GetString(StringName.Vendor)}";
		}

		private void Renderer3D_Paint(object? sender, PaintEventArgs e)
		{
			if (_view != null)
			{
				//System.Diagnostics.Debug.WriteLine($"Renderer3D.Renderer3D_Paint: {this.Name}");
				this.MakeCurrent();

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

		private void Renderer3D_Resize(object? sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"Renderer3D.Renderer3D_Resize: {this.Name}");
			this.MakeCurrent();

			_width = this.ClientSize.Width;
			_height = this.ClientSize.Height;

			if (_view != null)
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
            const OpenTK.Graphics.OpenGL.PixelFormat pxFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
            const PixelType pxType = PixelType.UnsignedByte;

			GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, pxFormat, pxType, data.Scan0);
			_image.UnlockBits(data);
			_image.RotateFlip(RotateFlipType.RotateNoneFlipY);
		}

		public void NextDisplayUpdateScreenshot()
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

		public void AddKeyListener(IBaseKeyListener baseListener)
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

		public void AddMouseListener(IBaseMouseListener baseListener)
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

		public void AddMouseMotionListener(IBaseMouseMotionListener baseListener)
		{
			if (baseListener is not IMouseMotionListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseMove += listener.MouseMoved;
		}

		public void AddMouseWheelListener(IBaseMouseWheelListener baseListener)
		{
			if (baseListener is not IMouseWheelListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseWheel += listener.MouseWheelMoved;
		}

		public new void Dispose()
		{
			_image.Dispose();
			_view.Dispose();
			base.Dispose();
		}

		public async void ForceRepaint()
		{
			//System.Diagnostics.Debug.WriteLine($"Renderer3D.ForceRepaint: {this.Name}");

			this.Invalidate();

			if (ForceUpdate)
			{
				if (this.InvokeRequired)
				{
					// https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/calling-synchronous-methods-asynchronously
					this.Invoke(new MethodInvoker(Update));
				}
				else
				{
					this.Update();
				}
			}
        }

		public void RemoveKeyListener(IBaseKeyListener baseListener)
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

		public void RemoveMouseListener(IBaseMouseListener baseListener)
		{
			if (baseListener is not IMouseListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseClick -= listener.MouseClicked;
			MouseDown -= listener.MousePressed;
			MouseUp -= listener.MouseReleased;
		}

		public void RemoveMouseMotionListener(IBaseMouseMotionListener baseListener)
		{
			if (baseListener is not IMouseMotionListener listener)
			{
				throw new ArgumentException("", nameof(baseListener));
			}

			MouseMove -= listener.MouseMoved;
			// NOT AVAILABLE IN WinForms : RemoveHandler ???, AddressOf listener.MouseDragged
		}

		public void RemoveMouseWheelListener(IBaseMouseWheelListener baseListener)
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

		public void SetView(BaseView value)
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

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			var proj = View?.ProjectMouseInAxes(e.X, Height - e.Y);
			if (proj != null)
			{
				System.Diagnostics.Debug.WriteLine($"Renderer3D.OnMouseMove: {this.Name} - Location={e.Location}, Projection={proj}");
				this.View.MouseCoord3d = proj;
				this.ForceRepaint();
			}

			base.OnMouseMove(e);
        }
    }
}
