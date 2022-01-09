namespace Nzy3d.Colors.ColorMaps
{
	/// <summary>
	///<para>
	/// Creates a new instance of ColorMapWhiteRed.
	/// </para>
	/// <para>
	/// A ColorMapWhiteRed objects provides a color for points standing
	/// between a Zmin and Zmax values.
	/// </para>
	/// <para>
	/// The points standing outside these [Zmin;Zmax] boundaries are assigned
	///  to the same color than the points standing on the boundaries.
	/// </para>
	/// <para>The white-red colormap is a progressive transition from white to red.</para>
	/// </summary>
	public class ColorMapWhiteRed : IColorMap
	{
		public bool Direction { get; set; }

		public Color GetColor(IColorMappable colorable, float v)
		{
			return GetColor(0, 0, v, colorable.ZMin, colorable.ZMax);
		}

		public Color GetColor(IColorMappable colorable, float x, float y, float z)
		{
			return GetColor(x, y, z, colorable.ZMin, colorable.ZMax);
		}

		/// <summary>
		/// Helper function
		/// </summary>
		private Color GetColor(float x, float y, float z, float zMin, float zMax)
		{
            float rel_value;
            if (z < zMin)
			{
				rel_value = 0;
			}
			else if (z > zMax)
			{
				rel_value = 1;
			}
			else
			{
				if (Direction)
				{
					rel_value = (z - zMin) / (zMax - zMin);
				}
				else
				{
					rel_value = (zMax - z) / (zMax - zMin);
				}
			}
			return new Color(1, rel_value, rel_value);
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		public override string ToString()
		{
			return "ColorMapWhiteRed";
		}
	}
}
