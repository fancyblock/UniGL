using System.Collections.Generic;
using UnityEngine;


public class DrawCall
{
    private List<Vertex> m_vertexBuff;
    private List<int> m_indexbuff;
    private int m_trangleCount;
	private List<Trangle> m_trangleList;


    public DrawCall(List<Vertex> vertexBuff, List<int> indexBuff, int trangleCount)
    {
        m_vertexBuff = vertexBuff;
        m_indexbuff = indexBuff;
        m_trangleCount = trangleCount;
    }

    public void Transform( Matrix4x4 matrix )
    {
        foreach( Vertex vertex in m_vertexBuff )
        {
			matrix.MultiplyPoint3x4 (vertex.position);
			matrix.MultiplyVector (vertex.normal);
        }
    }

    public void BackFaceCulling()
    {
        //TODO 
    }

    public void GenTrangleList()
    {
		m_trangleList = new List<Trangle> (m_trangleCount);

		for (int i = 0; i < m_trangleCount; i++) 
		{
			Trangle trangle = new Trangle ();
			trangle.m_vertexs = new Vertex[3];

			int indexBase = i * 3;

			for (int j = 0; j < 3; j++) 
			{
				trangle.m_vertexs[j] = m_vertexBuff[m_indexbuff[indexBase + j]];
			}

			m_trangleList.Add (trangle);
		}
    }

    public void Clipping( IClippingSpace clippingSpace )
    {
		m_trangleList = clippingSpace.Cliping (m_trangleList);
    }

    public void Projection( IProjector projector )
    {
		foreach (Trangle trangle in m_trangleList) 
		{
			foreach (Vertex vertex in trangle.m_vertexs) 
			{
				projector.ProcessPosition (vertex.position, out vertex.x, out vertex.y);
			}
		}
    }

    public List<Trangle> TRANGLE_LIST
    {
        get
        {
			return m_trangleList;
        }
    }
}
