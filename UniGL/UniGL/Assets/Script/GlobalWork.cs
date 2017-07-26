using UnityEngine;
using UnityEngine.UI;


public class GlobalWork : MonoBehaviour
{
    public UniGL_Ex m_renderer;

    public Toggle[] m_resolutions;
    public Toggle[] m_textures;
    public Toggle[] m_projections;
    public Toggle[] m_renderTypes;
    public Toggle m_light;


    public void onResolutionChange()
    {
        foreach( Toggle tog in m_resolutions )
        {
            if (tog.isOn)
            {
                string resolution = tog.gameObject.name;
                string[] size = resolution.Split('x');

                m_renderer.SetResolution(int.Parse(size[0]), int.Parse(size[1]));
            }
        }
    }

    public void onTexChange()
    {
        foreach( Toggle tog in m_textures )
        {
            if( tog.isOn )
            {
                string name = tog.gameObject.name;

                m_renderer.SetTexture(name);
            }
        }
    }

    public void onLightChange()
    {
        if (m_light.isOn)
            m_renderer.SetLightOn(true);
        else
            m_renderer.SetLightOn(false);
    }

    public void onProjectChange()
    {
        foreach( Toggle tog in m_projections )
        {
            if( tog.isOn )
            {
                string name = tog.gameObject.name;

                if (name == "Orth")
                    m_renderer.SetOrtho(true);
                else
                    m_renderer.SetOrtho(false);
            }
        }
    }

    public void onRenderTypeChange()
    {
        foreach( Toggle tog in m_renderTypes )
        {
            if( tog.isOn )
            {
                string name = tog.gameObject.name;

                m_renderer.SetRenderType(name);
            }
        }
    }
}
