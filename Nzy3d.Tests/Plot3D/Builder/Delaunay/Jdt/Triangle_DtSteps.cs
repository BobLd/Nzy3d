using Nzy3d.Plot3D.Builder.Delaunay.Jdt;
using Xunit;

/*
 Scenario: Triangle setup in counterclockwise
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(1.0,0.0)-(0.0,1.0)
	Then the triangle has coordinates (0.0,0.0)-(1.0,0.0)-(0.0,1.0)
	
Scenario: Triangle setup in clockwise
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	Then the triangle has coordinates (0.0,0.0)-(1.0,0.0)-(0.0,1.0)

Scenario: Point inside triangle
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	And I have a point with coordinates (0.25,0.25)
	Then The point is inside the triangle

Scenario: Point outside triangle
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	And I have a point with coordinates (0.75,0.75)
	Then The point is outside the triangle

Scenario: Point on the boundary of triangle
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	And I have a point with coordinates (0.5,0.5)
	Then The point is on the boundary of the triangle
 */

namespace Nzy3d.Tests.Plot3D.Builder.Delaunay.Jdt
{
    public class Triangle_DtSteps
    {
        [Fact]
        public void TriangleCoordinatesCounterClockwiseTest()
        {
            const float p0 = 0.0f;
            const float p1 = 0.0f;
            const float p2 = 1.0f;
            const float p3 = 0.0f;
            const float p4 = 0.0f;
            const float p5 = 1.0f;

            // (0.0,0.0)-(1.0,0.0)-(0.0,1.0)
            var triangle = new Triangle_dt(new Point_dt(0.0f, 0.0f),
                                           new Point_dt(1.0f, 0.0f),
                                           new Point_dt(0.0f, 1.0f));
            Assert.Equal(p0, triangle.A.X);
            Assert.Equal(p1, triangle.A.Y);
            Assert.Equal(p2, triangle.B.X);
            Assert.Equal(p3, triangle.B.Y);
            Assert.Equal(p4, triangle.C.X);
            Assert.Equal(p5, triangle.C.Y);
        }

        [Fact]
        public void TriangleCoordinatesClockwiseTest()
        {
            const float p0 = 0.0f;
            const float p1 = 0.0f;
            const float p2 = 1.0f;
            const float p3 = 0.0f;
            const float p4 = 0.0f;
            const float p5 = 1.0f;

            // (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
            var triangle = new Triangle_dt(new Point_dt(0.0f, 0.0f),
                                           new Point_dt(0.0f, 1.0f),
                                           new Point_dt(1.0f, 0.0f));
            Assert.Equal(p0, triangle.A.X);
            Assert.Equal(p1, triangle.A.Y);
            Assert.Equal(p2, triangle.B.X);
            Assert.Equal(p3, triangle.B.Y);
            Assert.Equal(p4, triangle.C.X);
            Assert.Equal(p5, triangle.C.Y);
        }

        [Fact]
        public void PointInsideTriangle()
        {
            var triangle = new Triangle_dt(new Point_dt(0.0f, 0.0f),
                                           new Point_dt(0.0f, 1.0f),
                                           new Point_dt(1.0f, 0.0f));
            Assert.True(triangle.Contains(new Point_dt(0.25f, 0.25f)));
        }

        [Fact]
        public void PointOutsideTriangle()
        {
            var triangle = new Triangle_dt(new Point_dt(0.0f, 0.0f),
                                           new Point_dt(0.0f, 1.0f),
                                           new Point_dt(1.0f, 0.0f));
            Assert.False(triangle.Contains(new Point_dt(0.75f, 0.75f)));
        }

        [Fact]
        public void PointOnBoundaryTriangle()
        {
            var triangle = new Triangle_dt(new Point_dt(0.0f, 0.0f),
                                           new Point_dt(0.0f, 1.0f),
                                           new Point_dt(1.0f, 0.0f));

            var point = new Point_dt(0.5f, 0.5f);
            Assert.True(triangle.Contains(point));
            Assert.False(triangle.ContainsBoundaryIsOutside(point), "Point is inside the triangle, boundary excluded");
        }
    }
}
