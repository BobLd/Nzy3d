using Nzy3d.Chart.Controllers.Mouse.Camera;
using Nzy3d.Chart.Controllers.Thread.Camera;
using Nzy3d.Plot3D.Primitives.Axes.Layout;
using Nzy3d.Plot3D.Rendering.Canvas;

namespace Nzy3d.WinformsDemo
{
    public partial class Form1 : Form
    {
        private CameraThreadController t;
        private IAxeLayout axeLayout;
        public Form1()
        {
            InitializeComponent();
        }
        private void InitRenderer()
        {
            // Create the Renderer 3D control.
            //Renderer3D myRenderer3D = new Renderer3D();

            // Add the Renderer control to the panel
            // mainPanel.Controls.Clear();
            //mainPanel.Controls.Add(myRenderer3D);

            // Create the chart and embed the surface within
            Chart.Chart chart = new Chart.Chart(myRenderer3D, Quality.Nicest);

            chart.Scene.Graph.Add(Graphs.GetGroupedLineScatter());// .GetScatterGraph());
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
            mouse.addControllerEventListener(myRenderer3D);
            chart.AddController(mouse);

            // This is just to ensure code is reentrant (used when code is not called in Form_Load but another reentrant event)
            DisposeBackgroundThread();

            // Create a thread to control the camera based on mouse movements
            t = new CameraThreadController();
            t.addControllerEventListener(myRenderer3D);
            mouse.addSlaveThreadController(t);
            chart.AddController(t);
            t.Start();

            // Associate the chart with current control
            myRenderer3D.SetView(chart.View);

            this.Refresh();
        }

        private void DisposeBackgroundThread()
        {
            t?.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeBackgroundThread();
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {
            myRenderer3D.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            InitRenderer();
        }
    }
}