using Nzy3d.Maths;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Primitives
{
	public interface ISelectable
	{
		void Project(Camera cam);

		List<Coord3d> LastProjection { get; }
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
