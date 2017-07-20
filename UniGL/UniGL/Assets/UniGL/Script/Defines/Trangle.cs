using UnityEngine;


/// <summary>
/// 三角形
/// </summary>
public class Trangle
{
	public Vertex[] m_vertexs;

    /// <summary>
    /// 面法线
    /// </summary>
    public Vector3 NORMAL
    {
        get
        {
            Vector3 v1 = m_vertexs[1].position - m_vertexs[0].position;
            Vector3 v2 = m_vertexs[2].position - m_vertexs[1].position;

            Vector3 normal = Vector3.Cross( v1, v2 );
            return normal.normalized;
        }
    }
}
