using UnityEngine;
using UnityEngine.UI;


public class GlobalWork : MonoBehaviour
{
    public UniGL_Ex m_renderer;

    public InputField m_resolution;
    public Toggle m_projections;

    public Toggle[] m_textures;
    public Toggle[] m_renderTypes;
    public Toggle m_light;


    public void onResolutionChange()
    {
        string resolution = m_resolution.text;
        string[] size = resolution.Split('x');

        m_renderer.SetResolution(int.Parse(size[0]), int.Parse(size[1]));
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
        m_renderer.SetOrtho(!m_projections.isOn);
    }

    public void onRenderTypeChange()
    {
        foreach( Toggle tog in m_renderTypes )
        {
            if( tog.isOn )
            {
                string name = tog.gameObject.name;

                m_renderer.SetRenderType(name);

                // [HACK]
                if (name == "Texture" || name == "Solid")
                {
                    m_renderer.SetOrtho(false);
                    m_projections.isOn = true;
                }
                //[HACK]
            }
        }
    }
}
