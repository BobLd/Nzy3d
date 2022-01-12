namespace Nzy3d.Plot3D.Primitives
{
	/// <summary>
	/// Allows building custom shapes defined by an {@link ArrayList} of {@link Polygon}s.
	/// Such {@link ArrayList} must be defined by the user.
	/// </summary>
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
