using UnityEngine;


public interface IProjector
{
	void CalculateProjection( Vector4 pos, out int x, out int y );

    bool IsPerspective();
}
