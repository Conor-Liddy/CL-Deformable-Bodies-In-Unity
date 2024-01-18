
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class SpringSystem : MonoBehaviour
{
//Public
    public float WaveSpeed = 1;
    public bool SineWave;
    public bool RippleWave;
    //Private
    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;

    private void Start()
    {
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
    }


    private void Update()
    {
        if (SineWave)
        {
            LeftToRightSine();
        }
        if (RippleWave)
        {
            RippleSine();
        }

        MeshCollider.sharedMesh = MeshFilter.sharedMesh;
    }

    void RippleSine()
    {
        var mesh = MeshFilter.mesh;
        var Vertices = mesh.vertices;
        
        for (int i = 0; i < Vertices.Length; i++)
        {
            float elevation = Mathf.Sin((Time.time * WaveSpeed) + (Vertices[i].x + Vertices[i].z));
            Vertices[i].y = elevation;
        }
       
        mesh.vertices = Vertices;
    }

    void LeftToRightSine()
    {
        var mesh = MeshFilter.mesh;
        var Vertices = mesh.vertices;

        for (int i = 0; i < Vertices.Length; i++)
        {
            float elevation = Mathf.Sin((Time.time * WaveSpeed) - Vertices[i].x);
            Vertices[i].y = elevation;
        }

        mesh.vertices = Vertices;
    }
}