using Nzy3d.Maths;
using Range = Nzy3d.Maths.Range;

namespace Nzy3d.Plot3D.Builder.Concrete
{
    public class RingGrid : Grid
	{
		internal double sqradius;
		public RingGrid(float radius, int xysteps, int enlargeSteps) : base(new Range(-radius - (enlargeSteps * radius / xysteps), radius + (enlargeSteps * radius / xysteps)), xysteps)
		{
			sqradius = (radius + (enlargeSteps * radius / xysteps)) * (radius + (enlargeSteps * radius / xysteps));
		}

		public RingGrid(float radius, int xysteps) : this(radius, xysteps, 0)
		{
		}

		public override List<Coord3d> Apply(Mapper mapper)
		{
			float xstep = xrange.Range / xsteps;
			float ystep = yrange.Range / ysteps;
			var output = new List<Coord3d>();

			for (int xi = -(xsteps - 1) / 2; xi <= (xsteps - 1) / 2; xi++)
			{
				for (int yi = -(ysteps - 1) / 2; yi <= (ysteps - 1) / 2; yi++)
				{
					float x = xi * xstep;
					float y = yi * ystep;
					if (sqradius > x * x + y * y)
					{
						output.Add(new Coord3d(x, y, mapper.f(x, y)));
					}
				}
			}

			return output;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
