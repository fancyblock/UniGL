using System.Collections;
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

        //TODO 
    }

    public void Clipping( IClippingSpace clippingSpace )
    {
        //TODO 
    }

}
