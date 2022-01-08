namespace Nzy3d.Plot3D.Builder.Delaunay
{
	public interface ICoordinateValidator
	{
		float[] GetX();
		float[] GetY();
		float[,] Get_Z_as_fxy();
	}
}
