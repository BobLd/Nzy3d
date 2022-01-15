using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using Nzy3d.Plot3D.Rendering.View;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Primitives.Axes
{
	/// <summary>
	/// An AxeBase provide a simple 3-segment object which is configured by
	/// a BoundingBox.
	/// @author Martin Pernollet
	/// </summary>
	public class AxeBase : IAxe
	{
		internal Coord3d _scale;
		internal BoundingBox3d _bbox;
		internal IAxeLayout _layout;

		/// <summary>
		/// Create a simple axe centered on (0,0,0), with a dimension of 1.
		/// </summary>
		public AxeBase() : this(new BoundingBox3d(0, 1, 0, 1, 0, 1))
		{
		}

		/// <summary>
		/// Create a simple axe centered on (box.xmin, box.ymin, box.zmin)
		/// </summary>
		public AxeBase(BoundingBox3d box)
		{
			SetAxe(box);
			SetScale(new Coord3d(1, 1, 1));
		}

		public void Dispose()
		{
		}

		public void Draw(Camera camera)
		{
			GL.LoadIdentity();
			GL.Scale(_scale.X, _scale.Y, _scale.Z);
			GL.LineWidth(2);
			GL.Begin(PrimitiveType.Lines);

			// R
			GL.Color4(1f, 0f, 0f, 1f);
			GL.Vertex3(_bbox.XMin, _bbox.YMin, _bbox.ZMin);
			GL.Vertex3(_bbox.XMax, _bbox.YMin, _bbox.ZMin);

			// G
			GL.Color4(0f, 1f, 0f, 1f);
			GL.Vertex3(_bbox.XMin, _bbox.YMin, _bbox.ZMin);
			GL.Vertex3(_bbox.XMin, _bbox.YMax, _bbox.ZMin);

			// B
			GL.Color4(0f, 0f, 1f, 1f);
			GL.Vertex3(_bbox.XMin, _bbox.YMin, _bbox.ZMin);
			GL.Vertex3(_bbox.XMin, _bbox.YMin, _bbox.ZMax);
			GL.End();
		}

		public BoundingBox3d GetBoxBounds()
		{
			return _bbox;
		}

		public BoundingBox3d GetWholeBounds()
		{
			return GetBoxBounds().MarginRatio(0.02);
		}

		public Coord3d GetCenter()
		{
			return new Coord3d(_bbox.XMin, _bbox.YMin, _bbox.ZMin);
		}

		public IAxeLayout Layout
		{
			get { return _layout; }
		}

		public void SetAxe(BoundingBox3d box)
		{
			_bbox = box;
		}

		public void SetScale(Coord3d scale)
		{
			_scale = scale;
		}
	}
}
