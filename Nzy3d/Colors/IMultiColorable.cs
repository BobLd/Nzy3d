namespace Nzy3d.Colors
{
    /// <summary>
    /// <para>
    /// <see cref="IMultiColorable"/> objects may have several colors interpolated between each of
    /// their individual points colors.
    /// </para>
    /// <para>
    /// A <see cref="IMultiColorable"/> object requires a <see cref="ColorMapper"/> that defines a strategy
    /// for coloring points according to their position.
    /// </para>
    /// </summary>
    public interface IMultiColorable
    {
        /// <summary>
        /// Get/Set the colormapper that will be used by the Drawable, instead of using precomputed colors
        /// </summary>
        ColorMapper ColorMapper { get; set; }
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
