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

    private TextureSampler m_sampler;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Raster( int width, int height )
    {
        m_width = width;
        m_height = height;

		RASTER_TYPE = RasterType.Line;
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
    /// UV采样
    /// </summary>
    /// <param name="samper"></param>
    public void SetTextureSampler( TextureSampler samper )
    {
        m_sampler = samper;
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

		}
	}

	private void line_rasterize( Trangle trangle )
	{
        drawLine(trangle.m_vertexs[0], trangle.m_vertexs[1]);
        drawLine(trangle.m_vertexs[1], trangle.m_vertexs[2]);
        drawLine(trangle.m_vertexs[2], trangle.m_vertexs[0]);
    }

	private void solid_rasterize( Trangle trangle )
	{
	}

	private void texture_rasterize( Trangle trangle )
	{
	}

    private void drawPixel( int x, int y, Color32 color, float z )
    {
        if (x >= 0 && x < m_width && y >= 0 && y < m_height)
        {
            int index = y * m_width + x;

            if (z >= 0)
            {
                if (z < m_depthBuffer[index])
                {
                    m_colorBuffer[index] = color;
                    m_depthBuffer[index] = z;
                }
            }
            else
            {
                m_colorBuffer[index] = color;
            }
        }
    }

    private void drawLine( Vertex pt1, Vertex pt2 )
    {
        if (pt1.x == pt2.x)
        {
            if( pt1.y > pt2.y )
            {
                for (int i = pt2.y; i <= pt1.y; i++)
                    drawPixel(pt1.x, i, Color.white, -1);
            }
            else
            {
                for (int i = pt1.y; i <= pt2.y; i++)
                    drawPixel(pt1.x, i, Color.white, -1);
            }
        }
        else if ( pt1.y == pt2.y )
        {
            if(pt1.x > pt2.x)
            {
                for (int i = pt2.x; i <= pt1.x; i++)
                    drawPixel(i, pt1.y, Color.white, -1);
            }
            else
            {
                for (int i = pt1.x; i <= pt2.x; i++)
                    drawPixel(i, pt1.y, Color.white, -1);
            }
        }
        else if ( Mathf.Abs(pt1.x - pt2.x) > Mathf.Abs(pt1.y - pt2.y) )
        {
            if( pt1.x > pt2.x )
            {
                float k = (float)( pt1.y - pt2.y )/(float)( pt1.x - pt2.x );
                float accu = pt2.y;

                for (int i = pt2.x; i <= pt1.x; i++, accu += k)
                    drawPixel(i, (int)accu, Color.white, -1);
            }
            else
            {
                float k = (float)( pt2.y - pt1.y ) / (float)( pt2.x - pt1.x );
                float accu = pt1.y;

                for (int i = pt1.x; i <= pt2.x; i++, accu += k)
                    drawPixel(i, (int)accu, Color.white, -1);
            }
        }
        else
        {
            if( pt1.y > pt2.y )
            {
                float k = (float)(pt1.x - pt2.x) / (float)(pt1.y - pt2.y);
                float accu = pt2.x;

                for (int i = pt2.y; i <= pt1.y; i++, accu += k)
                    drawPixel((int)accu, i, Color.white, -1);
            }
            else
            {
                float k = (float)(pt2.x - pt1.x) / (float)(pt2.y - pt1.y);
                float accu = pt1.x;

                for (int i = pt1.y; i <= pt2.y; i++, accu += k)
                    drawPixel((int)accu, i, Color.white, -1);
            }
        }
    }
}
