using System;
using UnityEngine;


public class UniGL
{
    private Texture2D m_texture;
    private Color32[] m_buffer;


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
    }

    /// <summary>
    /// 设置投影参数
    /// </summary>
    /// <param name="d"></param>
    public void SetProjection( float d )
    {
        //TODO 
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
        //TODO 
    }
}
