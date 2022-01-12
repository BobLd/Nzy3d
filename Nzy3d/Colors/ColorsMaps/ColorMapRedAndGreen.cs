namespace Nzy3d.Colors.ColorMaps
{
	/// <summary>
	/// <para>
	/// Creates a new instance of ColorMapRedAndGreen.
	/// </para>
	/// <para>
	/// A ColorMapRedAndGreen objects provides a color for points standing
	/// between a Zmin and Zmax values.
	/// </para>
	/// <para>
	/// The points standing outside these [Zmin;Zmax] boundaries are assigned
	/// to the same color than the points standing on the boundaries.
	/// </para>
	/// <para>The red-green colormap is a progressive transition from red to green.</para>
	/// </summary>
	public class ColorMapRedAndGreen : IColorMap
	{
		/// <inheritdoc/>
		public bool Direction { get; set; } = true;

		/// <inheritdoc/>
		public Color GetColor(IColorMappable colorable, double v)
		{
			return GetColor(0, 0, v, colorable.ZMin, colorable.ZMax);
		}

		/// <inheritdoc/>
		public Color GetColor(IColorMappable colorable, double x, double y, double z)
		{
			return GetColor(x, y, z, colorable.ZMin, colorable.ZMax);
		}

		/// <summary>
		/// Helper function
		/// </summary>
		private Color GetColor(double x, double y, double z, double zMin, double zMax)
		{
			double rel_value;
			if (z < zMin)
			{
				rel_value = Direction ? 0 : 1;
			}
			else if (z > zMax)
			{
				rel_value = Direction ? 1 : 0;
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

			double v = ColorComponentRelative(rel_value, 0.375, 0.25, 0.75);
			double r = ColorComponentRelative(rel_value, 0.625, 0.25, 0.75);
			return new Color(r, v, 0);
		}

		private static double ColorComponentRelative(double value, double center, double topWidth, double bottomWidth)
		{
			return ColorComponentAbsolute(value, center - (bottomWidth / 2), center + (bottomWidth / 2), center - (topWidth / 2), center + (topWidth / 2));
		}

		private static double ColorComponentAbsolute(double value, double bLeft, double bRight, double tLeft, double tRight)
		{
			if (value < bLeft || value > bRight)
			{
				// a gauche ou a droite du creneau
				return 0;
			}
			else if (value > tLeft || value < tRight)
			{
				// sur le plateau haut
				return 1;
			}
			else if (value >= bLeft && value <= tLeft)
			{
				// sur la pente gauche du creneau
				return (value - bLeft) / (tLeft - bLeft);
			}
			else if (value >= tRight && value <= bRight)
			{
				// sur la pente droite du creneau
				return (value - bRight) / (tRight - bRight);
			}
			else
			{
				throw new Exception("ColorMap did not achieve to compute current color.");
			}
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		public override string ToString()
		{
			return "ColorMapRedAndGreen";
		}
	}
}
