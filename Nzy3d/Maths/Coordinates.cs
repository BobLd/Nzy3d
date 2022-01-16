namespace Nzy3d.Maths
{
	/// <summary>
	/// A simple utility class for storing a list of x, y, and z coordinates as
	/// arrays of float values.
	/// </summary>
	public sealed class Coordinates
	{
        public Coordinates(float[] xi, float[] yi, float[] zi)
		{
			this.X = xi;
			this.Y = yi;
			this.Z = zi;
		}

		public Coordinates(Coord3d[] coords)
		{
			int nbCoords = coords.Length;
			X = new float[nbCoords];
			Y = new float[nbCoords];
			Z = new float[nbCoords];
			for (int iCoord = 0; iCoord <= nbCoords - 1; iCoord++)
			{
				X[iCoord] = (float)coords[iCoord].X;
				Y[iCoord] = (float)coords[iCoord].Y;
				Z[iCoord] = (float)coords[iCoord].Z;
			}
		}

		public Coordinates(List<Coord3d> coords)
		{
			int nbCoords = coords.Count;
			X = new float[nbCoords];
			Y = new float[nbCoords];
			Z = new float[nbCoords];
			for (int iCoord = 0; iCoord <= nbCoords - 1; iCoord++)
			{
				X[iCoord] = (float)coords[iCoord].X;
				Y[iCoord] = (float)coords[iCoord].Y;
				Z[iCoord] = (float)coords[iCoord].Z;
			}
		}

        public float[] X { get; }

        public float[] Y { get; }

        public float[] Z { get; }

        public Coord3d[] ToArray()
		{
			var array = new Coord3d[X.Length];
			for (int iCoord = 0; iCoord <= X.Length - 1; iCoord++)
			{
				array[iCoord] = new Coord3d(X[iCoord], Y[iCoord], Z[iCoord]);
			}
			return array;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			string txt = "";
			for (int iCoord = 0; iCoord <= X.Length - 1; iCoord++)
			{
				if (iCoord > 0)
				{
					txt += "\r\n";
				}
				txt += "[" + iCoord + "]" + X[iCoord] + "|" + Y[iCoord] + "|" + Z[iCoord];
			}
			return txt;
		}
	}
}
