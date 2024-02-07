using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]

public class SpringSystem : MonoBehaviour
{
//Public
    //UI Elements
    public bool SineWave;
    public bool RippleWave;
    public float WaveSpeed = 1;
    public float WaveHeight = 1;
    public float PlaneRes = 1;
//Private
    //Mesh Variables
    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;
    private List<Vector3> SourceVertices;
    private Vector3[] Normals;
    private Mesh mesh;
    private List<Quaternion> Rotations;

    private void Start()
    {
        //Get Components
        mesh = GetComponent<MeshFilter>().mesh;
        Normals = mesh.normals;
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
        SourceVertices = new List<Vector3>();
    }


    private void Update()
    {
        //Choose Wave Type
        if (SineWave)
        {
            LeftToRightSine();
        }
        if (RippleWave)
        {
            RippleSine();
        }

        //Update Mesh Collider
        MeshCollider.sharedMesh = MeshFilter.sharedMesh;
    }

    void RippleSine()
    {
        var meshFil = MeshFilter.mesh;
        var Vertices = meshFil.vertices;

        PopulateSourceVertices(); //Set Initial Values
        SetRotations(); //Set Rotation values for each vertices

        //Wave Loop
        for (int i = 0; i < Vertices.Length; i++)
        {
            float elevation = Mathf.Sin((Time.time * WaveSpeed) + (Vertices[i].x + Vertices[i].z) * PlaneRes);
            Vector3 NewPos = new Vector3(Vertices[i].x, elevation, Vertices[i].z);

            Vertices[i].y = (NewPos.y * WaveHeight) + SourceVertices[i].y;

            var v = (Vector3)Vertices[i] - SourceVertices[i];

            v = Rotations[i] * v;
        }

        meshFil.vertices = Vertices;
    }

    void LeftToRightSine()
    {
        var meshFil = MeshFilter.mesh;
        var Vertices = meshFil.vertices;

        PopulateSourceVertices(); //Set Initial Values
        SetRotations(); //Set Rotation values for each vertices

        //Wave Loop
        for (int i = 0; i < Vertices.Length; i++)
        {
            float elevation = Mathf.Sin((Time.time * WaveSpeed) - Vertices[i].x * PlaneRes);
            Vector3 NewPos = new Vector3(Vertices[i].x, elevation, Vertices[i].z);

            Vertices[i].y = (NewPos.y * WaveHeight) + SourceVertices[i].y;
        }

        meshFil.vertices = Vertices;
    }

    void PopulateSourceVertices()
    {
        var mesh = MeshFilter.mesh;
        var Vertices = mesh.vertices;

        for (int i = 0; i < Vertices.Length; i++)
        {
            SourceVertices.Add(Vertices[i]);
        }
    }

    void SetRotations()
    {
        for (int i = 0; i < Normals.Length; i++)
        {
            //Rotations.Add((Normals[i]);
        }
    }
}