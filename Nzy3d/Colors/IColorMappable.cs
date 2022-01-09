namespace Nzy3d.Colors
{
	public interface IColorMappable
	{
		/// <summary>
		/// Get/Set the lower value boundary for a <see cref="ColorMaps.IColorMap"/>.
		/// </summary>
		float ZMin { get; set; }

		/// <summary>
		/// Get/Set the upper value boundary for a <see cref="ColorMaps.IColorMap"/>.
		/// </summary>
		float ZMax { get; set; }
	}
}
