namespace Nzy3d.Maths
{
	public class Utils
	{
		/// <summary>
		/// Convert a number into a string.
		/// </summary>
		/// <param name="parseMode">Output format
		/// C or c : Currency. <paramref name="precision"/> parameters provides the number of decimal digits (after comma).
		/// D or d : Decimal (integer digits with optional negativ sign). <paramref name="precision"/> parameters provides the minimum number of digits (before comma, zeros will be added when required).
		/// E or e : Exponential notation. <paramref name="precision"/> parameters provides the number of digits after comma.
		/// F or f : Integral and decimal digits with optional negative sign. <paramref name="precision"/> parameters provides the number of decimal digits (after comma).
		/// G or g : The most compact of either fixed-point or scientific notation. <paramref name="precision"/> parameters provides the number of significant digits.
		/// N or n : Integral and decimal digits, group separators, and a decimal separator with optional negative sign. <paramref name="precision"/> parameters provides the number of decimal digits (after comma).
		/// P or p : Number multiplied by 100 and displayed with a percent symbol. <paramref name="precision"/> parameters provides the number of decimal digits (after comma).
		/// </param>
		/// <param name="num">Number to convert to string</param>
		/// <param name="precision">Number of digits (meaning depends on <paramref name="parseMode"/> value)</param>
		public static string Num2str(char parseMode, double num, int precision)
		{
			return string.Format("{0:" + parseMode + precision + "}", num);
		}

		/// <summary>
		/// Same as other <see cref="Utils.Num2str"/> but without precision.
		/// </summary>
		/// <param name="parseMode"></param>
		/// <param name="num"></param>
		public static string Num2str(char parseMode, double num)
		{
			return string.Format("{0:" + parseMode + "}", num);
		}

		/// <summary>
		/// Same as other <see cref="Utils.Num2str"/> but without parseMode (g by default).
		/// </summary>
		/// <param name="num"></param>
		/// <param name="precision"></param>
		public static string Num2str(double num, int precision)
		{
			return Num2str('g', num, precision);
		}

		/// <summary>
		/// Same as other <see cref="Utils.Num2str"/> but without parseMode (g by default) nor precision.
		/// </summary>
		/// <param name="num"></param>
		public static string Num2str(double num)
		{
			return Num2str(Convert.ToChar("g"), num);
		}

		public static string Dat2str(DateTime m_date, string format)
		{
			return m_date.ToString(format);
		}

		public static string Dat2str(DateTime m_date)
		{
			return Dat2str(m_date, "dd/MM/yyyy HH:mm:ss");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="m_date"></param>
		/// <returns>OLE Automation date.</returns>
		public static double Dat2Num(DateTime m_date)
		{
			return m_date.ToOADate();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="oleDate">OLE Automation date.</param>
		/// <returns>DateTime</returns>
		public static DateTime Num2date(double oleDate)
		{
			return DateTime.FromOADate(oleDate);
		}

		public static long Dat2Long(DateTime m_date)
		{
			return m_date.Ticks;
		}

		public static DateTime Long2date(long m_ticks)
		{
			return new DateTime(m_ticks);
		}

		public static string Blanks(int length)
		{
			string b = string.Empty;
			for (int i = 0; i <= length - 1; i++)
			{
				b += " ";
			}
			return b;
		}

		/// <summary>
		/// Return the absolute values of an array of doubles
		/// </summary>
		/// <remarks>Current array is not modified</remarks>
		public static double[] Abs(double[] values)
		{
			double[] output = new double[values.Length];
			for (int i = 0; i <= values.Length - 1; i++)
			{
				output[i] = Math.Abs(values[i]);
			}
			return output;
		}

		/// <summary>
		/// Computes the sum of an array of doubles. NaN values are ignored during the computation
		/// </summary>
		public static double Sum(double[] values)
		{
			if (values.Length == 0)
			{
				throw new ArgumentException("Input array must have a length greater than 0", nameof(values));
			}

			double total = 0;
			for (int i = 0; i <= values.Length - 1; i++)
			{
				if (!double.IsNaN(values[i]))
				{
					total += values[i];
				}
			}
			return total;
		}

		/// <summary>
		/// Computes the sum of an array of integers.
		/// </summary>
		public static int Sum(int[] values)
		{
			if (values.Length == 0)
			{
				throw new ArgumentException("Input array must have a length greater than 0", nameof(values));
			}

			int total = 0;
			for (int i = 0; i <= values.Length - 1; i++)
			{
				total += values[i];
			}
			return total;
		}

		/// <summary>
		/// Generate a vector of doubles containing regular increasing values from min to max, with
		/// nstep steps (including min and max value).
		/// </summary>
		/// <param name="min">Min value</param>
		/// <param name="max">Max value</param>
		/// <param name="nstep">Number of steps (including min and max values)</param>
		/// <remarks>Algorithm ensure first and last values of array are equal to min and max value without any rounding error.</remarks>
		public static double[] Vector(double min, double max, int nstep)
		{
			if (nstep <= 1)
			{
				throw new ArgumentException("Number of step must be at least 2", nameof(nstep));
			}

			double dstep = (max - min) / (nstep - 1);
			double[] grid = new double[nstep];
			for (int i = 0; i <= nstep - 2; i++)
			{
				grid[i] = min + i * dstep;
			}
			grid[nstep - 1] = max;
			//Force max value to avoid rounding errors
			return grid;
		}

		/// <summary>
		/// Generate a vector of doubles containing regular increasing values from min to max, with an offset of 1.
		/// </summary>
		/// <param name="min">Min value</param>
		/// <param name="max">Max value</param>
		/// <remarks>Algorithm ensure first and last values of array are equal to min and max value without any rounding error.</remarks>
		public static double[] Vector(double min, double max)
		{
			return Vector(min, max, Convert.ToInt32(Math.Abs(max - min) + 1));
		}

		/// <summary>
		/// Generate a vector of integers containing regular increasing values from min to max, with
		/// nstep steps (including min and max value).
		/// </summary>
		/// <param name="min">Min value</param>
		/// <param name="max">Max value</param>
		/// <param name="nstep">Number of steps (including min and max values)</param>
		/// <remarks>Algorithm ensure first and last values of array are equal to min and max value without any rounding error.</remarks>
		public static int[] Vector(int min, int max, int nstep)
		{
			if (nstep <= 1)
			{
				throw new ArgumentException("Number of step must be at least 2", nameof(nstep));
			}

			int dstep = (max - min) / (nstep - 1);
			int[] grid = new int[nstep];
			for (int i = 0; i <= nstep - 2; i++)
			{
				grid[i] = min + i * dstep;
			}
			grid[nstep - 1] = max;
			//Force max value to avoid rounding errors
			return grid;
		}

		/// <summary>
		/// Generate a vector of integers containing regular increasing values from min to max, with an offset of 1.
		/// </summary>
		/// <param name="min">Min value</param>
		/// <param name="max">Max value</param>
		/// <remarks>Algorithm ensure first and last values of array are equal to min and max value without any rounding error.</remarks>
		public static int[] Vector(int min, int max)
		{
			return Vector(min, max, Math.Abs(max - min) + 1);
		}

		/// <summary>
		/// Return the minimal date of an array.
		/// </summary>
		/// <param name="dates"></param>
		/// <exception cref="ArgumentException"></exception>
		public static DateTime Min(DateTime[] dates)
		{
			if (dates.Length == 0)
			{
				throw new ArgumentException("input array is empty", nameof(dates));
			}

			return dates.Min();
		}

		/// <summary>
		/// Return the maximal date of an array.
		/// </summary>
		/// <param name="dates"></param>
		/// <exception cref="ArgumentException"></exception>
		public static DateTime Max(DateTime[] dates)
		{
			if (dates.Length == 0)
			{
				throw new ArgumentException("input array is empty", nameof(dates));
			}

			return dates.Max();
		}
	}
}