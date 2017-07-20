using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用于纹理采样 
/// </summary>
public class TextureSampler 
{
	private Dictionary<int,Texture2D> m_texDic = new Dictionary<int, Texture2D>();


	public void SetTexture( int channel, Texture2D tex )
	{
		if( m_texDic.ContainsKey(channel) )
			m_texDic[channel] = tex;
		else
			m_texDic.Add(channel, tex);
	}

    //TODO 

}
