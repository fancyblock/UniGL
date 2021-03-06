﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// 光栅化
/// </summary>
public class UniGL 
{
    private Texture2D m_texture;
    private Color32[] m_buffer;
    private float[] m_depthBuffer;

    private int m_width;
    private int m_height;
    private Color32 m_clearColor;

    private Matrix4x4 m_modelViewMat;

    private IProjector m_projector;             // 提供投影矩阵
    private IClippingSpace m_clippingSpace;     // 空间裁剪
    private TextureSampler m_sampler;           // Texture Sampler
    private ILight m_light;

    private LinkedList<DrawCall> m_drawCallList;
    private Raster m_raster; 


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="texture"></param>
    public UniGL(Texture2D texture)
    {
        m_texture = texture;

        int bufferSize = m_texture.height * m_texture.width;
        m_buffer = new Color32[bufferSize];
        m_depthBuffer = new float[bufferSize];
        
        for(int i = 0; i < bufferSize; i++ )
        {
            m_buffer[i] = Color.black;
            m_depthBuffer[i] = float.MaxValue;
        }

        m_width = m_texture.width;
        m_height = m_texture.height;

        m_drawCallList = new LinkedList<DrawCall>();
        m_sampler = new TextureSampler();

        m_raster = new Raster(m_width, m_height);
        m_raster.SetColorBuffer(m_buffer);
        m_raster.SetDepthBuffer(m_depthBuffer);
        m_raster.SetTextureSampler(m_sampler);
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

		Frustum frustum = new Frustum(d, size, m_width, m_height);

        m_projector = frustum;
        m_clippingSpace = frustum;

        m_raster.SetProjector(m_projector);
    }

    /// <summary>
    /// 设置正交投影参数 
    /// </summary>
    /// <param name="size"></param>
    public void Ortho( float size )
    {
        Assert.IsTrue(size > float.Epsilon);

		ViewCube viewCube = new ViewCube(size, m_width, m_height);

        m_projector = viewCube;
        m_clippingSpace = viewCube;

        m_raster.SetProjector(m_projector);
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
        DrawCall drawCall = new DrawCall(vertexBuff, indexBuff, trangleCount);
        drawCall.Transform(m_modelViewMat);

        m_drawCallList.AddLast(drawCall);
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    public void Clear( bool colorBuffer, bool depthBuffer )
    {
        if( colorBuffer )
        {
            for( int i = 0; i < m_buffer.Length; i++ )
                m_buffer[i] = m_clearColor;
        }
        
        if( depthBuffer )
        {
            for( int i = 0; i < m_depthBuffer.Length; i++ )
                m_depthBuffer[i] = float.MaxValue;
        }
    }

    /// <summary>
    /// 绑定贴图
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tex"></param>
    public void BindTexture(int channel, Texture2D tex)
    {
        Assert.IsTrue(tex != null);

        m_sampler.SetTexture( channel, tex );
    }

    /// <summary>
    /// 设置渲染类型
    /// </summary>
    /// <param name="type"></param>
    public void SetRenderType( RasterType type )
    {
        m_raster.RASTER_TYPE = type;
    }

    /// <summary>
    /// 渲染当前的内容
    /// </summary>
    public void Present()
    {
        List<Trangle> trangleLists = new List<Trangle>();

        foreach (DrawCall drawCall in m_drawCallList)
        {
            drawCall.GenTrangleList();
            if( m_raster.RASTER_TYPE != RasterType.Point )
                drawCall.BackFaceCulling(m_projector.IsPerspective());
            drawCall.Clipping(m_clippingSpace);
            if (m_light != null)
                drawCall.ApplyLight(m_light);
            drawCall.Projection(m_projector);

            trangleLists.AddRange(drawCall.TRANGLE_LIST);
        }

        // 逐个光栅化三角形 
        foreach (Trangle trangle in trangleLists)
            m_raster.Rasterize(trangle);
        
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

    /// <summary>
    /// 设置光照
    /// </summary>
    /// <param name="light"></param>
    public void SetLight(ILight light)
    {
        m_light = light;
    }
}
