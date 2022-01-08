using Nzy3d.Events.Keyboard;
using Nzy3d.Events.Mouse;

namespace Nzy3d.Plot3D.Rendering.Canvas
{
	public interface ICanvas
	{
		/// <summary>
		/// Returns a reference to the held view.
		/// </summary>
		View.View View { get; }

		/// <summary>
		/// Returns the renderer's width, i.e. the display width.
		/// </summary>
		int RendererWidth { get; }

		/// <summary>
		/// Returns the renderer's height, i.e. the display height.
		/// </summary>
		int RendererHeight { get; }

		/// <summary>
		/// Invoked when a user requires the Canvas to be repainted (e.g. a non 3d layer has changed).
		/// </summary>
		void ForceRepaint();

		/// <summary>
		/// Returns an image with the current renderer's size.
		/// </summary>
		//System.Drawing.Bitmap Screenshot();
		object Screenshot();

		/// <summary>
		/// Performs all required cleanup when destroying a Canvas.
		/// </summary>
		void Dispose();
		void addMouseListener(IBaseMouseListener listener);
		void removeMouseListener(IBaseMouseListener listener);
		void addMouseWheelListener(IBaseMouseWheelListener listener);
		void removeMouseWheelListener(IBaseMouseWheelListener listener);
		void addMouseMotionListener(IBaseMouseMotionListener listener);
		void removeMouseMotionListener(IBaseMouseMotionListener listener);
		void addKeyListener(IBaseKeyListener listener);

		void removeKeyListener(IBaseKeyListener listener);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================