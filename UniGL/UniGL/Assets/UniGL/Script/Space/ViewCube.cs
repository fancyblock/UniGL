using System;
using System.Collections.Generic;
using UnityEngine;


public class ViewCube : IClippingSpace, IProjector
{
    private Plane m_leftClippingPlane;
    private Plane m_rightClippinPlane;

    private Plane m_upClippingPlane;
    private Plane m_downClippingPlane;

    private Plane m_nearClippingPlane;
    private float size;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="size"></param>
    public ViewCube(float size)
    {
        this.size = size;
    }

    public List<Vertex> Cliping(Vertex vertex)
    {
        throw new NotImplementedException();
    }

    public Matrix4x4 GetProjectionMatrix()
    {
        throw new NotImplementedException();
    }
}
