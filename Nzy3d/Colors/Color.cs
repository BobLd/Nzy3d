using OpenTK.Mathematics;

namespace Nzy3d.Colors
{
	/// <summary>
	///
	/// </summary>
	public struct Color
	{
		#region "Members"
		/// <summary>
		/// Red channel.
		/// </summary>
		public float R;

		/// <summary>
		/// Green channel.
		/// </summary>
		public float G;

		/// <summary>
		/// Blue channel.
		/// </summary>
		public float B;

		/// <summary>
		/// Alpha channel.
		/// </summary>
		public float A;
		#endregion

		#region "Constants"
		public static readonly Color BLACK = new Color(0.0f, 0.0f, 0.0f);
		public static readonly Color WHITE = new Color(1.0f, 1.0f, 1.0f);
		public static readonly Color GRAY = new Color(0.5f, 0.5f, 0.5f);
		public static readonly Color RED = new Color(1.0f, 0.0f, 0.0f);
		public static readonly Color GREEN = new Color(0.0f, 1.0f, 0.0f);
		public static readonly Color BLUE = new Color(0.0f, 0.0f, 1.0f);
		public static readonly Color YELLOW = new Color(1.0f, 1.0f, 0.0f);
		public static readonly Color MAGENTA = new Color(1.0f, 0.0f, 1.0f);
		public static readonly Color CYAN = new Color(0.0f, 1.0f, 1.0f);
		#endregion

		static internal Random randObj = new Random();

		#region "Constructors"
		/// <summary>
		/// Initialize a color with values between 0 and 1 and an alpha channel set to maximum
		/// </summary>
		/// <param name="r">Red value (between 0 and 1)</param>
		/// <param name="g">Green value (between 0 and 1)</param>
		/// <param name="b">Blue value (between 0 and 1)</param>
		public Color(float r, float g, float b)
			: this(r, g, b, 1)
		{
		}

		/// <summary>
		/// Initialize a color with values between 0 and 255 and an alpha channel set to maximum
		/// </summary>
		/// <param name="r">Red value (between 0 and 255)</param>
		/// <param name="g">Green value (between 0 and 255)</param>
		/// <param name="b">Blue value (between 0 and 255)</param>
		public Color(int r, int g, int b)
			: this(r, g, b, 1)
		{
		}

		/// <summary>
		/// Initialize a color with values between 0 and 1
		/// </summary>
		/// <param name="r">Red value (between 0 and 1)</param>
		/// <param name="g">Green value (between 0 and 1)</param>
		/// <param name="b">Blue value (between 0 and 1)</param>
		/// <param name="a">a value (between 0 and 1)</param>
		public Color(float r, float g, float b, float a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		/// <summary>
		///  Initialize a color with values between 0 and 255
		/// </summary>
		/// <param name="r">Red value (between 0 and 255)</param>
		/// <param name="g">Green value (between 0 and 255)</param>
		/// <param name="b">Blue value (between 0 and 255)</param>
		/// <param name="a">a value (between 0 and 255)</param>
		public Color(int r, int g, int b, int a)
			: this(r / 255f, g / 255f, b / 255f, a / 255f)
		{
		}
		#endregion

		#region "Methods"
		/// <summary>
		/// Multiply current color components (including alpha value) by <paramref name="factor"/> color components values and assign value to current color.
		/// </summary>
		/// <param name="factor">Multiply values.</param>
		public void Mul(Color factor)
		{
			this.R *= factor.R;
			this.G *= factor.G;
			this.B *= factor.B;
			this.A *= factor.A;
		}

		/// <summary>
		/// Returns the hexadecimal representation of this color, without alpha channel value.
		/// </summary>
		public string HexString
		{
			get
			{
				return $"#{R:X2}{G:X2}{B:X2}";
			}
		}

		/// <summary>
		/// Returns the string representation of this color, including alpha channel value
		/// </summary>
		public override string ToString()
		{
			return $"(Color) r={R} g={G} b={B} a={A}";
		}

		public float[] ToArray()
		{
			return new float[] { R, G, B, A };
		}

		public float[] Negative()
		{
			return new float[] { 1 - R, 1 - G, 1 - B, A };
		}

		public Color NegativeColor()
		{
			return new Color(1 - R, 1 - G, 1 - B, A);
		}

		/// <summary>
		/// Compute the distance between two colors.
		/// </summary>
		/// <remarks>See https://en.wikipedia.org/wiki/Color_difference</remarks>
		public float Distance(Color c)
		{
			return MathF.Sqrt(DistanceSq(c));
		}

		/// <summary>
		/// Compute the square distance between two colors.
		/// </summary>
		/// <remarks>See https://en.wikipedia.org/wiki/Color_difference</remarks>
		public float DistanceSq(Color c)
		{
			return MathF.Pow(R - c.R, 2) + MathF.Pow(G - c.G, 2) + MathF.Pow(B - c.B, 2);
		}

		public static Color Random()
		{
			return new Color(randObj.NextSingle(), randObj.NextSingle(), randObj.NextSingle());
		}

		public System.Drawing.Color ToColor()
		{
			return System.Drawing.Color.FromArgb((int)A, (int)R, (int)G, (int)B);
		}
		#endregion

		public Color4 OpenTKColor4
		{
			get { return new Color4(Convert.ToSingle(this.R), Convert.ToSingle(this.G), Convert.ToSingle(this.B), Convert.ToSingle(this.A)); }
		}
	}
}
