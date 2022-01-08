using Nzy3d.Colors;
using Xunit;

namespace Nzy3d.Tests.Colors
{
    public class TestColor
    {
        [Fact]
        public void testColorDistance()
        {
            //double delta = 0.000001;
            int delta = 6;

            Assert.Equal(3.0, Color.BLACK.DistanceSq(Color.WHITE), delta);

            Assert.Equal(1.0, Color.BLACK.DistanceSq(Color.RED), delta);
            Assert.Equal(1.0, Color.BLACK.DistanceSq(Color.GREEN), delta);
            Assert.Equal(1.0, Color.BLACK.DistanceSq(Color.BLUE), delta);

            Assert.Equal(2.0, Color.WHITE.DistanceSq(Color.RED), delta);
            Assert.Equal(2.0, Color.WHITE.DistanceSq(Color.GREEN), delta);
            Assert.Equal(2.0, Color.WHITE.DistanceSq(Color.BLUE), delta);

            Assert.Equal(2.0, Color.RED.DistanceSq(Color.BLUE), delta);
            Assert.Equal(2.0, Color.RED.DistanceSq(Color.GREEN), delta);
            Assert.Equal(2.0, Color.BLUE.DistanceSq(Color.GREEN), delta);
        }
    }
}
