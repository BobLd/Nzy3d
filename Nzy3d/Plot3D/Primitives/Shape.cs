namespace Nzy3d.Plot3D.Primitives
{
	/// <summary>
	/// Allows building custom shapes defined by an {@link ArrayList} of {@link Polygon}s.
	/// Such {@link ArrayList} must be defined by the user.
	/// </summary>
	/// <remarks></remarks>
	public class Shape : AbstractComposite
	{
		public Shape() : base()
		{
		}

		public Shape(List<Polygon> polygons) : base()
		{
			this.Add(polygons);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
