using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Builder
{
	public interface IObjectTopology<O>
	{
		Coord3d getCoord(O obj);
		string getXAxisLabel();
		string getYAxisLabel();
		string getZAxisLabel();
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
