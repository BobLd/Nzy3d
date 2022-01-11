using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Primitives.Axes
{
	public interface IAxe
	{
		void Dispose();

		void SetAxe(BoundingBox3d box);

		void Draw(Camera camera);

		void SetScale(Coord3d scale);

		BoundingBox3d GetBoxBounds();

		BoundingBox3d GetWholeBounds();

		Coord3d GetCenter();

		IAxeLayout GetLayout();
	}
}
