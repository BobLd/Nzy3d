namespace Nzy3d.Maths
{
	/// <summary>
	/// A <see cref="Coord3d"/> stores a 3 dimensional coordinate for cartesian (x, y, y) or
	/// polar (phi, theta, r) (azimuth, elevation/inclination, radius) mode, and provide operators allowing to add, substract,
	/// multiply and divises coordinate values, as well as computing the distance between
	/// two points, and converting polar and cartesian coordinates.
	/// </summary>
	public class Coord3d
	{
		#region "Members"
		public double X;
		public double Y;
		#endregion

		public double Z;

		#region "Constants"
		public readonly static Coord3d ORIGIN = new Coord3d(0, 0, 0);
		public readonly static Coord3d INVALID = new Coord3d(double.NaN, double.NaN, double.NaN);
		public readonly static Coord3d IDENTITY = new Coord3d(1, 1, 1);
		#endregion

		#region "Constructors"
		public Coord3d() : this(0, 0, 0)
		{
		}

		public Coord3d(double xi, double yi, double zi)
		{
			this.X = xi;
			this.Y = yi;
			this.Z = zi;
		}

		public Coord3d(Coord3d c, double zi)
			: this(c.X, c.Y, zi)
		{
		}

		public Coord3d(double[] values)
			: this(values[0], values[1], values[2])
		{
			if (values.Length != 3)
			{
				throw new Exception("When creating a Coord3d from an array of double, the array must contain 3 elements (" + values.Length + " elements found here)");
			}
		}
		#endregion

		#region "Functions"
		/// <summary>
		/// Returns a memberwise clone of current object.
		/// </summary>
		public Coord3d Clone()
		{
			return (Coord3d)MemberwiseClone();
		}

		/// <summary>
		/// Set all values of Coord3d
		/// </summary>
		/// <returns>Self</returns>
		public Coord3d SetValues(double xx, double yy, double zz)
		{
			X = xx;
			Y = yy;
			Z = zz;
			return this;
		}

		/// <summary>
		/// Set all values of Coord3d
		/// </summary>
		/// <returns>Self</returns>
		public Coord3d Set(Coord3d another)
		{
			return SetValues(another.X, another.Y, another.Z);
		}

		/// <summary>
		/// Returns the x and y components as 2d coordinate
		/// </summary>
		public Coord2d GetXY()
		{
			return new Coord2d(this.X, this.Y);
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="value">Value to add to all coordinates (x, y, z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Add(double value)
		{
			return new Coord3d(this.X + value, this.Y + value, this.Z + value);
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		/// <param name="zi">z value to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Add(double xi, double yi, double zi)
		{
			return new Coord3d(this.X + xi, this.Y + yi, this.Z + zi);
		}

		/// <summary>
		/// Add values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Add(Coord3d coord)
		{
			return new Coord3d(this.X + coord.X, this.Y + coord.Y, this.Z + coord.Z);
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="value">Value to add to all coordinates (x, y and z)</param>
		public void AddSelf(double value)
		{
			this.X += value;
			this.Y += value;
			this.Z += value;
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		/// <param name="zi">z value to add</param>
		public void AddSelf(double xi, double yi, double zi)
		{
			this.X += xi;
			this.Y += yi;
			this.Z += zi;
		}

		/// <summary>
		/// Add values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		public void AddSelf(Coord3d coord)
		{
			this.X += coord.X;
			this.Y += coord.Y;
			this.Z += coord.Z;
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x, y and z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Substract(double value)
		{
			return new Coord3d(this.X - value, this.Y - value, this.Z - value);
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		/// <param name="zi">z value to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Substract(double xi, double yi, double zi)
		{
			return new Coord3d(this.X - xi, this.Y - yi, this.Z - zi);
		}

		/// <summary>
		/// Substract values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Substract(Coord3d coord)
		{
			return new Coord3d(this.X - coord.X, this.Y - coord.Y, this.Z - coord.Z);
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x, y and z)</param>
		public void SubstractSelf(double value)
		{
			this.X -= value;
			this.Y -= value;
			this.Z -= value;
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		/// <param name="zi">z value to substract</param>
		public void SubstractSelf(double xi, double yi, double zi)
		{
			this.X -= xi;
			this.Y -= yi;
			this.Z -= zi;
		}

		/// <summary>
		/// Substract values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		public void SubstractSelf(Coord3d coord)
		{
			this.X -= coord.X;
			this.Y -= coord.Y;
			this.Z -= coord.Z;
		}

		/// <summary>
		/// Multiply all components of the current <see cref="Coord3d"/> by a given value and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x, y and z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Multiply(double value)
		{
			return new Coord3d(this.X * value, this.Y * value, this.Z * value);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> by given values and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		/// <param name="zi">z value to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Multiply(double xi, double yi, double zi)
		{
			return new Coord3d(this.X * xi, this.Y * yi, this.Z * zi);
		}

		/// <summary>
		/// Multiply components of another <see cref="Coord3d"/> with components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Multiply(Coord3d coord)
		{
			return new Coord3d(this.X * coord.X, this.Y * coord.Y, this.Z * coord.Z);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> with a given value.
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x, y and z)</param>
		public void MultiplySelf(double value)
		{
			this.X *= value;
			this.Y *= value;
			this.Z *= value;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> with given values.
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		/// <param name="zi">z value to multiply with</param>
		public void MultiplySelf(double xi, double yi, double zi)
		{
			this.X *= xi;
			this.Y *= yi;
			this.Z *= zi;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> with values of another <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		public void MultiplySelf(Coord3d coord)
		{
			this.X *= coord.X;
			this.Y *= coord.Y;
			this.Z *= coord.Z;
		}

		/// <summary>
		/// Divide all components of the current <see cref="Coord3d"/> by a given value and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x, y and z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Divide(double value)
		{
			return new Coord3d(this.X / value, this.Y / value, this.Z / value);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by given values and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to divide with</param>
		/// <param name="yi">y value to divide with</param>
		/// <param name="zi">z value to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Divide(double xi, double yi, double zi)
		{
			return new Coord3d(this.X / xi, this.Y / yi, this.Z / zi);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by components of another <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Divide(Coord3d coord)
		{
			return new Coord3d(this.X / coord.X, this.Y / coord.Y, this.Z / coord.Z);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by a given value.
		/// </summary>
		/// <param name="value">Value to divide both coordinates by (x, y and z)</param>
		public void DivideSelf(double value)
		{
			this.X /= value;
			this.Y /= value;
			this.Z /= value;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by given values.
		/// </summary>
		/// <param name="xi">x value to divide by</param>
		/// <param name="yi">y value to divide by</param>
		/// <param name="zi">z value to divide by</param>
		public void DivideSelf(double xi, double yi, double zi)
		{
			this.X /= xi;
			this.Y /= yi;
			this.Z /= zi;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by values of another <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to divide by</param>
		public void DivideSelf(Coord3d coord)
		{
			this.X /= coord.X;
			this.Y /= coord.Y;
			this.Z /= coord.Z;
		}

		/// <summary>
		/// Returns a new coordinate equal to the negation of current one
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Negative()
		{
			return new Coord3d(-X, -Y, -Z);
		}

		/// <summary>
		/// Assuming current coordinate is in polar system, returns a new coordinate in cartesian system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Cartesian()
		{
			return new Coord3d(Math.Cos(X) * Math.Cos(Y) * Z, Math.Sin(X) * Math.Cos(Y) * Z, Math.Sin(Y) * Z);
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d Polar()
		{
			double r = Math.Sqrt(X * X + Y * Y + Z * Z);
			double d = Math.Sqrt(X * X + Y * Y);
			double a, e;

			if (d == 0 && Z > 0)
			{
				return new Coord3d(0, Math.PI / 2, r);
			}
			else if (d == 0 && Z == 0)
			{
				return new Coord3d(0, 0, 0);
			}
			else if (d == 0 && Z < 0)
			{
				return new Coord3d(0, -Math.PI / 2, r);
			}
			else
			{
				if (Math.Abs(X / d) < 1)
				{
					// Classical case for azimuth
					a = Math.Acos(X / d) * (Y > 0 ? 1 : -1);
				}
				else if (Y == 0 && X > 0)
				{
					a = 0;
				}
				else if (Y == 0 && X < 0)
				{
					a = Math.PI;
				}
				else
				{
					a = 0;
				}
				e = Math.Atan(Z / d);
			}
			return new Coord3d(a, e, r);
		}

		/// <summary>
		/// Compute the distance between two coordinates.
		/// </summary>
		public double Distance(Coord3d coord)
		{
			return Math.Sqrt(Math.Pow(this.X - coord.X, 2) + Math.Pow(this.Y - coord.Y, 2) + Math.Pow(this.Z - coord.Z, 2));
		}

		/// <summary>
		/// Returns the squared distance of coordinates ( x * x + y * y + z * z )
		/// </summary>
		public double MagSquared()
		{
			return X * X + Y * Y + Z * Z;
		}

		/// <summary>
		/// Returns the dot product of current coordinate with another coordinate
		/// </summary>
		public double Dot(Coord3d coord)
		{
			return X * coord.X + Y * coord.Y + Z * coord.Z;
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d NormalizeTo(double len)
		{
			double mag = Math.Sqrt(MagSquared());
			if (mag > 0)
			{
				mag = len / mag;
				return new Coord3d(X * mag, Y * mag, Z * mag);
			}
			else
			{
				return new Coord3d(0, 0, 0);
			}
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d InterpolateTo(Coord3d coord, double f)
		{
			return new Coord3d(X + (coord.X - X) * f, Y + (coord.Y - Y) * f, Z + (coord.Z - Z) * f);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"X={X:0.00000} Y={Y:0.00000} Z={Z:0.00000}"; // return "x=" + X + " y=" + Y + " z=" + Z;
		}

		public double[] ToArray()
		{
			return new double[] { X, Y, Z };
		}

		/// <inheritdoc/>
		public static bool operator ==(Coord3d coord1, Coord3d coord2)
		{
			if (coord1 is null && coord2 is null)
			{
				return true;
			}

			if (coord1 is null || coord2 is null)
			{
				return false;
			}
			else
			{
				return coord1.Equals(coord2);
			}
		}

		/// <inheritdoc/>
		public static bool operator !=(Coord3d coord1, Coord3d coord2)
		{
			if (coord1 is null && coord2 is null)
			{
				return false;
			}

			if (coord1 is null || coord2 is null)
			{
				return true;
			}

			return !coord1.Equals(coord2);
		}
		#endregion

		/// <inheritdoc/>
		public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
		{
			if (obj is not Coord3d other)
			{
				return false;
			}

			if (BitConverter.ToInt64(BitConverter.GetBytes(X)) != BitConverter.ToInt64(BitConverter.GetBytes(other.X)))
			{
				return false;
			}

			if (BitConverter.ToInt64(BitConverter.GetBytes(Y)) != BitConverter.ToInt64(BitConverter.GetBytes(other.Y)))
			{
				return false;
			}

			if (BitConverter.ToInt64(BitConverter.GetBytes(Z)) != BitConverter.ToInt64(BitConverter.GetBytes(other.Z)))
			{
				return false;
			}

			return true;
		}
	}
}
