namespace Nzy3d.Colors
{
	/// <summary>
	/// <see cref="ISingleColorable"/> objects have a single plain color and a must define a setter for it
	/// </summary>
	public interface ISingleColorable
	{
		/// <summary>
		/// Get/Set the color
		/// </summary>
		Color Color { get; set; }
	}
}
