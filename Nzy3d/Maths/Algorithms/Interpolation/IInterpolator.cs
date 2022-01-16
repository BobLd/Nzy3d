namespace Nzy3d.Maths.Algorithms.Interpolation
{
	public interface IInterpolator
	{
		List<Coord3d> Interpolate(List<Coord3d> controlpoints, int resolution);
	}
}
