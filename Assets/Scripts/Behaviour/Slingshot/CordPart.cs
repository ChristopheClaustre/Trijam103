using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordPart : MonoBehaviour
{
    private Transform start;
    public Transform end;

    public float width;
    public float height;
    public Material material;

    private Mesh mesh;
    public BoxCollider boxCollider;

    float halfWidth;
    float halfHeight;

    // Start is called before the first frame update
    void Start()
    {
        start = transform;

        mesh = new Mesh();
        mesh.MarkDynamic();
        
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        halfWidth  = width / 2;
        halfHeight = height / 2;
    }

    void FixedUpdate()
    {
        if (!end)
            return;

        //Vector3 difference = end.localPosition - start.localPosition;
        Matrix4x4 frameEndToWorld = end.localToWorldMatrix; // end to world
        Matrix4x4 frameWorldToStart = start.worldToLocalMatrix; // world to start

        Vector3[] vertices = new Vector3[]
        {
            // face on start with normal -Z
            new Vector3(-halfWidth,-halfHeight,0),
            new Vector3(halfWidth,-halfWidth,0),
            new Vector3(-halfWidth,halfWidth,0),
            new Vector3(halfWidth,halfWidth,0),

            // face on end with normal +Z
            new Vector3(),
            new Vector3(),
            new Vector3(),
            new Vector3(),
        };
        for (int i = 4; i < 8; i++)
            vertices[i] = frameWorldToStart.MultiplyPoint3x4(frameEndToWorld.MultiplyPoint3x4(vertices[i - 4]));

        int[] indices = new int[]
        {
            // -z
            0,1,2,
            1,3,2,

            // +y
            2,3,6,
            3,7,6,

            // +z
            6,7,4,
            7,5,4,

            // -y
            4,5,1,
            4,1,0,

            // +x
            4,0,2,
            4,2,6,

            // -x
            3,5,7,
            3,1,7
        };

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
    }
}
