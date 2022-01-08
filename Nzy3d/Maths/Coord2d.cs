namespace Nzy3d.Maths
{
	/// <summary>
	/// A <see cref="Coord2d"/> stores a 2 dimensional coordinate for cartesian (x,y) or
	/// polar (a,r) mode, and provide operators allowing to add, substract,
	/// multiply and divises coordinate values, as well as computing the distance between
	/// two points, and converting polar and cartesian coordinates.
	/// </summary>
	public class Coord2d
	{
		#region "Members"
		public double X;
		public double Y;
		#endregion

		#region "Constants"
		public static readonly Coord2d ORIGIN = new Coord2d(0, 0);
		public static readonly Coord2d INVALID = new Coord2d(double.NaN, double.NaN);
		public static readonly Coord2d IDENTITY = new Coord2d(1, 1);
		#endregion

		#region "Constructors"
		public Coord2d() : this(0, 0)
		{
		}

		public Coord2d(double xi, double yi)
		{
			this.X = xi;
			this.Y = yi;
		}
		#endregion

		#region "Constructors"
		/// <summary>
		/// Set all values of Coord2d
		/// </summary>
		/// <returns>Self</returns>
		public Coord2d setvalues(double xx, double yy)
		{
			X = xx;
			Y = yy;
			return this;
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="value">Value to add to both coordinates (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d @add(double value)
		{
			return new Coord2d(this.X + value, this.Y + value);
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d @add(double xi, double yi)
		{
			return new Coord2d(this.X + xi, this.Y + yi);
		}

		/// <summary>
		/// Add values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d @add(Coord2d coord)
		{
			return new Coord2d(this.X + coord.X, this.Y + coord.Y);
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="value">Value to add to both coordinates (x and y)</param>
		public void addSelf(double value)
		{
			this.X += value;
			this.Y += value;
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		public void addSelf(double xi, double yi)
		{
			this.X += xi;
			this.Y += yi;
		}

		/// <summary>
		/// Add values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		public void addSelf(Coord2d coord)
		{
			this.X += coord.X;
			this.Y += coord.Y;
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d substract(double value)
		{
			return new Coord2d(this.X - value, this.Y - value);
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d substract(double xi, double yi)
		{
			return new Coord2d(this.X - xi, this.Y - yi);
		}

		/// <summary>
		/// Substract values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d substract(Coord2d coord)
		{
			return new Coord2d(this.X - coord.X, this.Y - coord.Y);
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x and y)</param>
		public void substractSelf(double value)
		{
			this.X -= value;
			this.Y -= value;
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		public void substractSelf(double xi, double yi)
		{
			this.X -= xi;
			this.Y -= yi;
		}

		/// <summary>
		/// Substract values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		public void substractSelf(Coord2d coord)
		{
			this.X -= coord.X;
			this.Y -= coord.Y;
		}

		/// <summary>
		/// Multiply all components of the current <see cref="Coord2d"/> by a given value and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d multiply(double value)
		{
			return new Coord2d(this.X * value, this.Y * value);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> by given values and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d multiply(double xi, double yi)
		{
			return new Coord2d(this.X * xi, this.Y * yi);
		}

		/// <summary>
		/// Multiply components of another <see cref="Coord2d"/> with components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d multiply(Coord2d coord)
		{
			return new Coord2d(this.X * coord.X, this.Y * coord.Y);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> with a given value.
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x and y)</param>
		public void multiplySelf(double value)
		{
			this.X *= value;
			this.Y *= value;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> with given values.
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		public void multiplySelf(double xi, double yi)
		{
			this.X *= xi;
			this.Y *= yi;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> with values of another <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		public void multiplySelf(Coord2d coord)
		{
			this.X *= coord.X;
			this.Y *= coord.Y;
		}

		/// <summary>
		/// Divide all components of the current <see cref="Coord2d"/> by a given value and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d divide(double value)
		{
			return new Coord2d(this.X / value, this.Y / value);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by given values and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to divide with</param>
		/// <param name="yi">y value to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d divide(double xi, double yi)
		{
			return new Coord2d(this.X / xi, this.Y / yi);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by components of another <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d divide(Coord2d coord)
		{
			return new Coord2d(this.X / coord.X, this.Y / coord.Y);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by a given value.
		/// </summary>
		/// <param name="value">Value to divide both coordinates by (x and y)</param>
		public void divideSelf(double value)
		{
			this.X /= value;
			this.Y /= value;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by given values.
		/// </summary>
		/// <param name="xi">x value to divide by</param>
		/// <param name="yi">y value to divide by</param>
		public void divideSelf(double xi, double yi)
		{
			this.X /= xi;
			this.Y /= yi;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by values of another <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="coord">Coordinate with values to divide by</param>
		public void divideSelf(Coord2d coord)
		{
			this.X /= coord.X;
			this.Y /= coord.Y;
		}

		/// <summary>
		/// Assuming current coordinate is in polar system, returns a new coordinate in cartesian system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d cartesian()
		{
			return new Coord2d(Math.Cos(X) * Y, Math.Sin(X) * Y);
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d polar()
		{
			return new Coord2d(Math.Atan(Y / X), Math.Sqrt(X * X + Y * Y));
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// with real polar values, i.e. with an angle in the range [0, 2*PI]
		/// Source : http://fr.wikipedia.org/wiki/Coordonn%C3%A9es_polaires
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d fullPolar()
		{
			double radius = Math.Sqrt(X * X + Y * Y);
			if (X < 0)
			{
				return new Coord2d(Math.Atan(Y / X), radius);
			}
			else if (X > 0)
			{
				if (Y >= 0)
				{
					return new Coord2d(Math.Atan(Y / X), radius);
				}
				else
				{
					return new Coord2d(Math.Atan(Y / X) + 2 * Math.PI, radius);
				}
				//x=0
			}
			else
			{
				if (Y > 0)
				{
					return new Coord2d(Math.PI / 2, radius);
				}
				else if (Y > 0)
				{
					return new Coord2d(3 * Math.PI / 2, radius);
					//y=0
				}
				else
				{
					return new Coord2d(0, 0);
				}
			}
		}

		/// <summary>
		/// Compute the distance between two coordinates.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public double distance(Coord2d coord)
		{
			return Math.Sqrt(Math.Pow((this.X - coord.X), 2) + Math.Pow((this.Y - coord.Y), 2));
		}

		public override string ToString()
		{
			return ("x=" + X + " y=" + Y);
		}

		public double[] toArray()
		{
			return new double[] {
				X,
				Y
			};
		}
		#endregion

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return (X, Y).GetHashCode();
		}

		/// <inheritdoc/>
		public override bool Equals(object? obj)
		{
			if (obj is not Coord2d other)
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

			return true;
		}
	}
}
