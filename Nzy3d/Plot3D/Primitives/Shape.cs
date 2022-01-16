namespace Nzy3d.Plot3D.Primitives
{
	/// <summary>
	/// Allows building custom shapes defined by an <see cref="System.Collections.IEnumerable"/> of <see cref="Polygon"/>s.
	/// Such <see cref="System.Collections.IEnumerable"/> must be defined by the user.
	/// </summary>
	public class Shape : AbstractComposite
	{
		public Shape() : base()
		{
		}

		public Shape(IEnumerable<Polygon> polygons) : base()
		{
			this.Add(polygons);
		}
	}
}
