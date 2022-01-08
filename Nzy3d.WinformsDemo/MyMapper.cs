using Nzy3d.Plot3D.Builder;

namespace Nzy3d.WinformsDemo
{
    class MyMapper : Mapper
    {
        public override double f(double x, double y)
        {
            return 10 * Math.Sin(x / 10) * Math.Cos(y / 20) * x;
        }
    }
}
