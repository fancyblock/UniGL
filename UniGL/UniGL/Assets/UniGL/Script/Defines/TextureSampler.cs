using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用于纹理采样 
/// </summary>
public class TextureSampler 
{
	private Dictionary<int,Texture2D> m_texDic = new Dictionary<int, Texture2D>();
    private Texture2D m_defaultTexture;


    /// <summary>
    /// 设置贴图
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tex"></param>
	public void SetTexture( int channel, Texture2D tex )
	{
		if( m_texDic.ContainsKey(channel) )
			m_texDic[channel] = tex;
		else
			m_texDic.Add(channel, tex);

        if (channel == 0)
            m_defaultTexture = tex;
	}

    /// <summary>
    /// 像素采样 
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    public Color32 Sampling( float u, float v )
    {
        return m_defaultTexture.GetPixelBilinear(u, v);            //[TEMP]
    }
}
