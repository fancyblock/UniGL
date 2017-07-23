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
    private IProjector m_projdecor;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Raster( int width, int height )
    {
        m_width = width;
        m_height = height;

		RASTER_TYPE = RasterType.SolidColor;
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
    /// 设置投影计算接口（透视修正计算用）
    /// </summary>
    /// <param name="projector"></param>
    public void SetProjector( IProjector projector )
    {
        m_projdecor = projector;
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
            trangle_rasterize(trangle, true);
			break;
		case RasterType.Texture:
            trangle_rasterize(trangle, false);
			break;
		default:
			break;
		}
    }


    /// <summary>
    /// 只画点
    /// </summary>
    /// <param name="trangle"></param>
	private void point_rasterize( Trangle trangle )
	{
        foreach (Vertex vertex in trangle.m_vertexs)
            drawPixel(vertex.x, vertex.y, vertex.color, vertex.position.z);
	}

    /// <summary>
    /// 画线
    /// </summary>
    /// <param name="trangle"></param>
	private void line_rasterize( Trangle trangle )
	{
        drawLine(trangle.m_vertexs[0], trangle.m_vertexs[1]);
        drawLine(trangle.m_vertexs[1], trangle.m_vertexs[2]);
        drawLine(trangle.m_vertexs[2], trangle.m_vertexs[0]);
    }

    /// <summary>
    /// 三角形光栅化
    /// </summary>
    /// <param name="trangle"></param>
	private void trangle_rasterize( Trangle trangle, bool solidColor )
	{
        if (isCollinear(trangle))       // 共线的三角形暂不绘制
            return;

        if( isUpTrangle( trangle ) )
        {
            upTrangleRasterize(trangle, solidColor);
        }
        else if( isDownTrangle( trangle ) )
        {
            downTrangleRasterize(trangle, solidColor);
        }
        else
        {
            Trangle[] trangles = splitTrangle(trangle);

            upTrangleRasterize(trangles[0], solidColor);
            downTrangleRasterize(trangles[1], solidColor);
        }
	}

    /// <summary>
    /// 光栅化上三角形
    /// </summary>
    /// <param name="trangle"></param>
    /// <param name="solidColor"></param>
    private void upTrangleRasterize( Trangle trangle, bool solidColor )
    {
        //TODO 
    }

    /// <summary>
    /// 光栅化下三角形
    /// </summary>
    /// <param name="trangle"></param>
    /// <param name="solidColor"></param>
    private void downTrangleRasterize( Trangle trangle, bool solidColor )
    {
        //TODO 
    }

    /// <summary>
    /// 将三角形切分成上三角形和下三角形 
    /// </summary>
    /// <param name="trangle"></param>
    /// <returns></returns>
    private Trangle[] splitTrangle( Trangle trangle )
    {
        Trangle[] trangles = new Trangle[2];
        int[] index = new int[] { 0, 1, 2 };

        // 顶点按照y来排序
        for( int i = 1; i <= 2; i++ )
        {
            if( trangle.m_vertexs[index[i]].y > trangle.m_vertexs[index[0]].y )
            {
                int temp = index[i];
                index[i] = index[0];
                index[0] = temp;
            }
        }

        if( trangle.m_vertexs[index[2]].y > trangle.m_vertexs[index[1]].y )
        {
            int temp = index[1];
            index[1] = index[2];
            index[2] = temp;
        }

        Vertex upV = trangle.m_vertexs[index[0]];
        Vertex midV = trangle.m_vertexs[index[1]];
        Vertex downV = trangle.m_vertexs[index[2]];

        Vertex midV2 = new Vertex();

        float ratio = (float)(upV.y - midV.y) / (float)(upV.y - downV.y);
        midV2.position = Vector4.Lerp(upV.position, downV.position, ratio );
        midV2.x = upV.x + Mathf.RoundToInt( (float)(downV.x - upV.x) * ratio );
        midV2.y = midV.y;

        //float ratio2 =
        //midV2.color = Color32.Lerp(upV.color, downV.color, ratio);      //[TEMP]
        //midV2.uv = ;

        trangles[0] = new Trangle();
        trangles[0].m_vertexs = new Vertex[3] { upV, midV, midV2 };

        trangles[1] = new Trangle();
        trangles[1].m_vertexs = new Vertex[3] { midV, midV2, downV };

        return trangles;
    }

    /// <summary>
    /// 是否成一条线
    /// </summary>
    /// <param name="trangle"></param>
    /// <returns></returns>
    private bool isCollinear( Trangle trangle )
    {
        //TODO 

        return false;
    }

    /// <summary>
    /// 是否是上三角形
    /// </summary>
    /// <param name="trangle"></param>
    /// <returns></returns>
    private bool isUpTrangle( Trangle trangle )
    {
        int y0 = trangle.m_vertexs[0].y;
        int y1 = trangle.m_vertexs[1].y;
        int y2 = trangle.m_vertexs[2].y;

        if (y0 > y1 && y1 == y2)
            return true;
        if (y1 > y0 && y0 == y2)
            return true;
        if (y2 > y0 && y0 == y1)
            return true;

        return false;
    }

    /// <summary>
    /// 是否是下三角形
    /// </summary>
    /// <param name="trangle"></param>
    /// <returns></returns>
    private bool isDownTrangle( Trangle trangle )
    {
        int y0 = trangle.m_vertexs[0].y;
        int y1 = trangle.m_vertexs[1].y;
        int y2 = trangle.m_vertexs[2].y;

        if (y0 < y1 && y1 == y2)
            return true;
        if (y1 < y0 && y0 == y2)
            return true;
        if (y2 < y0 && y0 == y1)
            return true;

        return false;
    }

    /// <summary>
    /// 只画像素
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    /// <param name="z"></param>
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

    /// <summary>
    /// DDA画线
    /// </summary>
    /// <param name="pt1"></param>
    /// <param name="pt2"></param>
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
                    drawPixel(i, (int)(accu), Color.white, -1);
            }
            else
            {
                float k = (float)( pt2.y - pt1.y ) / (float)( pt2.x - pt1.x );
                float accu = pt1.y;

                for (int i = pt1.x; i <= pt2.x; i++, accu += k)
                    drawPixel(i, (int)(accu), Color.white, -1);
            }
        }
        else
        {
            if( pt1.y > pt2.y )
            {
                float k = (float)(pt1.x - pt2.x) / (float)(pt1.y - pt2.y);
                float accu = pt2.x;

                for (int i = pt2.y; i <= pt1.y; i++, accu += k)
                    drawPixel((int)(accu), i, Color.white, -1);
            }
            else
            {
                float k = (float)(pt2.x - pt1.x) / (float)(pt2.y - pt1.y);
                float accu = pt1.x;

                for (int i = pt1.y; i <= pt2.y; i++, accu += k)
                    drawPixel((int)(accu), i, Color.white, -1);
            }
        }
    }
}
