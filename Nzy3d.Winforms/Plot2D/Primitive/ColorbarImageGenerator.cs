using Nzy3d.Colors;
using Nzy3d.Colors.ColorMaps;
using Nzy3d.Plot3D.Primitives.Axes.Layout.Providers;
using Nzy3d.Plot3D.Primitives.Axes.Layout.Renderers;
using Color = Nzy3d.Colors.Color;

namespace Nzy3d.Winforms.Plot2D.Primitive
{
    public class ColorbarImageGenerator
	{
		internal ColorMapper _mapper;
		internal ITickProvider _provider;
		internal ITickRenderer _renderer;
		internal float _min;
		internal float _max;
		internal bool _hasBackground = false;
		internal Color _backgroundColor;
		internal Color _foregroundColor = Color.BLACK;
		public static int MIN_BAR_WIDTH = 100;

		public static int MIN_BAR_HEIGHT = 100;

		public ColorbarImageGenerator(IColorMap map, float min, float max, ITickProvider provider, ITickRenderer renderer)
		{
			_mapper = new ColorMapper(map, min, max);
			_min = min;
			_max = max;
			_provider = provider;
			_renderer = renderer;
		}

		public ColorbarImageGenerator(ColorMapper mapper, ITickProvider provider, ITickRenderer renderer)
			: this(mapper.ColorMap, (float)mapper.ZMin, (float)mapper.ZMax, provider, renderer)
		{
		}

		public Bitmap ToImage(int width, int height)
		{
			return ToImage(width, height, 20);
		}

		public Bitmap ToImage(int width, int height, int barWidth)
		{
			if (barWidth > width)
			{
				return null;
			}
			// Init image output
			Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Graphics graphic = Graphics.FromImage(image);
			int txtSize = 12;
			// Draw background
			if (_hasBackground)
			{
				graphic.FillRectangle(new SolidBrush(_backgroundColor.ToColor()), 0, 0, width, height);
			}
			// Draw colorbar centering in half the Legend text height
			for (int h = txtSize / 2; h <= (height - txtSize / 2); h++)
			{
				// Compute value & color
				float v = _min + (_max - _min) * h / (height - txtSize);
				//			Color c = mapper.getColor(new Coord3d(0,0,v));
				Color c = _mapper.Color(v);
				//To allow the Color to be a variable independent of the coordinates
				// Draw line
				graphic.DrawLine(new Pen(new SolidBrush(c.ToColor())), 0, height - h, barWidth, height - h);
			}
			// Contour of bar
			graphic.FillRectangle(new SolidBrush(_foregroundColor.ToColor()), 0, Convert.ToSingle(txtSize / 2), barWidth, height - txtSize);
			// Text annotation
			if (_provider != null)
			{
				float[] ticks = _provider.GenerateTicks(_min, _max);
                for (int t = 0; t <= ticks.Length - 1; t++)
				{
                    // ypos = (int)(height-height*((ticks[t]-min)/(max-min)));
                    float ypos = txtSize + (height - txtSize - (height - txtSize) * ((ticks[t] - _min) / (_max - _min)));
                    //Making sure that the first and last tick appear in the colorbar
                    string txt = _renderer.Format(ticks[t]);
                    graphic.DrawString(txt, new Font("Arial", txtSize, GraphicsUnit.Pixel), new SolidBrush(_foregroundColor.ToColor()), barWidth + 1, ypos);
				}
			}
			return image;
		}

		public bool HasBackground
		{
			get { return _hasBackground; }
			set { _hasBackground = value; }
		}

		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { _backgroundColor = value; }
		}

		public Color ForegroundColor
		{
			get { return _foregroundColor; }
			set { _foregroundColor = value; }
		}
	}
}
