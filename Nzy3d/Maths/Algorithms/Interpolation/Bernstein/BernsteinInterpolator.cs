namespace Nzy3d.Maths.Algorithms.Interpolation.Bernstein
{
	public class BernsteinInterpolator : IInterpolator
	{
		public List<Coord3d> Interpolate(List<Coord3d> controlpoints, int resolution)
		{
			Spline3D spline = new Spline3D(controlpoints);
			return spline.ComputeVertices(resolution);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
