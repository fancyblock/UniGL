using UnityEngine;


public interface IProjector
{
	void WorldToScreen( Vector4 pos, out int x, out int y );

    Vector3 ScreenToWorld(int x, int y, float worldZ);

    bool IsPerspective();
}
