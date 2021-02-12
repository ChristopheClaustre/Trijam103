using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostProcessShader : MonoBehaviour
{
    public string shaderName = "ShaderExample";
    public DepthTextureMode depthTextureMode = DepthTextureMode.None;
    private Material material;

    public Material Material { get => material; }

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/"+shaderName));
    }

    private void Update()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
