namespace Nzy3d.Colors.ColorMaps
{
	/// <summary>
	/// Same as <see cref="ColorMapRBG"/> with a nicer name.
	/// </summary>
	public class ColorMapRainbow : ColorMapRBG
	{
		/// <summary>
		/// Same as <see cref="ColorMapRBG"/> with a nicer name.
		/// </summary>
		public ColorMapRainbow()
        {
        }

		/// <summary>
		/// Returns the string representation of this colormap.
		/// </summary>
		public override string ToString()
		{
			return this.GetType().Name; //"ColorMapRainbow";
		}
	}
}
