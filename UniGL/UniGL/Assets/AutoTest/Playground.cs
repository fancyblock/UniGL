using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    public Color32 m_color;
    public SpriteRenderer m_sprite;
    public int value;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (value < 0)
            value = 0;
        if (value > 100)
            value = 100;

        float val = (float)value / 100.0f;

        m_sprite.color = Color32.Lerp(Color.black, m_color, val);
	}
}
