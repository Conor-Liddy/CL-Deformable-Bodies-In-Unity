using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]

public class SpringSystem : MonoBehaviour
{
//Public
    //UI Elements
    public bool SineWave;
    public bool RippleWave;
    public bool Deformable;
    public float WaveSpeed = 1;
    public float WaveHeight = 1;
    public float PlaneRes = 1;
    public float DeformationAmmount = 1.0f;
//Private
    //Mesh Variables
    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;
    private List<Vector3> SourceVertices;
    private Vector3[] Normals;
    private Mesh mesh;

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
        if(!Deformable)
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
        }

        //Update Mesh Collider
        MeshCollider.sharedMesh = MeshFilter.sharedMesh;
    }

    void RippleSine()
    {
        var meshFil = MeshFilter.mesh;
        var Vertices = meshFil.vertices;

        PopulateSourceVertices(); //Set Initial Values

        //Wave Loop
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vector3 CurrentNormal = Normals[i];

            float sineTerm = Mathf.Sin((Time.time * WaveSpeed) + (Vertices[i].x + Vertices[i].z) * PlaneRes);
            Vector3 NewPos = SourceVertices[i] + CurrentNormal * (sineTerm * WaveHeight);

            Vertices[i] = NewPos;
             
        }

        meshFil.vertices = Vertices;
    }

    void LeftToRightSine()
    {
        var meshFil = MeshFilter.mesh;
        var Vertices = meshFil.vertices;

        PopulateSourceVertices(); //Set Initial Values

        //Wave Loop
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vector3 CurrentNormal = Normals[i];

            float sineTerm = Mathf.Sin((Time.time * WaveSpeed) - Vertices[i].x * PlaneRes);
            Vector3 NewPos = SourceVertices[i] + CurrentNormal * (sineTerm * WaveHeight);

            Vertices[i] = NewPos;
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

    // Detect collisions on deformable object
    void OnCollisionEnter(Collision collision)
    {
        Vector3 HitVert = NearestVertex(collision.transform.position); //Get closest Vertex to colliding object
        
        var meshFil = MeshFilter.mesh;
        var Vertices = meshFil.vertices;

        PopulateSourceVertices();

        for (int i = 0; i < Vertices.Length; i++)
        {

            Vector3 CurrentNormal = Normals[i];

            if (Vertices[i] == HitVert) //find matching vertex from list to the one which was hit
            {
                    Vector3 NewPos = SourceVertices[i] + CurrentNormal * (DeformationAmmount - (DeformationAmmount * 2));
                    Vertices[i] = NewPos;
            }
                
        }

        meshFil.vertices = Vertices;
    }

    //returns the nearest point to 
    public Vector3 NearestVertex(Vector3 point)
    {
        // convert point to local space
        point = transform.InverseTransformPoint(point);

        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;

        // scan all vertices to find nearest
        foreach (Vector3 vertex in mesh.vertices)
        {
            Vector3 diff = point - vertex;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }
        
        return nearestVertex;
    }
}




/*
 * Helpful Advice and notes:
 * 
 * 1.
 * "A vertex and its normal are stored together.
 * This means different normal, must be a different vertex.
 * This means if you have faceted object (not smooth shaded), each corner will have more than one vertex, which will be at the same position, but have a different normal.
 * To keep a face connected you need to move shared vertices the same amount.
 * In your case you would probably need to aggregate all verts that are at a corner and then compute their average normal and use that for your offsetting."
 * - Kurt-Dekker
 *
 * 2.
 * Figure out how to further subdivide the mesh to help with cases such as the cube mesh which only has 2 tris on each side.
 * Makes for janky movement and wont allow for good deformation
 * 
 * 
 * must get 2 working in order to fix 1
 * Both one and 2 are make it best features
 * 
 * 
 * 
 * Current State: Working!
 */
