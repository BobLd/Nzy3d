namespace Nzy3d.Colors.ColorMaps
{
	/// <summary>
	/// <para>Creates a new instance of ColorMapHotCold.</para>
	/// <para>
	/// A ColorMapHotCold objects provides a color for points standing
	/// between a Zmin and Zmax values.
	/// </para>
	/// <para>
	/// The points standing outside these [Zmin;Zmax] boundaries are assigned
	/// to the same color than the points standing on the boundaries.
	/// </para>
	/// <para>
	/// The hot-cold colormap is a progressive transition from blue,
	/// to white and last, red.
	/// </para>
	/// </summary>
	public class ColorMapHotCold : IColorMap
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
			double v = ColorComponentAbsolute(rel_value, 0.25, 0.875, 0.25, 0.5);
			double b = ColorComponentAbsolute(rel_value, 0.125, 0.875, 0.5, 0.5);
			double r = ColorComponentAbsolute(rel_value, 0.125, 1.25, 0.5, 0.75);
			return new Color(r, v, b);
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
			return "ColorMapHotCold";
		}
	}
}
