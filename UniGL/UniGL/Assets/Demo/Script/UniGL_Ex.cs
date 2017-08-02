using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UniGL_Ex : MonoBehaviour
{
    public RawImage m_rawImage;

    private UniGL m_uniGL;

    private List<Vertex> m_vertBuff;
    private List<int> m_indexBuf;
    private float m_angle = 0;

    private int m_width = 80;
    private int m_height = 80;

    private bool m_ortho = true;
    private bool m_lightOn = true;
    private string m_texture = "tex01";
    private string m_randerType = "Point";


    /// <summary>
    /// 设置分辨率
    /// </summary>
    /// <param name="wid"></param>
    /// <param name="hei"></param>
    public void SetResolution( int wid, int hei )
    {
        if (m_width == wid && m_height == hei)
            return;

        createGL(wid, hei);
    }

    /// <summary>
    /// 设置投影方式
    /// </summary>
    /// <param name="ortho"></param>
    public void SetOrtho( bool ortho )
    {
        m_ortho = ortho;

        if( m_ortho )
            m_uniGL.Ortho(10);
        else
            m_uniGL.Perspective(3, 5);
    }

    /// <summary>
    /// 设置纹理贴图
    /// </summary>
    /// <param name="tex"></param>
    public void SetTexture( string tex )
    {
        m_texture = tex;

        m_uniGL.BindTexture(0, Resources.Load<Texture2D>(m_texture));
    }

    /// <summary>
    /// 打开灯光与否
    /// </summary>
    /// <param name="on"></param>
    public void SetLightOn( bool on )
    {
        m_lightOn = on;

        if (m_lightOn)
            m_uniGL.SetLight(new DirLight((new Vector3(1, 1, 5)).normalized));
        else
            m_uniGL.SetLight(null);
    }

    /// <summary>
    /// 设置渲染类型 
    /// </summary>
    /// <param name="type"></param>
    public void SetRenderType( string type )
    {
        m_randerType = type;

        m_uniGL.SetRenderType( (RasterType)Enum.Parse(typeof(RasterType), type) );
    }


    private void createGL( int wid, int hei )
    {
        m_width = wid;
        m_height = hei;

        Texture2D texture = new Texture2D(m_width, m_height, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;
        m_rawImage.texture = texture;

        m_uniGL = new UniGL(texture);

        m_uniGL.ClearColor(Color.black);

        SetOrtho(m_ortho);
        SetTexture(m_texture);
        SetLightOn(m_lightOn);
        SetRenderType(m_randerType);
    }

    void Awake ()
    {
        createCube();
        createGL(80, 80);
    }

    void Update ()
    {
        m_uniGL.Clear(true, true);

        Matrix4x4 mat = Matrix4x4.identity;
        mat *= Matrix4x4.Translate(new Vector3(0, 0, 8));
        mat *= Matrix4x4.Rotate(Quaternion.Euler(m_angle / 3, m_angle, 0));
        m_angle += 0.3f;

        m_uniGL.SetModelViewMatrix(mat);

        // 绘制一个立方体
        m_uniGL.Draw(m_vertBuff, m_indexBuf, 2);

        mat *= Matrix4x4.Rotate(Quaternion.Euler(0, 90, 0));
        m_uniGL.SetModelViewMatrix(mat);
        m_uniGL.Draw(m_vertBuff, m_indexBuf, 2);

        mat *= Matrix4x4.Rotate(Quaternion.Euler(0, 90, 0));
        m_uniGL.SetModelViewMatrix(mat);
        m_uniGL.Draw(m_vertBuff, m_indexBuf, 2);

        mat *= Matrix4x4.Rotate(Quaternion.Euler(0, 90, 0));
        m_uniGL.SetModelViewMatrix(mat);
        m_uniGL.Draw(m_vertBuff, m_indexBuf, 2);

        mat *= Matrix4x4.Rotate(Quaternion.Euler(90, 0, 0));
        m_uniGL.SetModelViewMatrix(mat);
        m_uniGL.Draw(m_vertBuff, m_indexBuf, 2);

        mat *= Matrix4x4.Rotate(Quaternion.Euler(180, 0, 0));
        m_uniGL.SetModelViewMatrix(mat);
        m_uniGL.Draw(m_vertBuff, m_indexBuf, 2);

        m_uniGL.Present();
	}

    private void createCube()
    {
        m_vertBuff = new List<Vertex>()
        {
            new Vertex( new Vector3(-2, 2, -2), new Vector2(0, 0), new Vector3(0,0,-1) ),
            new Vertex( new Vector3(2, 2, -2), new Vector2(1, 0), new Vector3(0,0,-1) ),
            new Vertex( new Vector3(2, -2, -2), new Vector2(1, 1), new Vector3(0,0,-1) ),
            new Vertex( new Vector3(-2, -2, -2), new Vector2(0, 1), new Vector3(0,0,-1) ),
        };
        m_indexBuf = new List<int>()
        {
            0, 1, 2, 0, 2, 3
        };
    }
}
