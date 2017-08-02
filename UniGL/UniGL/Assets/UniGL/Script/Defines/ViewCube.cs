using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 正交投影体
/// </summary>
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

    private float m_halfHei;
    private float m_halfWid;


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

        m_halfHei = m_sizeHei / 2.0f;
        m_halfWid = m_sizeWid / 2.0f;

        m_ratio = (float)m_viewportHei / (float)m_sizeHei;
    }

    /// <summary>
    /// 三角形裁剪
    /// </summary>
    /// <param name="trangleList"></param>
    /// <returns></returns>
	public List<Trangle> Cliping(List<Trangle> trangleList)
    {
        //TODO 

		return trangleList;
    }

    /// <summary>
    /// 计算投影
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
	public void WorldToScreen( Vector4 pos, out int x, out int y )
	{
		x = (int)( (pos.x + m_halfWid) * m_ratio );
		y = (int)( (pos.y + m_halfHei) * m_ratio );
	}

    /// <summary>
    /// 屏幕坐标到世界坐标
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="worldZ"></param>
    /// <returns></returns>
    public Vector3 ScreenToWorld(int x, int y, float worldZ)
    {
        float worldX = (float)x / m_ratio - m_halfWid;
        float worldY = (float)y / m_ratio - m_halfHei;

        return new Vector3(worldX, worldY, worldZ);
    }

    /// <summary>
    /// 是否是透视
    /// </summary>
    /// <returns></returns>
    public bool IsPerspective() { return false; }

}
