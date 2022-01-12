using Nzy3d.Chart.Controllers.Mouse.Camera;
using Nzy3d.Chart.Controllers.Thread.Camera;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using OpenTK.Windowing.Common;

namespace Nzy3d.WinformsDemo
{
    public partial class Form1 : Form
    {
        //https://gamedev.stackexchange.com/questions/172170/multiple-glcontrol-on-same-winform-is-not-working-opentk-c
        //https://stackoverflow.com/questions/40578910/opentk-multiple-glcontrol-with-a-single-context

        private CameraThreadController _cameraController;
        private IAxeLayout axeLayout;

        private CameraThreadController _cameraController1;
        private IAxeLayout axeLayout1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myRenderer3D.Profile = ContextProfile.Compatability;
            //myRenderer3D_1.Profile = ContextProfile.Compatability;
            InitRenderer();
        }

        private void InitRenderer()
        {
            // ############ 1st Renderer ############

            myRenderer3D.Name = "Renderer 0";
            // Create the chart
            Chart.Chart chart = ChartsHelper.GetFRB_H15_dec_2021(myRenderer3D); //GetFRB_H15_dec_2021(myRenderer3D);
            axeLayout = chart.AxeLayout;

            // All activated by default
            DisplayXTicks = true;
            DisplayXAxisLabel = true;
            DisplayYTicks = true;
            DisplayYAxisLabel = true;
            DisplayZTicks = true;
            DisplayZAxisLabel = true;
            DisplayTickLines = true;

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

            this.Text = $"Running on {myRenderer3D.GetGpuInfo()}";

            this.Refresh();
        }

        private void DisposeBackgroundThread()
        {
            _cameraController?.Dispose();
        }

        private void DisposeBackgroundThread1()
        {
            _cameraController1?.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeBackgroundThread();
            DisposeBackgroundThread1();
        }

        #region AxeLayout changes
        private bool _DisplayTickLines;
        public bool DisplayTickLines
        {
            get
            {
                return _DisplayTickLines;
            }
            set
            {
                _DisplayTickLines = value;
                if (axeLayout != null)
                {
                    axeLayout.TickLineDisplayed = value;
                }
            }
        }

        private bool _DisplayXTicks;
        public bool DisplayXTicks
        {
            get
            {
                return _DisplayXTicks;
            }
            set
            {
                _DisplayXTicks = value;
                if (axeLayout != null)
                {
                    axeLayout.XTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayYTicks;
        public bool DisplayYTicks
        {
            get
            {
                return _DisplayYTicks;
            }
            set
            {
                _DisplayYTicks = value;
                if (axeLayout != null)
                {
                    axeLayout.YTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayZTicks;
        public bool DisplayZTicks
        {
            get
            {
                return _DisplayZTicks;
            }
            set
            {
                _DisplayZTicks = value;
                if (axeLayout != null)
                {
                    axeLayout.ZTickLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayXAxisLabel;
        public bool DisplayXAxisLabel
        {
            get
            {
                return _DisplayXAxisLabel;
            }
            set
            {
                _DisplayXAxisLabel = value;
                if (axeLayout != null)
                {
                    axeLayout.XAxeLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayYAxisLabel;
        public bool DisplayYAxisLabel
        {
            get
            {
                return _DisplayYAxisLabel;
            }
            set
            {
                _DisplayYAxisLabel = value;
                if (axeLayout != null)
                {
                    axeLayout.YAxeLabelDisplayed = value;
                }
            }
        }

        private bool _DisplayZAxisLabel;
        public bool DisplayZAxisLabel
        {
            get
            {
                return _DisplayZAxisLabel;
            }
            set
            {
                _DisplayZAxisLabel = value;
                if (axeLayout != null)
                {
                    axeLayout.ZAxeLabelDisplayed = value;
                }
            }
        }

        private void CheckBoxes_CheckedChanged(object sender, EventArgs e)
        {
            /*
            if (chkDisplayXTicks.Checked != DisplayXTicks)
            {
                DisplayXTicks = chkDisplayXTicks.Checked;
            }
            if (chkDisplayYTicks.Checked != DisplayYTicks)
            {
                DisplayYTicks = chkDisplayYTicks.Checked;
            }
            if (chkDisplayZTick.Checked != DisplayZTicks)
            {
                DisplayZTicks = chkDisplayZTick.Checked;
            }
            if (chkDisplayXAxisLabel.Checked != DisplayXAxisLabel)
            {
                DisplayXAxisLabel = chkDisplayXAxisLabel.Checked;
            }
            if (chkDisplayYAxisLabel.Checked != DisplayYAxisLabel)
            {
                DisplayYAxisLabel = chkDisplayYAxisLabel.Checked;
            }
            if (chkDisplayZAxisLabel.Checked != DisplayZAxisLabel)
            {
                DisplayZAxisLabel = chkDisplayZAxisLabel.Checked;
            }
            if (chkDisplayTickLines.Checked != DisplayTickLines)
            {
                DisplayTickLines = chkDisplayTickLines.Checked;
            }
            */
        }
#endregion
    }
}