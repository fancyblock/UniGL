using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GlobalWork : MonoBehaviour
{
    public UniGL_Ex m_renderer;

    public InputField m_resolution;

    public Toggle m_projections;
    public Toggle m_light;

    public Dropdown m_renderType;
    public Dropdown m_texture;
    

    public void onResolutionChange()
    {
        string resolution = m_resolution.text;
        string[] size = resolution.Split('x');

        m_renderer.SetResolution(int.Parse(size[0]), int.Parse(size[1]));
    }

    public void onTexChange()
    {
        m_renderer.SetTexture(m_texture.options[m_texture.value].text);
    }

    public void onLightChange()
    {
        m_renderer.SetLightOn(m_light.isOn);
    }

    public void onProjectChange()
    {
        m_renderer.SetOrtho(!m_projections.isOn);
    }

    public void onRenderTypeChange()
    {
        string name = m_renderType.options[m_renderType.value].text;
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
