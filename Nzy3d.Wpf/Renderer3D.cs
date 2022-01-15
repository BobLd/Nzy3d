using Nzy3d.Events;
using Nzy3d.Events.Keyboard;
using Nzy3d.Events.Mouse;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View;
using Nzy3d.Wpf.Events.Keyboard;
using Nzy3d.Wpf.Events.Mouse;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using System;

namespace Nzy3d.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class Renderer3D : GLWpfControl, ICanvas, IControllerEventListener
    {
        internal View _view;
        internal int _width;
        internal int _height;
        internal bool _doScreenshotAtNextDisplay = false;
        internal bool _traceGL;
        internal bool _debugGL;

        //internal Bitmap _image;

        //public bool ForceUpdate { get; set; }

        public string GetGpuInfo()
        {
            return $"{GL.GetString(StringName.Renderer)} - {GL.GetString(StringName.Vendor)}";
        }

        private void Renderer3D_Render(TimeSpan obj)
        {
            if (_view != null)
            {
                //System.Diagnostics.Debug.WriteLine($"Renderer3D.Renderer3D_Paint: {this.Name}");
                //this.MakeCurrent();

                _view.Clear();
                _view.Render();

                //this.SwapBuffers();
                if (_doScreenshotAtNextDisplay)
                {
                    //GrabScreenshot2();
                    _doScreenshotAtNextDisplay = false;
                }
            }
        }

        private void Renderer3D_Resize(object? sender, System.Windows.SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Renderer3D.Renderer3D_Resize: {this.Name}");
            //this.MakeCurrent();

            _width = (int)this.Width; //.ClientSize.Width;
            _height = (int)this.Height; //.ClientSize.Height;

            if (_view != null)
            {
                _view.DimensionDirty = true;
            }
        }

        private void GrabScreenshot2()
        {
            /*
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
            */
        }

        #region ICanvas
        public void AddKeyListener(IBaseKeyListener baseListener)
        {
            if (baseListener is not IKeyListener listener)
            {
                throw new ArgumentException("", nameof(baseListener));
            }

            KeyUp += listener.KeyReleased;
            KeyDown += listener.KeyPressed;

            // be cautious with cross-terminology (key_down / key_pressed / key_typed)
            //KeyPress += listener.KeyTyped;
            // be cautious with cross-terminology (key_down / key_pressed / key_typed)
        }

        public void AddMouseListener(IBaseMouseListener baseListener)
        {
            if (baseListener is not IMouseListener listener)
            {
                throw new ArgumentException("", nameof(baseListener));
            }

            //MouseClick += listener.MouseClicked;
            MouseDown +=  listener.MousePressed;
            MouseUp += listener.MouseReleased;
            //MouseDoubleClick += listener.MouseDoubleClicked;
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

        public void Dispose()
        {
            //_image.Dispose();
            _view.Dispose();
            //base.Dispose();
        }

        public void ForceRepaint()
        {
            this.Dispatcher.Invoke(() => this.InvalidateVisual());
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
            //KeyPress -= listener.KeyTyped;
            // be cautious with cross-terminology (key_down / key_pressed / key_typed)
        }

        public void RemoveMouseListener(IBaseMouseListener baseListener)
        {
            if (baseListener is not IMouseListener listener)
            {
                throw new ArgumentException("", nameof(baseListener));
            }

            //MouseClick -= listener.MouseClicked;
            MouseDown -= listener.MousePressed;
            MouseUp -= listener.MouseReleased;
        }

        public void RemoveMouseMotionListener(IBaseMouseMotionListener baseListener)
        {
            if (baseListener is not IMouseMotionListener listener)
            {
                throw new ArgumentException("", nameof(baseListener));
            }

            MouseMove -= (s, e) => listener.MouseMoved(s, e);
        }

        public void RemoveMouseWheelListener(IBaseMouseWheelListener baseListener)
        {
            if (baseListener is not IMouseWheelListener listener)
            {
                throw new ArgumentException("", nameof(baseListener));
            }

            MouseWheel -= listener.MouseWheelMoved;
        }

        public object Screenshot() // Bitmap
        {
            throw new NotImplementedException();
            //this.GrabScreenshot2();
            //return _image;
        }

        public void SetView(View value)
        {
            _view = value;
            _view.Init();
            _view.Scene.Graph.MountAllGLBindedResources();
            _view.BoundManual = _view.Scene.Graph.Bounds;
        }

        public int RendererWidth => (int)base.ActualWidth;

        public int RendererHeight => (int)base.ActualHeight;

        public View View => _view;
        #endregion

        #region IControllerEventListener
        public void ControllerEventFired(ControllerEventArgs e)
        {
            this.ForceRepaint();
        }
        #endregion

        public Renderer3D()
        {
            SizeChanged += Renderer3D_Resize;
            Render += Renderer3D_Render;
        }
    }
}
