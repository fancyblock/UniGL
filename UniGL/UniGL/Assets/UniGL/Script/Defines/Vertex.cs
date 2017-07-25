using UnityEngine;


/// <summary>
/// 顶点
/// </summary>
public class Vertex 
{
    public Vector4 position;
    public Vector3 normal;
    public Color32 color;
    public Vector2 uv;
    public float intensity;

    public int x;
    public int y;


    public Vertex()
    {
        position = Vector2.zero;
        position.w = 1;
        normal = Vector2.zero;
        color = Color.white;
        uv = Vector2.zero;
    }

	public Vertex( Vertex vertex )
	{
		position = vertex.position;
		normal = vertex.normal;
		color = vertex.color;
		uv = vertex.uv;
	}

    public Vertex( Vector3 _pos )
    {
        position = _pos;
        position.w = 1;
        normal = Vector2.zero;
        color = Color.white;
        uv = Vector2.zero;
    }

    public Vertex( Vector3 _pos, Vector2 _uv )
    {
        position = _pos;
        position.w = 1;
        normal = Vector2.zero;
        color = Color.white;
        uv = _uv;
    }

    public Vertex( Vector3 _pos, Vector2 _uv, Vector3 _normal )
    {
        position = _pos;
        position.w = 1;
        normal = _normal;
        uv = _uv;
        color = Color.white;
    }
}
