using UnityEngine;


public class UniGL_Ex : MonoBehaviour
{
    public int m_width;
    public int m_height;

    private UniGL m_uniGL;

	// Use this for initialization
	void Awake ()
    {
        // create camera 
        GameObject go = new GameObject();
        go.name = "Renderer";

        Camera camera = go.AddComponent<Camera>();

        camera.orthographic = true;
        camera.orthographicSize = 1.0f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;

        camera.nearClipPlane = -10;
        camera.farClipPlane = 100;

        camera.useOcclusionCulling = false;
        camera.allowHDR = false;
        camera.allowMSAA = false;

        // create sprite render
        SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();

        // create texture 
        Texture2D texture = new Texture2D( m_width, m_height, TextureFormat.RGBA32, false, false);

        // create texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f), 1);

        spriteRender.sprite = sprite;

        // adjust size scale 
        if (Screen.width > Screen.height)
            camera.orthographicSize = texture.height / 2;
        else
            camera.orthographicSize = texture.height;             ///////[TEMP]

        m_uniGL = new UniGL(texture);
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_uniGL.SetModelViewMatrix(Matrix4x4.identity);

        //TODO 

        m_uniGL.Present();
	}
}
