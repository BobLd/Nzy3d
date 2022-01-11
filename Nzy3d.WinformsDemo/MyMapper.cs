using Nzy3d.Plot3D.Builder;
using static System.Math;

namespace Nzy3d.WinformsDemo
{
    //https://www.geeks3d.com/20130702/3d-surfaces-plots-equations-examples-glslhacker-demo-opengl-glsl/3/
    internal class MyMapper : Mapper
    {
        public override double f(double x, double y)
        {
            return Ripples(x, y);
        }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration.
        private static double Default(double x, double y)
        {
            return 10 * Sin(x / 10) * Cos(y / 20) * x;
        }

        /// <summary>
        /// Function 010
        /// </summary>
        private static double Cuve(double x, double y, double a = 1f, double b = 1f, double c = 1f)
        {
            return (x * x * a + y * y * b) * c;
        }

        /// <summary>
        /// Function 008
        /// </summary>
        private static double Islands(double x, double y, double a = 0.05f, double b = 0.05f)
        {
            return Log(Sin(x * a)) + Log(Sin(y * b));
        }

        /// <summary>
        /// Function 007
        /// </summary>
        private static double SquareCubicCurve(double x, double y, double a = 0.0000005f, double b = 0.0000005f)
        {
            return (x * x * y * y * y * b) - (y * y * x * x * x * a);
        }

        private static double MountHole(double x, double z, double a = 0.0005f, double b = 0.0005f)
        {
            double r = Sqrt(a * x * x + b * z * z);
            return Sin(x * x + 0.1f * z * z * 2f) / (0.1f + r * r) + (x * x + 1.9f * z * z) * Exp(1 - r * r) / 4.0f;
        }

        private static double Spikes(double x, double z)
        {
            return Exp(Sin(x * 2f) * Sin(z * 0.2f)) * 0.9f * Exp(Sin(z * 2f) * Sin(x * 0.2f)) * 0.9f;
        }

        private static double Cone(double x, double y, double a = 0.1f)
        {
            return Sqrt(x * x + y * y) * a;
        }

        private static double Ripples(double x, double y, double a = 0.1f)
        {
            var r = Cone(x, y, a);
            return Sin(r) / r;
        }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore RCS1213 // Remove unused member declaration.
    }
}
