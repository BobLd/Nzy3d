using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Builder.Concrete
{
	public class SphereScatterGenerator
	{
		public static object Generate(Coord3d center, float radius, int steps, bool half)
		{
			var coords = new List<Coord3d>();
			float inc = MathF.PI / steps;
			float i = 0;
			int jrat = (half ? 1 : 2);
			while (i < (2 * MathF.PI))
			{
				float j = 0;
				while (j < (jrat * MathF.PI))
				{
					var c = new Coord3d(i, j, radius).Cartesian();
					if (center != null)
					{
						c.X += center.X;
						c.Y += center.Y;
						c.Z += center.Z;
					}
					coords.Add(c);
					j += inc;
				}
				i += inc;
			}
			return coords;
		}

		public static object Generate(Coord3d center, float radius, int steps)
		{
			return Generate(center, radius, steps, false);
		}

		public static object Generate(float radius, int steps)
		{
			return Generate(null, radius, steps, false);
		}
	}
}
