using Nzy3d.Colors.ColorMaps;
using Nzy3d.Maths;
using Range = Nzy3d.Maths.Range;
using Scale = Nzy3d.Maths.Scale;

namespace Nzy3d.Colors
{
    public class ColorMapper : IColorMappable
	{
        private readonly Color m_factor;

		public ColorMapper(IColorMap colormap, double zmin, double zmax, Color factor)
		{
			ColorMap = colormap;
			ZMin = zmin;
			ZMax = zmax;
			m_factor = factor;
		}

		public ColorMapper(IColorMap colormap, double zmin, double zmax) : this(colormap, zmin, zmax, null)
		{
		}

		public ColorMapper(ColorMapper colormapper, Color factor) : this(colormapper.ColorMap, colormapper.ZMin, colormapper.ZMax, factor)
		{
		}

        public IColorMap ColorMap { get; }

        public Color Color(Coord3d coord)
		{
			Color @out = ColorMap.GetColor(this, coord.X, coord.Y, coord.Z);
			if (m_factor != null)
			{
				@out.Mul(m_factor);
			}
			return @out;
		}

		public Color Color(double v)
		{
			Color @out = ColorMap.GetColor(this, v);
			if (m_factor != null)
			{
				@out.Mul(m_factor);
			}
			return @out;
		}

        public double ZMax { get; set; }

        public double ZMin { get; set; }

        /// <summary>
        /// Range representing zmin/zmax values (same as <see cref="Scale"/> with different object type)
        /// </summary>
        public Range Range
		{
            get { return new Range(ZMin, ZMax); }
			set
			{
				ZMin = value.Min;
				ZMax = value.Max;
			}
		}

		/// <summary>
		/// Scale representing zmin/zmax values (same as <see cref="Range"/> with different object type)
		/// </summary>
		public Scale Scale
		{
			get { return new Scale(ZMin, ZMax); }
			set
			{
				ZMin = value.Min;
				ZMax = value.Max;
			}
		}

		/// <summary>
		/// Returns the string representation of this colormapper
		/// </summary>
		public override string ToString()
		{
			return "(ColorMapper) " + ColorMap.ToString() + " zmin=" + ZMin + " zmax=" + ZMax + " factor=" + m_factor.ToString();
		}
	}
}
