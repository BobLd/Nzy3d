using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Rendering.Ordering
{
    /// <summary>
    /// The <see cref="BarycentreOrderingStrategy"/> compare two <see cref="AbstractDrawable"/>s by computing
    /// their respective distances to the <see cref="Camera"/>, which must be referenced prior to any
    /// comparison.
    ///
    /// @author Martin Pernollet
    /// </summary>
    /// <remarks></remarks>
    public class BarycentreOrderingStrategy : AbstractOrderingStrategy
	{
		public override int Compare(AbstractDrawable d1, AbstractDrawable d2)
		{
			if ((_camera == null))
			{
				throw new Exception("No available camera for computing BarycentreOrderingStrategy");
			}
			// Reflexivity
			if (d1.Equals(d2))
			{
				return 0;
			}
			double dist1 = d1.getDistance(_camera);
			double dist2 = d2.getDistance(_camera);
			return Comparison(dist1, dist2);
		}

		//
		// Operation must be:
		// symetric: compare(a,b)=-compare(b,a)
		// transitive: ((compare(x, y)>0) && (compare(y, z)>0)) implies compare(x, z)>0    true if all Drawables and the Camera don't change position!
		// consistency?: compare(x, y)==0  implies that sgn(compare(x, z))==sgn(compare(y, z))
		//
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
