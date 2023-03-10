using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : MonoBehaviour
{
    Texture2D texColor;
    Texture2D texPosScale;
    VisualEffect vfx;

    uint resolution = 2048;

    public float particleSize = 0.1f;
    bool toUpdate = false;
    uint particleCount = 0;

    private void Start(){
        vfx = GetComponent<VisualEffect>();

        Vector3[] positions = new Vector3[(int)resolution];
        Color[] colors = new Color[(int)resolution];

        for (int x = 0; x < (int)resolution/2; x++){
            for (int y = 0; y < (int)resolution/2; y++) {
                positions[x + y] = new Vector3(Random.value * 10, Random.value * 10, Random.value * 10);
                colors[x + y] = new Color(Random.value, Random.value, Random.value, 1);
            }
        }

        SetParticles(positions, colors);
    }

    private void Update(){
        if(toUpdate){
            toUpdate = false;

            vfx.Reinit();
            vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particleCount);
            vfx.SetTexture(Shader.PropertyToID("TexColor"), texColor);
            vfx.SetTexture(Shader.PropertyToID("TexPosScale"), texPosScale);
            vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        }
    }

    private void SetParticles(Vector3[] positions, Color[] colors){
        texColor = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        texPosScale = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        int texWidth = texColor.width;
        int texHeight = texColor.height;

        for (int y = 0; y < texHeight; y++){
            for (int x = 0; x < texWidth; x++){
                int index = x + y * texWidth;
                texColor.SetPixel(x, y, colors[index]);
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, particleSize);
                texPosScale.SetPixel(x, y, data);
            }
        }

        texColor.Apply();
        texPosScale.Apply();
        particleCount = (uint)positions.Length;
        toUpdate = true;
    }
}
