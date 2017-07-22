using UnityEngine;


public class DirLight : ILight
{
    private Vector3 m_lightDir;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="lightDir"></param>
    public DirLight( Vector3 lightDir )
    {
        m_lightDir = -lightDir;
    }

    /// <summary>
    /// 光照计算
    /// </summary>
    /// <param name="vertex"></param>
    public void CalculateLight(Vertex vertex)
    {
        float intensity = Vector3.Dot(m_lightDir, vertex.normal);

        if (intensity <= 0)
            vertex.color = Color.black;
        else
            vertex.color = Color32.Lerp(Color.black, vertex.color, intensity);
    }
}
