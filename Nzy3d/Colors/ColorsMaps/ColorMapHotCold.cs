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
		public bool Direction { get; set; }

		public Color GetColor(IColorMappable colorable, float v)
		{
			return GetColor(0, 0, v, colorable.ZMin, colorable.ZMax);
		}

		public Color GetColor(IColorMappable colorable, float x, float y, float z)
		{
			return GetColor(x, y, z, colorable.ZMin, colorable.ZMax);
		}

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
			float v = ColorComponentAbsolute(rel_value, 0.25f, 0.875f, 0.25f, 0.5f);
			float b = ColorComponentAbsolute(rel_value, 0.125f, 0.875f, 0.5f, 0.5f);
			float r = ColorComponentAbsolute(rel_value, 0.125f, 1.25f, 0.5f, 0.75f);
			return new Color(r, v, b);
		}

		private static float ColorComponentAbsolute(float value, float bLeft, float bRight, float tLeft, float tRight)
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
