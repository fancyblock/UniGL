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


    void Awake ()
    {
        // create texture 
        Texture2D texture = new Texture2D( 80, 80, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;
        m_rawImage.texture = texture;

        m_uniGL = new UniGL(texture);

        m_uniGL.Ortho(10);
        //m_uniGL.Perspective(3, 5);
        m_uniGL.ClearColor(Color.black);

        m_uniGL.BindTexture(0, Resources.Load<Texture2D>("Texture/tex01"));

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

        DirLight light = new DirLight((new Vector3(1, 1, 5)).normalized);
        m_uniGL.SetLight(light);
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
}
