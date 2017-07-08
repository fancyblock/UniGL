using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// 光栅化
/// </summary>
public class UniGL
{
    private Texture2D m_texture;
    private Color32[] m_buffer;

    private int m_width;
    private int m_height;
    private Color32 m_clearColor;

    private Matrix4x4 m_modelViewMat;

    private IProjector m_projector;             // 提供投影矩阵
    private IClippingSpace m_clippingSpace;     // 空间裁剪

    private LinkedList<DrawCall> m_drawCallList;


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

        m_drawCallList = new LinkedList<DrawCall>();
    }

    /// <summary>
    /// 设置透视投影参数
    /// </summary>
    /// <param name="d"></param>    投影面距视点距离
    /// <param name="size"></param> 投影面高度
    public void Perspective( float d, float size = 1.0f )
    {
        Assert.IsTrue(d > float.Epsilon);
        Assert.IsTrue(size > float.Epsilon);

        Frustum frustum = new Frustum(d, size);

        m_projector = frustum;
        m_clippingSpace = frustum;
    }

    /// <summary>
    /// 设置正交投影参数 
    /// </summary>
    /// <param name="size"></param>
    public void Ortho( float size )
    {
        Assert.IsTrue(size > float.Epsilon);

        ViewCube viewCube = new ViewCube(size);

        m_projector = viewCube;
        m_clippingSpace = viewCube;
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
    /// 绘制调用
    /// </summary>
    /// <param name="vertexBuff"></param>
    /// <param name="indexBuff"></param>
    /// <param name="trangleCount"></param>
    public void Draw(List<Vertex> vertexBuff, List<int> indexBuff, int trangleCount)
    {
        DrawCall drawCall = new DrawCall();
        //TODO 

        m_drawCallList.AddLast(drawCall);
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="stencilBuffer"></param>
    public void Clear( bool colorBuffer, bool depthBuffer, bool stencilBuffer )
    {
        //TODO 
    }

    /// <summary>
    /// 绑定贴图
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tex"></param>
    public void BindTexture(int channel, Texture2D tex)
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

        m_drawCallList.Clear();
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
