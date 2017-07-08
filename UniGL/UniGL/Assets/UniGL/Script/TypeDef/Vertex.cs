using UnityEngine;


public class Vertex 
{
    public Vector4 position;
    public Vector3 normal;
    public Color32 color;
    public Vector2 uv;

    public Vertex()
    {
        position = Vector2.zero;
        normal = Vector2.zero;
        color = Color.white;
        uv = Vector2.zero;
    }
}
