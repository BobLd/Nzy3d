using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder
{
	public abstract class Tessellator
	{
		public Tessellator()
		{
		}

		public AbstractComposite Build(List<Coord3d> coordinates)
		{
			Coordinates coords = new Coordinates(coordinates);
			return Build(coords.X, coords.Y, coords.Z);
		}

		public abstract AbstractComposite Build(float[] x, float[] y, float[] z);

		public AbstractComposite Build(double[] x, double[] y, double[] z)
		{
			float[] xs = new float[x.Length];
			System.Array.Copy(x, xs, x.Length);
			float[] ys = new float[y.Length];
			System.Array.Copy(y, ys, y.Length);
			float[] zs = new float[z.Length];
			System.Array.Copy(z, zs, z.Length);
			return Build(xs, ys, zs);
		}
	}
}
