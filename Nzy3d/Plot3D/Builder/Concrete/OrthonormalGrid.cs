using Nzy3d.Maths;
using Range = Nzy3d.Maths.Range;

namespace Nzy3d.Plot3D.Builder.Concrete
{
	public class OrthonormalGrid : Grid
	{
		public OrthonormalGrid(Range xrange, int xsteps, Range yrange, int ysteps) : base(xrange, xsteps, yrange, ysteps)
		{
		}

		public OrthonormalGrid(Range xyrange, int xysteps) : base(xyrange, xysteps)
		{
		}

		public override List<Coord3d> Apply(Mapper mapper)
		{
			float xstep = xrange.Range / (xsteps - 1);
			float ystep = yrange.Range / (ysteps - 1);
			var output = new List<Coord3d>();

			for (int xi = 0; xi <= xsteps - 1; xi++)
			{
				for (int yi = 0; yi <= ysteps - 1; yi++)
				{
					float x = xrange.Min + xi * xstep;
					float y = yrange.Min + yi * ystep;
                    output.Add(new Coord3d(x, y, mapper.f(x, y)));
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
