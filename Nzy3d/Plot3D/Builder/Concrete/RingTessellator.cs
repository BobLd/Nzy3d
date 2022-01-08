using Nzy3d.Colors;
using Nzy3d.Maths;
using Nzy3d.Plot3D.Primitives;

namespace Nzy3d.Plot3D.Builder.Concrete
{
    public class RingTessellator : OrthonormalTessellator
    {
        internal float _ringMin;
        internal float _ringMax;
        internal ColorMapper _cmap;

        internal Color _factor;
        public RingTessellator(float ringMin, float ringMax, ColorMapper cmap, Color factor)
        {
            _ringMin = ringMin;
            _ringMax = ringMax;
            _cmap = cmap;
            _factor = factor;
        }

        private RingTessellator()
        {
            throw new Exception("Forbidden constructor");
        }

        public override AbstractComposite Build(float[] x, float[] y, float[] z)
        {
            SetData(x, y, z);
            Shape s = new Shape();
            s.Add(GetInterpolatedRingPolygons());
            return s;
        }

        /// <summary>
        /// Load data standing on an orthonormal grid.
        /// <br/>
        /// Each input point (i.e. the association of x(i), y(j), z(i)(j)) will be
        /// represented by a polygon centered on this point. The default coordinates
        /// of this polygon will be:
        /// <ul>
        /// <li>x(i-1), y(j+1), z(i-1)(j+1)</li>
        /// <li>x(i-1), y(j-1), z(i-1)(j-1)</li>
        /// <li>x(i+1), y(j-1), z(i+1)(j-1)</li>
        /// <li>x(i+1), y(j+1), z(i+1)(j+1)</li>
        /// </ul>
        /// There are thus three types of polygons:
        /// <ul>
        /// <li>those that stand completely inside the ringMin and ringMax radius and
        /// that have the previous coordinates.</li>
        /// <li>those that stand completely outside the ringMin and ringMax radius and
        /// that won't be added to the list of polygons.</li>
        /// <li>those that have some points in and some points out of the ringMin and
        /// ringMax radius. These polygons are recomputed so that "out" points are replaced
        /// by two points that make the smooth contour. According to the number of "out"
        /// points, the modified polygon will gather 3, 4, or 5 points.</li>
        /// </ul>
        /// <br/>
        /// As a consequence, it is suggested to provide data ranging outside of ringMin
        /// and ringMax, in order to be sure to have a perfect round surface.
        /// </summary>
        public List<Polygon> GetInterpolatedRingPolygons()
        {
            var polygons = new List<Polygon>();
            for (int xi = 0; xi <= X.Length - 2; xi++)
            {
                for (int yi = 0; yi <= Y.Length - 2; yi++)
                {
                    // Compute points surrounding current point
                    Point[] p = GetRealQuadStandingOnPoint(xi, yi);
                    p[0].Color = _cmap.Color(p[0].XYZ);
                    p[1].Color = _cmap.Color(p[1].XYZ);
                    p[2].Color = _cmap.Color(p[2].XYZ);
                    p[3].Color = _cmap.Color(p[3].XYZ);
                    p[0].Rgb.mul(_factor);
                    p[1].Rgb.mul(_factor);
                    p[2].Rgb.mul(_factor);
                    p[3].Rgb.mul(_factor);

                    float[] radius = new float[p.Length];

                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        radius[i] = Radius2d(p[i]);
                    }

                    // Compute status of each point according to there radius, or NaN status
                    bool[] isIn = IsInside(p, radius, _ringMin, _ringMax);

                    // Ignore polygons that are out
                    if ((!isIn[0]) && (!isIn[1]) && (!isIn[2]) && (!isIn[3]))
                    {
                        continue;
                    }

                    if (isIn[0] && isIn[1] && isIn[2] && isIn[3])
                    {
                        // Directly store polygons that have non NaN values for all points
                        var quad = new Polygon();
                        for (int pi = 0; pi <= p.Length - 1; pi++)
                        {
                            quad.Add(p[pi]);
                        }
                        polygons.Add(quad);
                    }
                    else
                    {
                        // Partly inside: generate points that intersect a radius
                        var polygon = new Polygon();
                        int[] seq = { 0, 1, 2, 3, 0 };
                        bool[] done = new bool[4];

                        for (int pi = 0; pi <= done.Length - 1; pi++)
                        {
                            done[pi] = false;
                        }

                        // Handle all square edges and shift "out" points
                        for (int s = 0; s <= seq.Length - 2; s++)
                        {
                            Point intersection;
                            // generated point
                            float ringRadius;

                            // Case of point s "in" and point s+1 "in"
                            if (isIn[seq[s]] && isIn[seq[s + 1]])
                            {
                                if (!done[seq[s]])
                                {
                                    polygon.Add(p[seq[s]]);
                                    done[seq[s]] = true;
                                }

                                if (!done[seq[s + 1]])
                                {
                                    polygon.Add(p[seq[s + 1]]);
                                    done[seq[s + 1]] = true;
                                }
                            }
                            else if (isIn[seq[s]] && (!isIn[seq[s + 1]]))
                            {
                                // Case of point s "in" and point s+1 "out"
                                if (!done[seq[s]])
                                {
                                    polygon.Add(p[seq[s]]);
                                    done[seq[s]] = true;
                                }
                                // Select the radius on which the point is supposed to stand
                                if (Math.Abs(radius[seq[s + 1]] - _ringMin) < Math.Abs(radius[seq[s + 1]] - _ringMax))
                                {
                                    ringRadius = _ringMin;
                                }
                                else
                                {
                                    ringRadius = _ringMax;
                                }

                                // Generate a point on the circle that replaces s+1
                                intersection = FindPoint(p[seq[s]], p[seq[s + 1]], ringRadius);
                                intersection.Color = _cmap.Color(intersection.XYZ);
                                intersection.Rgb.mul(_factor);
                                polygon.Add(intersection);
                            }
                            else if ((!isIn[seq[s]]) && isIn[seq[s + 1]])
                            {
                                //Case of point s "out" and point s+1 "in"
                                // Select the radius on which the point is supposed to stand
                                if (Math.Abs(radius[seq[s + 1]] - _ringMin) < Math.Abs(radius[seq[s + 1]] - _ringMax))
                                {
                                    ringRadius = _ringMin;
                                }
                                else
                                {
                                    ringRadius = _ringMax;
                                }

                                // Generate a point on the circle that replaces s
                                intersection = FindPoint(p[seq[s]], p[seq[s + 1]], ringRadius);
                                intersection.Color = _cmap.Color(intersection.XYZ);
                                intersection.Rgb.mul(_factor);
                                polygon.Add(intersection);

                                if (!done[seq[s + 1]])
                                {
                                    polygon.Add(p[seq[s + 1]]);
                                    done[seq[s + 1]] = true;
                                }
                            }
                            // end case 3
                        }
                        // end polygon construction loop
                        polygons.Add(polygon);
                    }
                    // end switch quad/polygon
                }
                // end for y
            }
            // end for x	
            return polygons;
        }

        /// <summary>
        /// Indicates which point lies inside and outside the given min and max radius.
        /// </summary>
        internal static bool[] IsInside(Point[] p, float[] radius, float minRadius, float maxRadius)
        {
            bool[] isIn = new bool[4];
            isIn[0] = (!double.IsNaN(p[0].XYZ.Z)) && radius[0] < maxRadius && radius[0] >= minRadius;
            isIn[1] = (!double.IsNaN(p[1].XYZ.Z)) && radius[1] < maxRadius && radius[1] >= minRadius;
            isIn[2] = (!double.IsNaN(p[2].XYZ.Z)) && radius[2] < maxRadius && radius[2] >= minRadius;
            isIn[3] = (!double.IsNaN(p[3].XYZ.Z)) && radius[3] < maxRadius && radius[3] >= minRadius;
            return isIn;
        }

        internal static float Radius2d(Point p)
        {
            return (float)Math.Sqrt(p.XYZ.X * p.XYZ.X + p.XYZ.Y * p.XYZ.Y);
        }

        /// <summary>
        /// Return a point that is the intersection between a segment and a circle
        /// Throws ArithmeticException if points do not stand on an squared (orthonormal) grid.
        /// </summary>
        private Point FindPoint(Point p1, Point p2, float ringRadius)
        {
            double x3, y3, z3, w1, w2, alpha;

            // We know that the seeked point is on a horizontal or vertial line

            //We know x3 and radius and seek y3, using intermediate alpha
            if (p1.XYZ.X == p2.XYZ.X)
            {
                x3 = p1.XYZ.X;
                alpha = Math.Acos(x3 / ringRadius);
                if (p1.XYZ.Y < 0 && p2.XYZ.Y < 0)
                {
                    y3 = -Math.Sin(alpha) * ringRadius;
                }
                else if (p1.XYZ.Y > 0 && p2.XYZ.Y > 0)
                {
                    y3 = Math.Sin(alpha) * ringRadius;
                }
                else if (p1.XYZ.Y == -p2.XYZ.Y)
                {
                    y3 = 0;
                    // ne peut pas arriver
                }
                else
                {
                    throw new ArithmeticException("no alignement between p1(" + p1.XYZ.X + "," + p1.XYZ.Y + "," + p1.XYZ.Z + ") and p2(" + p2.XYZ.X + "," + p2.XYZ.Y + "," + p2.XYZ.Z + ")");
                }

                // and now get z3
                if ((!double.IsNaN(p1.XYZ.Z)) && double.IsNaN(p2.XYZ.Z))
                {
                    z3 = p1.XYZ.Z;
                }
                else if (double.IsNaN(p1.XYZ.Z) && (!double.IsNaN(p2.XYZ.Z)))
                {
                    z3 = p2.XYZ.Z;
                }
                else if ((!double.IsNaN(p1.XYZ.Z)) && (!double.IsNaN(p2.XYZ.Z)))
                {
                    w2 = (Math.Sqrt((x3 - p1.XYZ.X) * (x3 - p1.XYZ.X) + (y3 - p1.XYZ.Y) * (y3 - p1.XYZ.Y)) / Math.Sqrt((p2.XYZ.X - p1.XYZ.X) * (p2.XYZ.X - p1.XYZ.X) + (p2.XYZ.Y - p1.XYZ.Y) * (p2.XYZ.Y - p1.XYZ.Y)));
                    w1 = 1 - w2;
                    z3 = w1 * p1.XYZ.Z + w2 * p2.XYZ.Z;
                }
                else
                {
                    throw new ArithmeticException("can't compute z3 with p1(" + p1.XYZ.X + "," + p1.XYZ.Y + ") and p2(" + p2.XYZ.X + "," + p2.XYZ.Y + ")");
                }
                // We know y3 and radius and seek x3, using intermediate alpha
            }
            else if (p1.XYZ.Y == p2.XYZ.Y)
            {
                y3 = p1.XYZ.Y;
                alpha = Math.Asin(y3 / ringRadius);
                if (p1.XYZ.X < 0 && p2.XYZ.X < 0)
                {
                    x3 = -Math.Cos(alpha) * ringRadius;
                }
                else if (p1.XYZ.X > 0 && p2.XYZ.X > 0)
                {
                    x3 = Math.Cos(alpha) * ringRadius;
                }
                else if (p1.XYZ.X == -p2.XYZ.X)
                {
                    x3 = 0;
                    // ne peut pas arriver
                }
                else
                {
                    throw new ArithmeticException("no alignement between p1(" + p1.XYZ.X + "," + p1.XYZ.Y + "," + p1.XYZ.Z + ") and p2(" + p2.XYZ.X + "," + p2.XYZ.Y + "," + p2.XYZ.Z + ")");
                }

                // and now get z3
                if ((!double.IsNaN(p1.XYZ.Z)) && double.IsNaN(p2.XYZ.Z))
                {
                    z3 = p1.XYZ.Z;
                }
                else if (double.IsNaN(p1.XYZ.Z) && (!double.IsNaN(p2.XYZ.Z)))
                {
                    z3 = p2.XYZ.Z;
                }
                else if ((!double.IsNaN(p1.XYZ.Z)) && (!double.IsNaN(p2.XYZ.Z)))
                {
                    w2 = (Math.Sqrt((x3 - p1.XYZ.X) * (x3 - p1.XYZ.X) + (y3 - p1.XYZ.Y) * (y3 - p1.XYZ.Y)) / Math.Sqrt((p2.XYZ.X - p1.XYZ.X) * (p2.XYZ.X - p1.XYZ.X) + (p2.XYZ.Y - p1.XYZ.Y) * (p2.XYZ.Y - p1.XYZ.Y)));
                    w1 = 1 - w2;
                    z3 = w1 * p1.XYZ.Z + w2 * p2.XYZ.Z;
                }
                else
                {
                    throw new ArithmeticException("can't compute z3 with p1(" + p1.XYZ.X + "," + p1.XYZ.Y + ") and p2(" + p2.XYZ.X + "," + p2.XYZ.Y + ")");
                }
            }
            else
            {
                throw new ArithmeticException("no alignement between p1(" + p1.XYZ.X + "," + p1.XYZ.Y + ") and p2(" + p2.XYZ.X + "," + p2.XYZ.Y + ")");
            }
            return new Point(new Coord3d(x3, y3, z3));
        }
    }
}
