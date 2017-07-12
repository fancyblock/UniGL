using UnityEngine;


public interface IProjector
{
    Matrix4x4 GetProjectionMatrix();

	void ProcessPosition( Vector4 pos, out int x, out int y );
}
