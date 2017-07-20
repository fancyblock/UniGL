using System.Collections.Generic;
using UnityEngine;


public interface IUniGL
{
    void Perspective(float d, float size = 1.0f);

    void Ortho(float size);

    void ClearColor(Color32 color);

    void Clear(bool colorBuffer, bool depthBuffer);

    void SetModelViewMatrix(Matrix4x4 modelViewMat);

    void BindTexture(int channel, Texture2D tex);

    void Draw(List<Vertex> vertexBuff, List<int> indexBuff, int trangleCount);

    void Present();

    void SetLight(ILight light);
}
