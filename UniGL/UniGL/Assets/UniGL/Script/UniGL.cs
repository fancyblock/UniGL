using System;
using UnityEngine;


public class UniGL
{
    private Texture2D m_texture;
    private Color32[] m_buffer;

    private int m_width;
    private int m_height;
    private Color32 m_clearColor;

    private Matrix4x4 m_projectionMat;
    private Matrix4x4 m_modelViewMat;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="texture"></param>
    public UniGL(Texture2D texture)
    {
        m_texture = texture;

        int bufferSize = m_texture.height * m_texture.width;
        m_buffer = new Color32[bufferSize];
        
        for(int i = 0; i < bufferSize; i++ )
            m_buffer[i] = Color.black;

        m_width = m_texture.width;
        m_height = m_texture.height;
    }

    /// <summary>
    /// 设置透视投影参数
    /// </summary>
    /// <param name="d"></param>    投影面距视点距离
    /// <param name="size"></param> 投影面高度
    public void Perspective( float d, float size = 1.0f )
    {
        //TODO 
    }

    /// <summary>
    /// 设置正交投影参数 
    /// </summary>
    /// <param name="size"></param>
    public void Ortho( float size )
    {
        //TODO 
    }

    /// <summary>
    /// 设置背景色
    /// </summary>
    /// <param name="color"></param>
    public void ClearColor( Color32 color )
    {
        m_clearColor = color;
    }

    /// <summary>
    /// 渲染当前的内容
    /// </summary>
    public void Present()
    {
        //TODO 
        
        m_texture.SetPixels32(m_buffer);
        m_texture.Apply();
    }

    /// <summary>
    /// 设置模型视图矩阵
    /// </summary>
    /// <param name="modelViewMat"></param>
    public void SetModelViewMatrix(Matrix4x4 modelViewMat)
    {
        m_modelViewMat = modelViewMat;
    }
}
