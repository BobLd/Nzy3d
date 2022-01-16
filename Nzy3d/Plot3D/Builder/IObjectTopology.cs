using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Builder
{
	public interface IObjectTopology<O>
	{
		Coord3d GetCoord(O obj);

		string GetXAxisLabel();

		string GetYAxisLabel();

		string GetZAxisLabel();
	}
}
