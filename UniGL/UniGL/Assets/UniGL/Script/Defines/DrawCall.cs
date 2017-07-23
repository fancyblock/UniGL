using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一次描绘的信息
/// </summary>
public class DrawCall
{
    private List<Vertex> m_vertexBuff;
    private List<int> m_indexbuff;
    private int m_trangleCount;

	private List<Vertex> m_verteices;
	private List<Trangle> m_trangleList;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vertexBuff"></param>
    /// <param name="indexBuff"></param>
    /// <param name="trangleCount"></param>
    public DrawCall(List<Vertex> vertexBuff, List<int> indexBuff, int trangleCount)
    {
        m_vertexBuff = vertexBuff;
        m_indexbuff = indexBuff;
        m_trangleCount = trangleCount;

		m_verteices = new List<Vertex> (vertexBuff.Count);
    }

    /// <summary>
    /// 坐标变换
    /// </summary>
    /// <param name="matrix"></param>
    public void Transform( Matrix4x4 matrix )
    {
		for (int i = 0; i < m_vertexBuff.Count; i++) 
		{
			Vertex vertex = new Vertex (m_vertexBuff [i]);
			vertex.position = matrix.MultiplyPoint3x4 (vertex.position);
			vertex.normal = matrix.MultiplyVector (vertex.normal);

			m_verteices.Add(vertex);
		}
    }

    /// <summary>
    /// 生成三角形列表
    /// </summary>
    public void GenTrangleList()
    {
        m_trangleList = new List<Trangle>(m_trangleCount);

        for (int i = 0; i < m_trangleCount; i++)
        {
            Trangle trangle = new Trangle();
            trangle.m_vertexs = new Vertex[3];

            int indexBase = i * 3;

            for (int j = 0; j < 3; j++)
                trangle.m_vertexs[j] = m_verteices[m_indexbuff[indexBase + j]];

            m_trangleList.Add(trangle);
        }
    }

    /// <summary>
    /// 背面剔除
    /// </summary>
    public void BackFaceCulling( bool isPerspective )
    {
        List<Trangle> removeList = new List<Trangle>();

        if( isPerspective )
        {
            foreach (Trangle trangle in m_trangleList)
            {
                Vector3 seeVec = trangle.m_vertexs[0].position.normalized;
                float d = Vector3.Dot(seeVec, trangle.NORMAL);
                if (!(d <= 0 || d >= 1.0f))
                    removeList.Add(trangle);
            }
        }
        else
        {
            foreach( Trangle trangle in m_trangleList )
            {
                if (trangle.NORMAL.z > 0)
                    removeList.Add(trangle);
            }
        }

        foreach (Trangle trangle in removeList)
            m_trangleList.Remove(trangle);
    }

    /// <summary>
    /// 视线之外剔除
    /// </summary>
    /// <param name="clippingSpace"></param>
    public void Clipping( IClippingSpace clippingSpace )
    {
		m_trangleList = clippingSpace.Cliping (m_trangleList);
    }

    /// <summary>
    /// 应用光照
    /// </summary>
    /// <param name="light"></param>
    public void ApplyLight( ILight light )
    {
        foreach( Trangle trangle in m_trangleList )
        {
            foreach (Vertex vertex in trangle.m_vertexs)
                light.CalculateLight(vertex);
        }
    }

    /// <summary>
    /// 投影计算
    /// </summary>
    /// <param name="projector"></param>
    public void Projection( IProjector projector )
    {
		foreach (Trangle trangle in m_trangleList) 
		{
			foreach (Vertex vertex in trangle.m_vertexs) 
				projector.WorldToScreen (vertex.position, out vertex.x, out vertex.y);
		}
    }

    /// <summary>
    /// 三角形列表 
    /// </summary>
    public List<Trangle> TRANGLE_LIST { get { return m_trangleList; } }
}
