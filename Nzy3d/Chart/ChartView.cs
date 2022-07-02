using Nzy3d.Plot3D.Primitives;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.Legends;
using Nzy3d.Plot3D.Rendering.View;
using System.Drawing;

namespace Nzy3d.Chart
{
	/// <summary>
	/// A <see cref="ChartView"/> allows displaying a 3d scene on the left,
	/// and a set of <see cref="AbstractDrawable"/>'s <see cref="Legend"/> on the right.
	/// @author Martin Pernollet
	/// </summary>
	public class ChartView : View
	{
		internal Rectangle _zone1;

		internal Rectangle _zone2;

		/// <summary>
		///
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="canvas"></param>
		/// <param name="quality"></param>
		public ChartView(Scene scene, ICanvas canvas, Quality quality) : base(scene, canvas, quality)
		{
			// display zones
			_zone1 = new Rectangle(0, 0, 0, 0);
			_zone2 = new Rectangle(0, 0, 0, 0);
		}

		/// <summary>
		/// Set the camera held by this view, and draw the scene graph.
		/// Performs all transformations of eye, target coordinates to adapt the camera settings
		/// to the scaled scene.
		/// </summary>
		public override void Render()
		{
			List<Legend> list = _scene.Graph.Legends;
			bool hasMeta = (list.Count > 0);
			// Compute an optimal layout so that we use the minimal area for metadata
			float screenSeparator = 1.0f;
			if (hasMeta)
			{
				int minwidth = 0;
				foreach (Legend data in list)
				{
					minwidth += data.MinimumSize.Width;
				}
				screenSeparator = (float)((_canvas.RendererWidth - minwidth) / _canvas.RendererWidth);
				// 0.7f
			}

			var sceneViewPort = ViewPort.Slice(_canvas.RendererWidth, _canvas.RendererHeight, 0, screenSeparator);
			var backgroundViewPort = new ViewPort(_canvas.RendererWidth, _canvas.RendererHeight);

			RenderBackground(backgroundViewPort);
			RenderScene(sceneViewPort);

			if (hasMeta)
			{
				RenderFaces(screenSeparator, 1);
			}

			// fix overlay on top of chart
			//System.out.println(scenePort);
			RenderOverlay(_cam.LastViewPort);
			//renderOverlay(gl);
			if (_dimensionDirty)
			{
				_dimensionDirty = false;
			}
		}

		internal void RenderFaces(float left, float right)
		{
			List<Legend> data = _scene.Graph.Legends;
			float slice = (right - left) / data.Count;
			int k = 0;
			foreach (Legend layer in data)
			{
				layer.StretchToFill = true;
				layer.SetViewPort(_canvas.RendererWidth, _canvas.RendererHeight, left + slice * k, left + slice * (k + 1));
				// correction par rapport à l'incrémentation des indices
				k++;
				layer.Render();
			}
		}
	}
}
