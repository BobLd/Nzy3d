using Nzy3d.Maths;
using Range = Nzy3d.Maths.Range;

namespace Nzy3d.Plot3D.Builder
{
	public abstract class Grid
	{
		protected internal Range XRange;
		protected internal Range YRange;
		protected internal int XSteps;
		protected internal int YSteps;

		public Grid(Range xrange, int xsteps, Range yrange, int ysteps)
		{
			this.XRange = xrange;
			this.YRange = yrange;
			this.XSteps = xsteps;
			this.YSteps = ysteps;
		}

		public Grid(Range xyrange, int xysteps) : this(xyrange, xysteps, xyrange, xysteps)
		{
		}

		public abstract List<Coord3d> Apply(Mapper mapper);
	}
}
