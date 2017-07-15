﻿using System.Collections.Generic;
using UnityEngine;


public class ViewCube : IClippingSpace, IProjector
{
    private Plane m_leftClippingPlane;
    private Plane m_rightClippinPlane;

    private Plane m_upClippingPlane;
    private Plane m_downClippingPlane;

    private Plane m_nearClippingPlane;

    private float m_sizeHei;
	private float m_sizeWid;
	private int m_viewportWid;
	private int m_viewportHei;
	private float m_ratio;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="size"></param>
	public ViewCube(float size, int viewportWid, int viewportHei)
    {
		m_viewportWid = viewportWid;
		m_viewportHei = viewportHei;

		m_sizeHei = size;
		m_sizeWid = (float)m_viewportWid * m_sizeHei / (float)m_viewportHei;

		m_ratio = (float)m_viewportHei / (float)m_sizeHei;
    }

	public List<Trangle> Cliping(List<Trangle> trangleList)
    {
        //TODO 

		return trangleList;
    }

	public void ProcessPosition( Vector4 pos, out int x, out int y )
	{
		x = (int)( (pos.x + m_sizeWid / 2) * m_ratio );
		y = (int)( (pos.y + m_sizeHei / 2) * m_ratio );
	}
}
