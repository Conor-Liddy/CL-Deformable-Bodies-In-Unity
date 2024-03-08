using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class SpringPlaneDemo : MonoBehaviour
{
    Mesh PlaneMesh;
    MeshFilter MeshFilter;

    public Vector2 PlaneSize = new Vector2(1, 1);
    public int PlaneRes = 1;
    public float WaveSpeed;
    public bool SineWave;
    public bool RippleWave;

    List<Vector3> Vertices;
    List<int> Triangles;

    // Start is called before the first frame update
    void Awake()
    {
        PlaneMesh = new Mesh();
        MeshFilter = GetComponent<MeshFilter>();
        MeshFilter.mesh = PlaneMesh;
    }

    // Update is called once per frame
    void Update()
    {
        PlaneRes = Mathf.Clamp(PlaneRes, 1, 50);

        GeneratePlane(PlaneSize, PlaneRes);
        if (SineWave)
        {
            LeftToRightSine(Time.timeSinceLevelLoad * WaveSpeed);
        }
        if (RippleWave)
        {
            RippleSine(Time.timeSinceLevelLoad * WaveSpeed);
        }

        AssignMesh();
    }

    void GeneratePlane(Vector2 Size, int Res)
    {
        //Nodes
        Vertices = new List<Vector3>();
        float XPerStep = Size.x / Res;
        float YPerStep = Size.y / Res;

        for (int y = 0; y < Res + 1; y++)
        {
            for (int x = 0; x < Res + 1; x++)
            {
                Vertices.Add(new Vector3(x * XPerStep, 0, y * YPerStep));
            }
        }

        //Springs
        Triangles = new List<int>();
        for (int row = 0; row < Res; row++)
        {
            for (int col = 0; col < Res; col++)
            {
                int i = (row * Res) + row + col;
                Triangles.Add(i);
                Triangles.Add(i + (Res) + 1);
                Triangles.Add(i + (Res) + 2);

                Triangles.Add(i);
                Triangles.Add(i + (Res) + 2);
                Triangles.Add(i + 1);
            }
        }
    }

    void AssignMesh()
    {
        PlaneMesh.Clear();
        PlaneMesh.vertices = Vertices.ToArray();
        PlaneMesh.triangles = Triangles.ToArray();
    }

    void LeftToRightSine(float Time)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vector3 Vertex = Vertices[i];
            Vertex.y = Mathf.Sin(Time + Vertex.x); 
            Vertices[i] = Vertex;
        }
    }

    void RippleSine(float Time)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vector3 Vertex = Vertices[i];
            Vertex.y = Mathf.Sin(Time + (Vertex.x + Vertex.z));
            Vertices[i] = Vertex;
        }
    }
}
