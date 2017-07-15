using UnityEngine;


public enum RasterType
{
	Point,
	Line,
	SolidColor,
	Texture,
}


/// <summary>
/// 用于三角形光栅化 
/// </summary>
public class Raster
{
    private int m_width;
    private int m_height;

    private Color32[] m_colorBuffer;
    private float[] m_depthBuffer;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Raster( int width, int height )
    {
        m_width = width;
        m_height = height;

		RASTER_TYPE = RasterType.Point;
    }

	/// <summary>
	/// Gets or sets the RASTE r TYP.
	/// </summary>
	/// <value>The RASTE r TYP.</value>
	public RasterType RASTER_TYPE{ get; set; }

    /// <summary>
    /// 设置颜色缓冲区
    /// </summary>
    /// <param name="colorBuffer"></param>
    public void SetColorBuffer(Color32[] colorBuffer)
    {
        m_colorBuffer = colorBuffer;
    }

    /// <summary>
    /// 设置深度缓冲区
    /// </summary>
    /// <param name="depthBuffer"></param>
    public void SetDepthBuffer(float[] depthBuffer)
    {
        m_depthBuffer = depthBuffer;
    }

    /// <summary>
    /// 光栅化一个三角形 
    /// </summary>
    /// <param name="trangle"></param>
    public void Rasterize( Trangle trangle )
    {
		switch (RASTER_TYPE) 
		{
		case RasterType.Line:
			line_rasterize (trangle);
			break;
		case RasterType.Point:
			point_rasterize (trangle);
			break;
		case RasterType.SolidColor:
			solid_rasterize (trangle);
			break;
		case RasterType.Texture:
			texture_rasterize (trangle);
			break;
		default:
			break;
		}
    }


	private void point_rasterize( Trangle trangle )
	{
		foreach (Vertex vertex in trangle.m_vertexs) 
		{
			if (vertex.x >= 0 && vertex.x < m_width && vertex.y >= 0 && vertex.y < m_height) 
			{
				int index = vertex.y * m_width + vertex.x;

				if (vertex.position.z < m_depthBuffer [index]) 
				{
					m_colorBuffer [index] = vertex.color;
					m_depthBuffer [index] = vertex.position.z;
				}
			}
		}
	}

	private void line_rasterize( Trangle trangle )
	{
	}

	private void solid_rasterize( Trangle trangle )
	{
	}

	private void texture_rasterize( Trangle trangle )
	{
	}
}
