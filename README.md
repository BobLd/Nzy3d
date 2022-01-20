# Nzy3d

- Updated net6.0 version of [nzy3d-api](https://github.com/benoit74/nzy3d-api), uses OpenTK 4.6.7
- Perspective mode bug fixed
- Available in Winforms and *native* WPF (WFP available in the [wpf branch](https://github.com/BobLd/Nzy3d/tree/wpf) due to version issue with the [OpenTK control](https://github.com/opentk/GLWpfControl), see [issue 61](https://github.com/opentk/GLWpfControl/issues/61))

# How to


## Samples
See [Nzy3d.WinformsDemo](https://github.com/BobLd/Nzy3d/tree/master/Nzy3d.WinformsDemo) and the [ChartsHelper](https://github.com/BobLd/Nzy3d/blob/master/Nzy3d.WinformsDemo/ChartsHelper.cs) for Winforms examples 

## Mapper surface
```csharp
// Create the chart
Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
chart.View.Maximized = false;
chart.View.CameraMode = CameraMode.PERSPECTIVE;
chart.View.IncludingTextLabels = true;

// Create a range for the graph generation
var range = new Maths.Range(-150, 150);
const int steps = 50;

// Build a nice surface to display with cool alpha colors 
// (alpha 0.8 for surface color and 0.5 for wireframe)
var surface = Plot3D.Builder.Builder.BuildOrthonomal(new OrthonormalGrid(range, steps, range, steps), new MyMapper());
surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
surface.FaceDisplayed = true;
surface.WireframeDisplayed = true;
surface.WireframeColor = Color.CYAN;
surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));

// Add surface to chart
chart.Scene.Graph.Add(surface);
```
![surface_ripples](https://github.com/BobLd/Nzy3d/blob/master/resources/Nzy3d-ripples_small.gif)

## Delaunay surface
```csharp
// Create data
const int size = 100;
List<Coord3d> coords = new List<Coord3d>(size);
float x, y, z;
var r = new Random(0);

for (int i = 0; i < size; i++)
{
	x = r.NextSingle() - 0.5f;
	y = r.NextSingle() - 0.5f;
	z = r.NextSingle() - 0.5f;
	coords.Add(new Coord3d(x, y, z));
}

// Create chart
Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
chart.View.Maximized = false;
chart.View.CameraMode = CameraMode.PERSPECTIVE;
chart.View.IncludingTextLabels = true;

// Create surface
var surface = Plot3D.Builder.Builder.BuildDelaunay(coords);
surface.ColorMapper = new ColorMapper(new ColorMapRainbow(), surface.Bounds.ZMin, surface.Bounds.ZMax, new Color(1, 1, 1, 0.8));
surface.FaceDisplayed = true;
surface.WireframeDisplayed = true;
surface.WireframeColor = Color.CYAN;
surface.WireframeColor.Mul(new Color(1, 1, 1, 0.5));

// Add surface to chart
chart.Scene.Graph.Add(surface);
```

## Scatter graph
![scatter_1million](https://github.com/BobLd/Nzy3d/blob/master/resources/Nzy3d-scatter_1million.png)
```csharp
// Create data
var points = new Coord3d[size];
var colors = new Color[size];

float x, y, z;
const float a = 0.25f;

var r = new Random(0);
for (int i = 0; i < size; i++)
{
	x = r.NextSingle() - 0.5f;
	y = r.NextSingle() - 0.5f;
	z = r.NextSingle() - 0.5f;
	points[i] = new Coord3d(x, y, z);
	colors[i] = new Color(x, y, z, a);
}

// Create chart
Chart.Chart chart = new Chart.Chart(renderer3D, Quality.Nicest);
chart.View.Maximized = false;
chart.View.CameraMode = CameraMode.PERSPECTIVE;
chart.View.IncludingTextLabels = true;

// Create scatter
var scatter = new Scatter(points, colors);

// Add surface to chart
chart.Scene.Graph.Add(scatter);
```

## References
- https://github.com/benoit74/nzy3d-api
- https://github.com/jzy3d/jzy3d-api/
