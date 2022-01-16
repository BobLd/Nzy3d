using Nzy3d.Colors;
using Nzy3d.Colors.ColorMaps;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Concrete;
using Nzy3d.Plot3D.Builder.Delaunay;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder
{
    public class Builder
	{
		static internal IColorMap _colorMap = new ColorMapRainbow();
		static internal Color _colorFactor = new Color(1, 1, 1, 1);
		static internal bool _faceDisplayed = true;
		static internal bool _wireframeDisplayed;

		static internal Color _wireframeColor = Color.BLACK;

		public static Shape BuildOrthonomal(OrthonormalGrid grid, Mapper mapper)
		{
			var tesselator = new OrthonormalTessellator();
			return (Shape)tesselator.Build(grid.Apply(mapper));
		}

		public static Shape BuildRing(OrthonormalGrid grid, Mapper mapper, float ringMin, float ringMax)
		{
			var tesselator = new RingTessellator(ringMin, ringMax, new ColorMapper(new ColorMapRainbow(), 0, 1), Color.BLACK);
			return (Shape)tesselator.Build(grid.Apply(mapper));
		}

		public static Shape BuildRing(OrthonormalGrid grid, Mapper mapper, float ringMin, float ringMax, ColorMapper cmap, Color factor)
		{
			var tesselator = new RingTessellator(ringMin, ringMax, cmap, factor);
			return (Shape)tesselator.Build(grid.Apply(mapper));
		}

		public static Shape BuildDelaunay(List<Coord3d> coordinates)
		{
			var tesselator = new DelaunayTessellator();
			return (Shape)tesselator.Build(coordinates);
		}

		// BIG SURFACE
		public static CompileableComposite BuildOrthonormalBig(OrthonormalGrid grid, Mapper mapper)
		{
			var tesselator = new OrthonormalTessellator();
			Shape s1 = (Shape)tesselator.Build(grid.Apply(mapper));
			return BuildComposite(ApplyStyling(s1));
		}

		public static Shape ApplyStyling(Shape s)
		{
			s.ColorMapper = new ColorMapper(_colorMap, s.Bounds.ZMin, s.Bounds.ZMax);
			s.FaceDisplayed = _faceDisplayed;
			s.WireframeDisplayed = _wireframeDisplayed;
			s.WireframeColor = _wireframeColor;
			return s;
		}

		public static CompileableComposite BuildComposite(Shape s)
		{
			var sls = new CompileableComposite();
			sls.Add(s.Drawables);
			sls.ColorMapper = new ColorMapper(_colorMap, sls.Bounds.ZMin, sls.Bounds.ZMax, _colorFactor);
			sls.FaceDisplayed = s.FaceDisplayed;
			sls.WireframeDisplayed = s.WireframeDisplayed;
			sls.WireframeColor = s.WireframeColor;
			return sls;
		}
	}
}
