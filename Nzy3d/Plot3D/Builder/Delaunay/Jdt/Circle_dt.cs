namespace Nzy3d.Plot3D.Builder.Delaunay.Jdt
{
    /// <summary>
    /// This class represents a 3D simple circle used by the Delaunay Triangulation class
    /// </summary>
    public class Circle_dt
    {
        /// <summary>
        /// Constructs a new Circle_dt.
        /// </summary>
        /// <param name="c">Center of the circle.</param>
        /// <param name="r">Radius of the circle.</param>
        public Circle_dt(Point_dt c, double r)
        {
            Center = c;
            Radius = r;
        }

        /// <summary>
        /// Copy Constructor. Creates a new Circle with same properties of <paramref name="circle"/>
        /// </summary>
        /// <param name="circle">Circle to clone.</param>
        public Circle_dt(Circle_dt circle) : this(circle.Center, circle.Radius)
        {
        }

        /// <summary>
        /// Gets the center of the circle.
        /// </summary>
        /// <returns>The center of the circle.</returns>
        public Point_dt Center { get; }

        /// <summary>
        /// Gets the radius of the circle.
        /// </summary>
        /// <returns>The radius of the circle.</returns>
        public double Radius { get; }
    }
}
