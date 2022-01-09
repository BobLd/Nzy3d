namespace Nzy3d.Colors.ColorMaps
{
	public class ColorMapRBG : IColorMap
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
		/// <para>
		/// Helper function
		/// </para>
		/// <para>
		/// Return the influence of a color, by comparing the input value to the color spectrum.
		/// </para>
		/// <para>
		///  The design of a colormap implies defining the influence of each base color
		///  (red, green, and blue) over the range of input data.
		///  For this reason, the value given to this function is a number between 0 and 1,
		///  indicating the position of the input value in the colormap.
		///  Any value standing outside of colormap boundaries should have the "maximal" or
		///  "minimal" color.
		/// </para>
		/// <para>
		///  Exemple:
		///  A rainbow colormap is a progressive transition from blue, to green and red.
		///  The mix between these 3 colors, may be expressed by the definition of 3 functions:
		/// </para>
		///  <code>
		///        blue     green     red
		///      /-------\/-------\/-------\
		///     /        /\       /\        \
		///    /        /  \     /  \        \
		///   /        /    \   /    \        \
		///  |----------------|----------------|
		///  0               0.5               1
		///  </code>
		///
		///  In order to get the color of an input value standing between 0 and 1, the user
		///  should call:
		/// d
		/// <code>
		///  float blue  = (float) creneau_relatif( rel_value, 0.25, 0.25, 0.75 );
		///  float green = (float) creneau_relatif( rel_value, 0.50, 0.25, 0.75 );
		///  float red   = (float) creneau_relatif( rel_value, 0.75, 0.25, 0.75 );
		///  return new Color4f( red, green, blue, 1.0f );
		///  </code>
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
			float b = ColorComponentRelative(rel_value, 0.25f, 0.25f, 0.75f);
			float g = ColorComponentRelative(rel_value, 0.5f, 0.25f, 0.75f);
			float r = ColorComponentRelative(rel_value, 0.75f, 0.25f, 0.75f);
			return new Color(r, g, b);
		}

		private static float ColorComponentRelative(float value, float center, float topWidth, float bottomWidth)
		{
			return ColorComponentAbsolute(value, center - (bottomWidth / 2), center + (bottomWidth / 2), center - (topWidth / 2), center + (topWidth / 2));
		}

		private static float ColorComponentAbsolute(float value, float bLeft, float bRight, float tLeft, float tRight)
		{
			if (value < bLeft || value >= bRight)
			{
				// a gauche ou a droite du creneau
				return 0;
			}
			else if (value >= tLeft && value < tRight)
			{
				// sur le plateau haut
				return 1;
			}
			else if (value >= bLeft && value < tLeft)
			{
				// sur la pente gauche du creneau
				return (value - bLeft) / (tLeft - bLeft);
			}
			else if (value >= tRight && value < bRight)
			{
				// sur la pente droite du creneau
				return (value - bRight) / (tRight - bRight);
			}
			else
			{
				//Throw New Exception("ColorMap did not achieve to compute current color.")
				return 0;
			}
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		public override string ToString()
		{
			return "ColorMapRGB";
		}
	}
}
