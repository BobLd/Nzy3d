using Nzy3d.Plot3D.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
