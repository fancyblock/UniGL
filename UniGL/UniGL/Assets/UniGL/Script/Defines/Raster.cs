using UnityEngine;


public enum RasterType
{
	Point,
	Line,
	Solid,
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
    private IProjector m_projector;


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Raster( int width, int height )
    {
        m_width = width;
        m_height = height;

		RASTER_TYPE = RasterType.Texture;
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
        m_projector = projector;
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
		case RasterType.Solid:
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
        if( isUpTrangle( trangle ) )
        {
            if (solidColor)
                upTrangleSolidRasterize(trangle);
            else
                upTrangleRasterize(trangle);
        }
        else if( isDownTrangle( trangle ) )
        {
            if (solidColor)
                downTrangleSolidRasterize(trangle);
            else
                downTrangleRasterize(trangle);
        }
        else
        {
            Trangle[] trangles = splitTrangle(trangle);

            if (solidColor)
            {
                upTrangleSolidRasterize(trangles[0]);
                downTrangleSolidRasterize(trangles[1]);
            }
            else
            {
                upTrangleRasterize(trangles[0]);
                downTrangleRasterize(trangles[1]);
            }
        }
	}

    private void upTrangleSolidRasterize(Trangle trangle)
    {
        //TODO 
    }

    private void downTrangleSolidRasterize(Trangle trangle)
    {
        //TODO 
    }

    /// <summary>
    /// 光栅化平底三角形
    /// </summary>
    /// <param name="trangle"></param>
    /// <param name="solidColor"></param>
    private void upTrangleRasterize( Trangle trangle )
    {
        sortVertex(trangle.m_vertexs);

        Vertex top = trangle.m_vertexs[0];
        Vertex left = trangle.m_vertexs[1];
        Vertex right = trangle.m_vertexs[2];

        int upY = top.y;
        int downY = left.y;

        float leftK = (float)(left.x - top.x) / (float)(upY - downY);
        float rightK = (float)(right.x - top.x) / (float)(upY - downY);

        float leftX = top.x;
        float rightX = top.x;

        Color32 color = m_sampler.Sampling(top.uv.x, top.uv.y);
        color.r = (byte)(color.r * top.intensity);
        color.g = (byte)(color.g * top.intensity);
        color.b = (byte)(color.b * top.intensity);
        drawPixel(top.x, top.y, color, top.position.z);

        Vector2 topPos = new Vector2(top.x, top.y);
        Vector3 topWorldPos = new Vector3(top.position.x, top.position.y, top.position.z);

        float leftEdgeLen = (new Vector2(left.x, left.y) - topPos).magnitude;
        float rightEdgeLen = (new Vector2(right.x, right.y) - topPos).magnitude;

        float leftWorldEdgeLen = (top.position - left.position).magnitude;
        float rightWorldEdgeLen = (top.position - right.position).magnitude;

        for ( int i = upY - 1; i >= downY; i-- )
        {
            leftX += leftK;
            rightX += rightK;

            int iLeftX = (int)(leftX);
            int iRightX = (int)(rightX);

            Vector2 midLeftPos = new Vector2( leftX, i );
            Vector2 midRightPos = new Vector2( rightX, i );

            float leftRatio = (midLeftPos - topPos).magnitude / leftEdgeLen;
            float rightRatio = (midRightPos - topPos).magnitude / rightEdgeLen;

            float midLeftRd = Mathf.Lerp(1.0f / top.position.z, 1.0f / left.position.z, leftRatio);         // 左边的1/z
            float midRightRd = Mathf.Lerp(1.0f / top.position.z, 1.0f / right.position.z, rightRatio);      // 右边的1/z

            Vector3 leftWorldPos = m_projector.ScreenToWorld(iLeftX, i, 1.0f / midLeftRd);
            Vector3 rightWorldPos = m_projector.ScreenToWorld(iRightX, i, 1.0f / midRightRd);
            float horiLen = (leftWorldPos - rightWorldPos).magnitude;

            float leftWorldRatio = (topWorldPos - leftWorldPos).magnitude / leftWorldEdgeLen;
            float rightWorldRatio = (topWorldPos - rightWorldPos).magnitude / rightWorldEdgeLen;

            Vector2 leftUV = Vector2.Lerp(top.uv, left.uv, leftWorldRatio);
            float leftIntensity = Mathf.Lerp(top.intensity, left.intensity, leftWorldRatio);

            Vector2 rightUV = Vector2.Lerp(top.uv, right.uv, rightWorldRatio);
            float rightIntensity = Mathf.Lerp(top.intensity, right.intensity, rightWorldRatio);

            for ( int j = iLeftX; j <= iRightX; j++ )
            {
                if (j < 0 || j >= m_width)
                    continue;

                Vector2 mid = new Vector2(j, i);

                float midRatio = (float)(j - leftX) / (rightX - leftX);
                float midRd = Mathf.Lerp( midLeftRd, midRightRd, midRatio );

                Vector3 originPos = m_projector.ScreenToWorld(j, i, 1.0f / midRd);

                float midRatio2 = (originPos - leftWorldPos).magnitude / horiLen;

                float intensity = Mathf.Lerp(leftIntensity, rightIntensity, midRatio2);
                Vector2 uv = Vector2.Lerp(leftUV, rightUV, midRatio2);

                color = m_sampler.Sampling(uv.x, uv.y);
                color.r = (byte)(color.r * intensity);
                color.g = (byte)(color.g * intensity);
                color.b = (byte)(color.b * intensity);

                drawPixel(j, i, color, originPos.z);
            }
        }
    }

    /// <summary>
    /// 光栅化平顶三角形
    /// </summary>
    /// <param name="trangle"></param>
    /// <param name="solidColor"></param>
    private void downTrangleRasterize( Trangle trangle )
    {
        sortVertex(trangle.m_vertexs);

        Vertex left = trangle.m_vertexs[0];
        Vertex right = trangle.m_vertexs[1];
        Vertex down = trangle.m_vertexs[2];

        int upY = left.y;
        int downY = down.y;

        float leftK = (float)(down.x - left.x) / (float)(upY - downY);
        float rightK = (float)(down.x - right.x) / (float)(upY - downY);

        float leftX = left.x;
        float rightX = right.x;

        Vector2 downPos = new Vector2(down.x, down.y);
        Vector3 downWorldPos = new Vector3(down.position.x, down.position.y, down.position.z);

        float leftEdgeLen = (new Vector2(left.x, left.y) - downPos).magnitude;
        float rightEdgeLen = (new Vector2(right.x, right.y) - downPos).magnitude;

        float leftWorldEdgeLen = (down.position - left.position).magnitude;
        float rightWorldEdgeLen = (down.position - right.position).magnitude;

        Color32 color;

        for ( int i = upY; i > downY; i-- )
        {
            int iLeftX = (int)(leftX);
            int iRightX = (int)(rightX);

            Vector2 midLeftPos = new Vector2(leftX, i);
            Vector2 midRightPos = new Vector2(rightX, i);

            float leftRatio = (midLeftPos - downPos).magnitude / leftEdgeLen;
            float rightRatio = (midRightPos - downPos).magnitude / rightEdgeLen;

            float midLeftRd = Mathf.Lerp(1.0f / down.position.z, 1.0f / left.position.z, leftRatio);         // 左边的1/z
            float midRightRd = Mathf.Lerp(1.0f / down.position.z, 1.0f / right.position.z, rightRatio);      // 右边的1/z

            Vector3 leftWorldPos = m_projector.ScreenToWorld(iLeftX, i, 1.0f / midLeftRd);
            Vector3 rightWorldPos = m_projector.ScreenToWorld(iRightX, i, 1.0f / midRightRd);
            float horiLen = (leftWorldPos - rightWorldPos).magnitude;

            float leftWorldRatio = (downWorldPos - leftWorldPos).magnitude / leftWorldEdgeLen;
            float rightWorldRatio = (downWorldPos - rightWorldPos).magnitude / rightWorldEdgeLen;

            Vector2 leftUV = Vector2.Lerp(down.uv, left.uv, leftWorldRatio);
            float leftIntensity = Mathf.Lerp(down.intensity, left.intensity, leftWorldRatio);

            Vector2 rightUV = Vector2.Lerp(down.uv, right.uv, rightWorldRatio);
            float rightIntensity = Mathf.Lerp(down.intensity, right.intensity,  rightWorldRatio);

            for (int j = iLeftX; j <= iRightX; j++)
            {
                if (j < 0 || j >= m_width)
                    continue;

                Vector2 mid = new Vector2(j, i);

                float midRatio = (float)(j - leftX) / (rightX - leftX);
                float midRd = Mathf.Lerp(midLeftRd, midRightRd, midRatio);

                Vector3 originPos = m_projector.ScreenToWorld(j, i, 1.0f / midRd);

                float midRatio2 = (originPos - leftWorldPos).magnitude / horiLen;

                float intensity = Mathf.Lerp(leftIntensity, rightIntensity, midRatio2);
                Vector2 uv = Vector2.Lerp(leftUV, rightUV, midRatio2);

                color = m_sampler.Sampling(uv.x, uv.y);
                color.r = (byte)(color.r * intensity);
                color.g = (byte)(color.g * intensity);
                color.b = (byte)(color.b * intensity);

                drawPixel(j, i, color, originPos.z);
            }

            leftX += leftK;
            rightX += rightK;
        }

        color = m_sampler.Sampling(down.uv.x, down.uv.y);
        color.r = (byte)(color.r * down.intensity);
        color.g = (byte)(color.g * down.intensity);
        color.b = (byte)(color.b * down.intensity);

        drawPixel(down.x, down.y, color, down.position.z);
    }

    /// <summary>
    /// 将三角形切分成上三角形和下三角形 
    /// </summary>
    /// <param name="trangle"></param>
    /// <returns></returns>
    private Trangle[] splitTrangle( Trangle trangle )
    {
        Trangle[] trangles = new Trangle[2];

        sortVertex(trangle.m_vertexs);

        Vertex upV = trangle.m_vertexs[0];
        Vertex midV = trangle.m_vertexs[1];
        Vertex downV = trangle.m_vertexs[2];

        Vertex midV2 = new Vertex();

        float ratio = (float)(upV.y - midV.y) / (float)(upV.y - downV.y);
        midV2.x = upV.x + Mathf.RoundToInt( (float)(downV.x - upV.x) * ratio );
        midV2.y = midV.y;

        float rd = Mathf.Lerp(1.0f/upV.position.z, 1.0f/downV.position.z, ratio);
        midV2.position = m_projector.ScreenToWorld(midV2.x, midV2.y, 1.0f / rd);

        float ratio2 = (upV.position - midV2.position).magnitude / (upV.position - downV.position).magnitude;
        midV2.color = Color32.Lerp(upV.color, downV.color, ratio2);
        midV2.uv = Vector2.Lerp(upV.uv, downV.uv, ratio2);
        midV2.intensity = Mathf.Lerp(upV.intensity, downV.intensity, ratio2);

        trangles[0] = new Trangle();
        trangles[0].m_vertexs = new Vertex[3] { upV, midV, midV2 };

        trangles[1] = new Trangle();
        trangles[1].m_vertexs = new Vertex[3] { midV, midV2, downV };

        return trangles;
    }

    /// <summary>
    /// 顶点按照从上往下从左往右的顺序排序
    /// </summary>
    /// <param name="vertices"></param>
    private void sortVertex( Vertex[] vertices )
    {
        // 顶点按照y来排序
        for (int i = 1; i <= 2; i++)
        {
            if (vertices[i].y > vertices[0].y || (vertices[i].y == vertices[0].y && vertices[i].x < vertices[0].x) )
            {
                Vertex temp = vertices[i];
                vertices[i] = vertices[0];
                vertices[0] = temp;
            }
        }

        if (vertices[2].y > vertices[1].y || ( vertices[2].y == vertices[1].y && vertices[2].x < vertices[1].x ) )
        {
            Vertex temp = vertices[1];
            vertices[1] = vertices[2];
            vertices[2] = temp;
        }
    }

    /// <summary>
    /// 是否是平底三角形
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
    /// 是否是平顶三角形
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
