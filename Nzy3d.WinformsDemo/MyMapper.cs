using Nzy3d.Plot3D.Builder;
using static System.MathF;

namespace Nzy3d.WinformsDemo
{
    //https://www.geeks3d.com/20130702/3d-surfaces-plots-equations-examples-glslhacker-demo-opengl-glsl/3/
    internal class MyMapper : Mapper
    {
        public override float f(float x, float y)
        {
            return SquareCubicCurve(x, y);
        }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration.
        private static float Original(float x, float y)
        {
            return 10 * Sin(x / 10) * Cos(y / 20) * x;
        }

        /// <summary>
        /// Function 010
        /// </summary>
        private static float Cuve(float x, float y, float a = 1f, float b = 1f, float c = 1f)
        {
            return (x * x * a + y * y * b) * c;
        }

        /// <summary>
        /// Function 008
        /// </summary>
        private static float Islands(float x, float y, float a = 0.05f, float b = 0.05f)
        {
            return Log(Sin(x * a)) + Log(Sin(y * b));
        }

        /// <summary>
        /// Function 007
        /// </summary>
        private static float SquareCubicCurve(float x, float y, float a = 0.0000005f, float b = 0.0000005f)
        {
            return (x * x * y * y * y * b) - (y * y * x * x * x * a);
        }

        private static float MountHole(float x, float z, float a = 0.0005f, float b = 0.0005f)
        {
            float r = Sqrt(a * x * x + b * z * z);
            return Sin(x * x + 0.1f * z * z * 2f) / (0.1f + r * r) + (x * x + 1.9f * z * z) * Exp(1 - r * r) / 4.0f;
        }

        private static float Spikes(float x, float z)
        {
            return Exp(Sin(x * 2f) * Sin(z * 0.2f)) * 0.9f * Exp(Sin(z * 2f) * Sin(x * 0.2f)) * 0.9f;
        }

        private static float Cone(float x, float y, float a = 0.1f)
        {
            return Sqrt(x * x + y * y) * a;
        }

        private static float Waves(float x, float y, float a = 0.1f)
        {
            var r = Cone(x, y, a);
            return Sin(r) / r;
        }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore RCS1213 // Remove unused member declaration.
    }
}
