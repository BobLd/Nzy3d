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
		public static Shape buildOrthonomal(OrthonormalGrid grid, Mapper mapper)
		{
			var tesselator = new OrthonormalTessellator();
			return (Shape)tesselator.build(grid.Apply(mapper));
		}

		public static Shape buildRing(OrthonormalGrid grid, Mapper mapper, float ringMin, float ringMax)
		{
			var tesselator = new RingTessellator(ringMin, ringMax, new ColorMapper(new ColorMapRainbow(), 0, 1), Color.BLACK);
			return (Shape)tesselator.build(grid.Apply(mapper));
		}

		public static Shape buildRing(OrthonormalGrid grid, Mapper mapper, float ringMin, float ringMax, ColorMapper cmap, Color factor)
		{
			var tesselator = new RingTessellator(ringMin, ringMax, cmap, factor);
			return (Shape)tesselator.build(grid.Apply(mapper));
		}

		public static Shape buildDelaunay(List<Coord3d> coordinates)
		{
			var tesselator = new DelaunayTessellator();
			return (Shape)tesselator.build(coordinates);
		}

		// BIG SURFACE
		public static CompileableComposite buildOrthonormalBig(OrthonormalGrid grid, Mapper mapper)
		{
			var tesselator = new OrthonormalTessellator();
			Shape s1 = (Shape)tesselator.build(grid.Apply(mapper));
			return buildComposite(applyStyling(s1));
		}

		public static Shape applyStyling(Shape s)
		{
			s.ColorMapper = new ColorMapper(_colorMap, s.Bounds.zmin, s.Bounds.zmax);
			s.FaceDisplayed = _faceDisplayed;
			s.WireframeDisplayed = _wireframeDisplayed;
			s.WireframeColor = _wireframeColor;
			return s;
		}

		public static CompileableComposite buildComposite(Shape s)
		{
			var sls = new CompileableComposite();
			sls.Add(s.GetDrawables);
			sls.ColorMapper = new ColorMapper(_colorMap, sls.Bounds.zmin, sls.Bounds.zmax, _colorFactor);
			sls.FaceDisplayed = s.FaceDisplayed;
			sls.WireframeDisplayed = s.WireframeDisplayed;
			sls.WireframeColor = s.WireframeColor;
			return sls;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
