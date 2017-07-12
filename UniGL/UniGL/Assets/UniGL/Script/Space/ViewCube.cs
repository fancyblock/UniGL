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

    private float m_size;
	private int m_viewportWid;
	private int m_viewportHei;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="size"></param>
	public ViewCube(float size, int viewportWid, int viewportHei)
    {
        m_size = size;
		m_viewportWid = viewportWid;
		m_viewportHei = viewportHei;
    }

    public List<Vertex> Cliping(Vertex vertex)
    {
        throw new NotImplementedException();
    }

    public Matrix4x4 GetProjectionMatrix()
    {
        throw new NotImplementedException();
    }

	public void ProcessPosition( Vector4 pos, out int x, out int y )
	{
		x = pos.x;
		y = pos.y;
	}
}
