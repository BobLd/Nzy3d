using Nzy3d.Maths;

namespace Nzy3d.Plot3D.Transform
{
    public interface ITransformer
	{
		// Execute the effective GL transformation held by this class.
		void Execute();

		Coord3d Compute(Coord3d input);
		// Apply the transformations to the input coordinates. (Warning: this method is a utility that may not be implemented.)
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
