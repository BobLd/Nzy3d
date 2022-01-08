using Nzy3d.Maths;
using Xunit;

namespace Nzy3d.Tests.Maths
{
    public class TestCoord2d
    {
        [Fact]
        public void EqualsTests()
        {
            Assert.True(new Coord2d(1, 2).Equals(new Coord2d(1, 2)));
            Assert.True(new Coord2d(1, 2).GetHashCode().Equals(new Coord2d(1, 2).GetHashCode()));

            Assert.False(new Coord2d(1, 2).Equals(new Coord2d(2, 1)));
            Assert.False(new Coord2d(1, 2).GetHashCode().Equals(new Coord2d(2, 1).GetHashCode()));

            Assert.True(new Coord2d(-0, 2).Equals(new Coord2d(0, 2)));
            Assert.True(new Coord2d(-0, 2).GetHashCode().Equals(new Coord2d(0, 2).GetHashCode()));

            Assert.True(new Coord2d(float.NaN, 2).Equals(new Coord2d(float.NaN, 2)));
            Assert.True(new Coord2d(float.NaN, 2).GetHashCode().Equals(new Coord2d(float.NaN, 2).GetHashCode()));

            Assert.True(new Coord2d(float.PositiveInfinity, 2).Equals(new Coord2d(float.PositiveInfinity, 2)));
            Assert.True(new Coord2d(float.PositiveInfinity, 2).GetHashCode().Equals(new Coord2d(float.PositiveInfinity, 2).GetHashCode()));

            Assert.True(new Coord2d(float.NegativeInfinity, 2).Equals(new Coord2d(float.NegativeInfinity, 2)));
            Assert.True(new Coord2d(float.NegativeInfinity, 2).GetHashCode().Equals(new Coord2d(float.NegativeInfinity, 2).GetHashCode()));

            Assert.False(new Coord2d(float.PositiveInfinity, 2).Equals(new Coord2d(float.NegativeInfinity, 2)));
            Assert.False(new Coord2d(float.PositiveInfinity, 2).GetHashCode().Equals(new Coord2d(float.NegativeInfinity, 2).GetHashCode()));
        }
    }
}
