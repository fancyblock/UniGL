using System;
using System.Collections.Generic;
using UnityEngine;


public class Frustum : IClippingSpace, IProjector
{
	private Plane m_leftClippingPlane;
    private Plane m_rightClippinPlane;

    private Plane m_upClippingPlane;
    private Plane m_downClippingPlane;

    private Plane m_nearClippingPlane;

    private float m_d;
    private float m_size;
	private int m_viewportWid;
	private int m_viewportHei;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="d"></param>
    /// <param name="size"></param>
	public Frustum(float d, float size, int viewportWid, int viewportHei)
    {
        m_d = d;
        m_size = size;
		m_viewportWid = viewportWid;
		m_viewportHei = viewportHei;
    }

	public List<Trangle> Cliping(List<Trangle> trangleList)
    {
        //TODO 

		return trangleList;
    }

	public void ProcessPosition( Vector4 pos, out int x, out int y )
	{
		x = 0;
		y = 0;

		//TODO 
	}
}
