namespace Nzy3d.Maths.Algorithms.Interpolation.Bernstein
{
	public sealed class BernsteinInterpolator : IInterpolator
	{
		public List<Coord3d> Interpolate(List<Coord3d> controlpoints, int resolution)
		{
			Spline3D spline = new Spline3D(controlpoints);
			return spline.ComputeVertices(resolution);
		}
	}
}
