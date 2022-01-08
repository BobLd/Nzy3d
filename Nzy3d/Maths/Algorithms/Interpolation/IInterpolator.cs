namespace Nzy3d.Maths.Algorithms.Interpolation
{
	public interface IInterpolator
	{
		List<Coord3d> Interpolate(List<Coord3d> controlpoints, int resolution);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
