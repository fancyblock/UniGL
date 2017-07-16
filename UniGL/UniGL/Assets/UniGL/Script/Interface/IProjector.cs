using UnityEngine;


public interface IProjector
{
	void ProcessPosition( Vector4 pos, out int x, out int y );

    bool IsPerspective();
}
