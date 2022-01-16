using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.Canvas;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Chart
{
	public class ChartScene : Scene
	{
		internal int _nview;

		internal View _view;
		public ChartScene(bool graphsort) : base(graphsort)
		{
			_nview = 0;
		}

		public void Clear()
		{
			_view.BoundManual = new BoundingBox3d(0, 0, 0, 0, 0, 0);
		}

		public override View NewView(ICanvas canvas, Quality quality)
		{
			if (_nview > 0)
			{
				throw new Exception("A view has already been defined for this scene. Can not use several views.");
			}
			_nview++;
			_view = base.NewView(canvas, quality);
			return _view;
		}

		public override void ClearView(View view)
		{
			base.ClearView(view);
			_nview = 0;
		}
	}
}
