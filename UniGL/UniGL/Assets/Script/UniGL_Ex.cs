﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class UniGL_Ex : MonoBehaviour
{
    public Camera m_camera;
    public RawImage m_rawImage;
    public CanvasScaler m_scaler;

    public int m_width;
    public int m_height;

    private UniGL m_uniGL;

	// Use this for initialization
	void Awake ()
    {
        // create texture 
        Texture2D texture = new Texture2D( m_width, m_height, TextureFormat.RGBA32, false, false);
        m_rawImage.texture = texture;
        m_rawImage.SetNativeSize();

        m_scaler.referenceResolution = new Vector2(m_width, m_height);

        // adjust size scale 
        if (Screen.width > Screen.height)
            m_camera.orthographicSize = texture.height / 2;
        else
            m_camera.orthographicSize = texture.height;             ///////[TEMP]

        m_uniGL = new UniGL(texture);

        init();
    }


    private List<Vertex> m_vertBuff;
    private List<int> m_indexBuf;
    private Texture2D m_tex;

    private void init()
    {
        //m_uniGL.Ortho(10);
        m_uniGL.Perspective(3, 5);
        m_uniGL.ClearColor(Color.black);
        m_tex = Resources.Load<Texture2D>("logo");

        m_uniGL.BindTexture(0, m_tex);

        m_vertBuff = new List<Vertex>() 
		{
            new Vertex( new Vector3(-2, 2, -2), new Vector2(0, 0) ),
            new Vertex( new Vector3(2, 2, -2), new Vector2(1, 0) ),
            new Vertex( new Vector3(2, -2, -2), new Vector2(1, 1) ),
            new Vertex( new Vector3(-2, -2, -2), new Vector2(0, 1) ),
        };
        m_indexBuf = new List<int>() 
		{
            0, 1, 2, 0, 2, 3
        };
    }

    private float m_angle = 0;

    // Update is called once per frame
    void Update ()
    {
		Matrix4x4 mat = Matrix4x4.identity;
		mat *= Matrix4x4.Translate (new Vector3 (0, 0, 8));
        mat *= Matrix4x4.Rotate(Quaternion.Euler(m_angle/3,m_angle,0));
        m_angle += 0.3f;

		m_uniGL.SetModelViewMatrix(mat);

        m_uniGL.Clear(true, true);

        // 绘制一个立方体
        {
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
        }

        m_uniGL.Present();
	}

}
