namespace Nzy3d.Maths
{
	public class Scale
	{
		public Scale(float min, float max)
		{
			Min = min;
			Max = max;
		}

		public float Min { get; set; }

		public float Max { get; set; }

		public float Range
		{
			get { return Max - Min; }
		}

		/// <summary>
		/// Add a value to min and max values of the scale
		/// </summary>
		/// <param name="value">Value to add</param>
		/// <returns>New scale with added value to min &amp; max</returns>
		/// <remarks>Current object is not modified</remarks>
		public Scale Add(float value)
		{
			return new Scale(Min + value, Max + value);
		}

		/// <summary>
		/// Return True if value is inside [Min;Max]
		/// </summary>
		public bool Contains(float value)
		{
			return Min <= value && value <= Max;
		}

		public bool IsMaxNan()
		{
			return double.IsNaN(Max);
		}

		public bool IsMinNan()
		{
			return double.IsNaN(Min);
		}

		/// <summary>
		/// Returns True if Min &lt;= Max
		/// </summary>
		public bool Valid()
		{
			return Min <= Max;
		}

		public static Scale Widest(Scale scale1, Scale scale2)
		{
			return new Scale(MathF.Min(scale1.Min, scale2.Min), MathF.Max(scale1.Max, scale2.Max));
		}

		public static Scale Thinest(Scale scale1, Scale scale2)
		{
			return new Scale(MathF.Max(scale1.Min, scale2.Min), MathF.Min(scale1.Max, scale2.Max));
		}

		public static Scale Enlarge(Scale scale, float ratio)
		{
			float offset = (scale.Max - scale.Min) * ratio;
			if (offset == 0)
			{
				offset = 1;
			}
			return new Scale(scale.Min - offset, scale.Max + offset);
		}

		public override string ToString()
		{
			return $"min={Min} max={Max}";
		}
	}
}
