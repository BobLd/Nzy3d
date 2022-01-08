using Nzy3d.Chart;
using Nzy3d.Events;
using Nzy3d.Plot3D.Primitives;
using Nzy3d.Plot3D.Rendering.View;

namespace Nzy3d.Plot3D.Rendering.Legends
{
    /// <summary>
    /// <para>
    /// A <see cref="Legend"/> represent information concerning a <see cref="AbstractDrawable"/> that may be
    /// displayed as a metadata in the <see cref="ChartView"/>.
    /// </para>
    /// <para>
    /// The constructor of a <see cref="Legend"/> registers itself as listener of its
    /// parent <see cref="AbstractDrawable"/>, and unregister itself when it is disposed.
    /// </para>
    /// <para>
    /// When defining a concrete <see cref="Legend"/>, one should:
    /// <ul>
    /// <li>override the {@link toImage(int width, int height)} method, that defines the picture representation.</li>
    /// <li>override the {@link drawableChanged(DrawableChangedEvent e)} method, that must select events that actually triggers an image update.</li>
    /// </ul>
    /// </para>
    /// <para>
    /// Last, a <see cref="Legend"/> optimizes rendering by:
    /// <ul>
    /// <li>storing current image dimension,</li>
    /// <li>computing a new image only if the required <see cref="Legend"/> dimensions changed.</li>
    /// </ul>
    /// </para>
    /// <para>@author Martin Pernollet</para>
    /// </summary>
    public abstract class Legend : ImageViewport, IDrawableListener
	{
		internal AbstractDrawable _parent;
		public Legend(AbstractDrawable parent)
		{
			_parent = parent;
			_parent?.AddDrawableListener(this);
		}

		public void Dispose()
		{
			_parent?.RemoveDrawableListener(this);
		}

		//public abstract Bitmap toImage(int width, int height);
		public abstract void DrawableChanged(DrawableChangedEventArgs e);

		public override void SetViewPort(int width, int height, float left, float right)
		{
			base.SetViewPort(width, height, left, right);
			int imgWidth = (int)(width * (right - left));
			if (_imageWidth != imgWidth || _imageHeight != height)
			{
				//this.Image = toImage(imgWidth, height);
			}
		}

		/// <summary>
		/// Recompute the picture, using last used dimensions.
		/// </summary>
		public void UpdateImage()
		{
			//this.Image = toImage(_imageWidth, _imageHeight);
		}
	}
}
