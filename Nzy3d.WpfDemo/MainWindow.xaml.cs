using Nzy3d.Chart.Controllers.Thread.Camera;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using Nzy3d.Wpf.Chart.Controllers.Mouse.Camera;
using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nzy3d.WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CameraThreadController _cameraController;
        private IAxeLayout axeLayout;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //myRenderer3D.Profile = ContextProfile.Compatability;

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3,
                GraphicsProfile = OpenTK.Windowing.Common.ContextProfile.Compatability,
            };
            myRenderer3D.Start(settings);

            InitRenderer();
        }

        private void InitRenderer()
        {
            // ############ 1st Renderer ############

            //myRenderer3D.Name = "Renderer 0";
            // Create the chart
            Chart.Chart chart = ChartsHelper.GetIssue16(myRenderer3D); //GetFRB_H15_dec_2021(myRenderer3D);
            axeLayout = chart.AxeLayout;

            // All activated by default
            // TODO

            // Create a mouse control
            CameraMouseController mouse = new CameraMouseController();
            mouse.AddControllerEventListener(myRenderer3D);
            chart.AddController(mouse);

            // This is just to ensure code is reentrant (used when code is not called in Form_Load but another reentrant event)
            DisposeBackgroundThread();

            // Create a thread to control the camera based on mouse movements
            _cameraController = new CameraThreadController();
            _cameraController.AddControllerEventListener(myRenderer3D);
            mouse.AddSlaveThreadController(_cameraController);
            chart.AddController(_cameraController);
            _cameraController.Start();

            // Associate the chart with current control
            myRenderer3D.SetView(chart.View);

            // ############ 2nd Renderer ############

            //myRenderer3D_1.Name = "Renderer 1";

            //// Create the chart and embed the surface within
            //Chart.Chart chart1 = new Chart.Chart(myRenderer3D_1, Quality.Nicest);
            //chart1.View.Maximized = false;
            //chart1.View.CameraMode = CameraMode.PERSPECTIVE;

            //chart1.Scene.Graph.Add(GraphsHelper.GetScatterGraph(1_000_000));// .GetScatterGraph()); // GetSurfaceGraph
            //axeLayout1 = chart1.AxeLayout;

            //// All activated by default
            //DisplayXTicks = true;
            //DisplayXAxisLabel = true;
            //DisplayYTicks = true;
            //DisplayYAxisLabel = true;
            //DisplayZTicks = true;
            //DisplayZAxisLabel = true;
            //DisplayTickLines = true;

            //// Create a mouse control
            //CameraMouseController mouse1 = new CameraMouseController();
            //mouse1.AddControllerEventListener(myRenderer3D_1);
            //chart1.AddController(mouse1);

            //// This is just to ensure code is reentrant (used when code is not called in Form_Load but another reentrant event)
            //DisposeBackgroundThread1();

            //// Associate the chart with current control
            //myRenderer3D_1.SetView(chart1.View);

            //// Create a thread to control the camera based on mouse movements
            //_cameraController1 = new CameraThreadController();
            //_cameraController1.AddControllerEventListener(myRenderer3D_1);
            //mouse1.AddSlaveThreadController(_cameraController1);
            //chart1.AddController(_cameraController1);
            //_cameraController1.Start();

            this.Title = $"Running on {myRenderer3D.GetGpuInfo()}";

            //this.Refresh();
        }

        private void DisposeBackgroundThread()
        {
            _cameraController?.Dispose();
        }
    }
}
