using Nzy3d.Maths;
using OpenTK.Graphics.OpenGL;

namespace Nzy3d.Plot3D.Rendering.Lights
{
	public class LightSet
	{
		internal List<Light> _lights;

		internal bool _lazyLightInit = false;
		public LightSet()
		{
			_lights = new List<Light>();
		}

		public LightSet(List<Light> ligths)
		{
			_lights = ligths;
		}

		public void Init()
		{
			GL.Enable(EnableCap.ColorMaterial);
		}

		public void Apply(Coord3d scale)
		{
			if (_lazyLightInit)
			{
				InitLight();
				foreach (Light alight in _lights)
				{
					LightSwitch.Enable(alight.Id);
					_lazyLightInit = false;
				}
			}

			foreach (Light alight in _lights)
			{
				alight.Apply(scale);
			}
		}

		/// <summary>
		/// Enable lighting only if at least one light is present in light set
		/// </summary>
		public void Enable()
		{
			Enable(true);
		}

		/// <summary>
		/// Enable lighting.
		/// </summary>
		/// <param name="onlyIfAtLeastOneLight">If True, lighting is enabled only if at leat one light is present in light set. If False, lighting is always enabled.</param>
		/// <remarks></remarks>
		public void Enable(bool onlyIfAtLeastOneLight)
		{
			if ((!onlyIfAtLeastOneLight) || _lights.Count > 0)
			{
				GL.Enable(EnableCap.Lighting);
			}
		}

		public void Disable()
		{
			GL.Disable(EnableCap.Lighting);
		}

		public object Item(int index)
		{
			return _lights[index];
		}

		public void Add(Light alight)
		{
			if (_lights.Count == 0)
			{
				QueryLazyLightInit();
			}
			_lights.Add(alight);
		}

		public void Remove(Light alight)
		{
			_lights.Remove(alight);
		}

		internal void QueryLazyLightInit()
		{
			_lazyLightInit = true;
		}

		/// <summary>
		/// Initialize the lightset
		/// </summary>
		/// <remarks>Original source : http://www.sjbaker.org/steve/omniv/opengl_lighting.html</remarks>
		internal void InitLight()
		{
			GL.Enable(EnableCap.ColorMaterial);
			GL.Enable(EnableCap.Lighting);

			// Ligth model 
			GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
			//GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
			//GL.LightModel(LightModelParameter.LightModelLocalViewer, 0);
        }
	}
}
