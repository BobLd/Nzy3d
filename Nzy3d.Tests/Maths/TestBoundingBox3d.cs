using Nzy3d.Maths;
using Xunit;

namespace Nzy3d.Tests.Maths
{
    public class TestBoundingBox3d
    {
        [Fact]
        public void intersect()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);

            Assert.True(bb.Intersect(new BoundingBox3d(10, 11, 10, 11, 10, 11))); // "Intersect if at least touch all components",
            Assert.True(bb.Intersect(new BoundingBox3d(9, 11, 9, 11, 9, 11))); // "Intersect if all component intersect", 
            Assert.True(bb.Intersect(new BoundingBox3d(-1, 1, -1, 1, -1, 1))); // "Intersect if all component intersect", 

            Assert.False(bb.Intersect(new BoundingBox3d(10, 11, 100, 110, 100, 110))); // "No intersect if only X component 1", 
            Assert.False(bb.Intersect(new BoundingBox3d(8, 12, 100, 110, 100, 110))); // "No intersect if only X component 2",

            Assert.False(bb.Intersect(new BoundingBox3d(10, 11, 10, 11, 100, 110))); // "No intersect if only X, Y component 1", 
            Assert.False(bb.Intersect(new BoundingBox3d(8, 12, 10, 11, 100, 110))); // "No intersect if only X, Y component 2",

            Assert.False(bb.Intersect(new BoundingBox3d(11, 12, 100, 110, 100, 110))); // "No intersect if no component", 
        }

        [Fact]
        public void containsBounds()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);

            Assert.False(bb.Contains(new BoundingBox3d(9, 11, 9, 11, 9, 11)));
            Assert.True(bb.Contains(new BoundingBox3d(8, 10, 8, 10, 8, 10)));
        }

        [Fact]
        public void containsCoord()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);

            Assert.True(bb.Contains(new Coord3d(5, 5, 5)));
            Assert.False(bb.Contains(new Coord3d(15, 15, 15)));
            Assert.False(bb.Contains(new Coord3d(-5, -5, -5)));
            Assert.False(bb.Contains(new Coord3d(5, 5, 15)));
        }

        [Fact]
        public void margin()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);
            BoundingBox3d b2 = bb.Margin(1);

            //float delta = 0.00001f;
            int delta = 5;

            Assert.Equal(11f, b2.XMax, delta);
            Assert.Equal(-1f, b2.XMin, delta);
            Assert.Equal(11f, b2.YMax, delta);
            Assert.Equal(-1f, b2.YMin, delta);
            Assert.Equal(11f, b2.ZMax, delta);
            Assert.Equal(-1f, b2.ZMin, delta);
        }

        [Fact]
        public void marginRatio()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);
            BoundingBox3d b2 = bb.MarginRatio(0.1f);

            //float delta = 0.00001f;
            int delta = 5;

            Assert.Equal(11f, b2.XMax, delta);
            Assert.Equal(-1f, b2.XMin, delta);
            Assert.Equal(11f, b2.YMax, delta);
            Assert.Equal(-1f, b2.YMin, delta);
            Assert.Equal(11f, b2.ZMax, delta);
            Assert.Equal(-1f, b2.ZMin, delta);
        }

        [Fact]
        public void marginSelf()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);
            bb.SelfMargin(1);

            //float delta = 0.00001f;
            int delta = 5;

            Assert.Equal(11f, bb.XMax, delta);
            Assert.Equal(-1f, bb.XMin, delta);
            Assert.Equal(11f, bb.YMax, delta);
            Assert.Equal(-1f, bb.YMin, delta);
            Assert.Equal(11f, bb.ZMax, delta);
            Assert.Equal(-1f, bb.ZMin, delta);
        }

        [Fact]
        public void marginRatioSelf()
        {
            BoundingBox3d bb = new BoundingBox3d(0, 10, 0, 10, 0, 10);
            bb.SelfMarginRatio(0.1f);

            //float delta = 0.00001f;
            int delta = 5;

            Assert.Equal(11f, bb.XMax, delta);
            Assert.Equal(-1f, bb.XMin, delta);
            Assert.Equal(11f, bb.YMax, delta);
            Assert.Equal(-1f, bb.YMin, delta);
            Assert.Equal(11f, bb.ZMax, delta);
            Assert.Equal(-1f, bb.ZMin, delta);
        }
    }
}
