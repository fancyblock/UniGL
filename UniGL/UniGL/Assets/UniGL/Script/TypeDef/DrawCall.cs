using System.Collections.Generic;
using UnityEngine;


public class DrawCall
{
    private List<Vertex> m_vertexBuff;
    private List<int> m_indexbuff;
    private int m_trangleCount;


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
            //TODO 
        }
    }

    public void GenTrangleList()
    {
        //TODO 
    }

    public void Clipping( IClippingSpace clippingSpace )
    {
        //TODO 
    }

    public void Projection( IProjector projector )
    {
        //TODO 
    }

    public List<Trangle> TRANGLE_LIST
    {
        get
        {
            return null;
        }
    }
}
