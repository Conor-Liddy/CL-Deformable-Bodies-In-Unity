/*
Author: Conor Liddy
Date: 19-10-2023
Desc: This file is used to break the GameObject into a 3D Array Mass.
*/

using System.Collections;
using UnityEngine;

public class Make3DArrayMass : MonoBehaviour
{
    public GameObject ParentObject; //Template game object to be replaced by soft-body
    public bool IsInstant; //Defines which creation method to use
    public float SpawnRateInSeconds; //Defines the spawn rate when not in instant mode
    public bool HasPhysics; //Defines whether the Voxels have a Rigidbody applied 

    public Vector3 SectionCount; //Stores the total amount of voxels
    public Material VoxelMaterial; //defines the material of each voxel

    private Vector3 SizeOfOriginalObject; //Expected size of total array in physical
    private Vector3 SectionSize; //Voxel size
    private Vector3 FillStartPosition; //Starting oint of array
    private Transform ParentTransform; //Transform of parent object 
    private GameObject Voxel; //This will store the voxels used in the array mass

    private GameObject[,,] ArrayMass3D; //This is a 3D array which will be used to store and manage the voxels 
    private MeshRenderer mr = new MeshRenderer(); // Used to store the MeshRenderer of the Voxels
    private Vector3[,,] VoxelOffset; // Stores the voxel offsets 
    private bool OffsetsSet = false; // boolean used to check if offsets are set 

    void Start()
    {
        if (ParentObject == null) //if nothing is attatched then use the object the script is linked to
        {
            ParentObject = gameObject;
        }

        if (VoxelMaterial == null) //if nothing is attatched then use the material of the object the script is linked to
        {
            VoxelMaterial = gameObject.GetComponent<Renderer>().material;
        }

        //Initialize The 3DArray
        int XVal = (int)SectionCount.x;
        int YVal = (int)SectionCount.y;
        int ZVal = (int)SectionCount.z;
        ArrayMass3D = new GameObject[XVal, YVal, ZVal];
        VoxelOffset = new Vector3[XVal, YVal, ZVal];

        //Define the section size
        SizeOfOriginalObject = ParentObject.transform.lossyScale;
        SectionSize = new Vector3(
            SizeOfOriginalObject.x / SectionCount.x,
            SizeOfOriginalObject.y / SectionCount.y,
            SizeOfOriginalObject.z / SectionCount.z
            );

        //Define the array start position 
        FillStartPosition = ParentObject.transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f))
                                + ParentObject.transform.TransformDirection(new Vector3(SectionSize.x, SectionSize.y, SectionSize.z) / 2.0f);

        //Get the current parent transform
        ParentTransform = new GameObject(ParentObject.name + "CubeParent").transform;

        //Define the render method
        if (IsInstant)
        {
            InstantDivide();
        }
        else
        {
            StartCoroutine(DivideIntoCubes());
        }

        RenderCheck();
        Debug.Log("End of Start");
    }

    //Figure out how to disable target cube colider 
    IEnumerator DivideIntoCubes() //This will spawn each voxel one-by-one for demonstration purposes
    {
        for (int i = 0; i < SectionCount.x; i++) //X
        {
            for (int j = 0; j < SectionCount.y; j++) //Y  
            {
                for (int k = 0; k < SectionCount.z; k++) //Z 
                {
                    //create Voxel
                    Voxel = CreateVoxel(i, j, k);

                    ArrayMass3D[i, j, k] = Voxel; //Add Voxel to 3D array mass

                    //Wait for set spawn rate
                    yield return new WaitForSeconds(SpawnRateInSeconds);
                } //Z
            } //Y
        }// X
        ParentObject.transform.localScale = new Vector3(0, 0, 0);
    }

    void InstantDivide() //This version will be used in engine and will instantly load all voxels
    {
        for (int i = 0; i < SectionCount.x; i++) //X
        {
            for (int j = 0; j < SectionCount.y; j++) //Y
            {
                for (int k = 0; k < SectionCount.z; k++) //Z
                {
                    //Create voxel
                    Voxel = CreateVoxel(i, j, k);

                    ArrayMass3D[i, j, k] = Voxel; //Add Voxel to 3D array mass
                } //Z
            } //Y
        }// X
        ParentObject.transform.localScale = new Vector3(0, 0, 0);

        Debug.Log("End of InstantDivide");
    }

    GameObject CreateVoxel(int i, int j, int k) //Called When Creating a new Voxel
    {
        GameObject Voxel;

        Voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Define the voxel
        Voxel.transform.localScale = SectionSize;
        Voxel.transform.position = FillStartPosition +
            ParentObject.transform.TransformDirection(new Vector3((SectionSize.x) * i, (SectionSize.y) * j, (SectionSize.z) * k));
        Voxel.transform.rotation = ParentObject.transform.rotation;

        // Define the parent and material
        Voxel.transform.SetParent(ParentTransform);
        Voxel.GetComponent<MeshRenderer>().material = VoxelMaterial;

        //Apply Rigidbody
        if (HasPhysics)
            Voxel.AddComponent<Rigidbody>();

        return Voxel;
    }

    void RenderCheck() // used to only render the outside faces of an object
    {
        for (int i = 0; i < SectionCount.x; i++) //X
        {
            for (int j = 0; j < SectionCount.y; j++) //Y
            {
                for (int k = 0; k < SectionCount.z; k++) //Z
                {
                    mr = ArrayMass3D[i, j, k].GetComponent<MeshRenderer>();
                    //Check if the voxel is on the outside face of the object
                    if (k != 0 && k != SectionCount.z-1 && j != 0 && j !=SectionCount.y-1 && i != 0 && i != SectionCount.x-1)
                    {
                        mr.enabled = false;
                    }
                } //Z
            } //Y
        }// X
    }

    void GetOffsets() // Sets the offset data for the Voxels
    {
        if (!OffsetsSet)
        {
            for (int i = 0; i < SectionCount.x; i++) //X
            {
                for (int j = 0; j < SectionCount.y; j++) //Y  
                {
                    for (int k = 0; k < SectionCount.z; k++) //Z 
                    {
                        if (i > 0)
                            VoxelOffset[i, j, k] = ArrayMass3D[i, j, k].transform.position - ArrayMass3D[i - 1, j, k].transform.position;
                        if (j > 0)
                            VoxelOffset[i, j, k] = ArrayMass3D[i, j, k].transform.position - ArrayMass3D[i, j - 1, k].transform.position;
                        if (k > 0)
                            VoxelOffset[i, j, k] = ArrayMass3D[i, j, k].transform.position - ArrayMass3D[i, j, k - 1].transform.position;
                    } //Z
                } //Y
            }// X

            OffsetsSet = true;
            Debug.Log("Offsets Set");
        }
    }

    void Link() // Set the Voxels to follow each other every frame 
    {
        if (OffsetsSet)
        {
            for (int i = 0; i < SectionCount.x; i++) //X
            {
                for (int j = 0; j < SectionCount.y; j++) //Y  
                {
                    for (int k = 0; k < SectionCount.z; k++) //Z 
                    {
                        if (i > 0)
                            ArrayMass3D[i, j, k].transform.position = ArrayMass3D[i - 1, j, k].transform.position + VoxelOffset[i, j, k];
                            //ArrayMass3D[i, j, k].transform.rotation = ArrayMass3D[i - 1, j, k].transform.rotation;
                        if (j > 0)
                            ArrayMass3D[i, j, k].transform.position = ArrayMass3D[i, j - 1, k].transform.position + VoxelOffset[i, j, k];
                           //ArrayMass3D[i, j, k].transform.rotation = ArrayMass3D[i, j - 1, k].transform.rotation;
                        if (k > 0)
                            ArrayMass3D[i, j, k].transform.position = ArrayMass3D[i, j, k - 1].transform.position + VoxelOffset[i, j, k];
                            //ArrayMass3D[i, j, k].transform.rotation = ArrayMass3D[i, j, k - 1].transform.rotation;
                    } //Z
                } //Y
            }// X
        }
    }








      //^^^^^^^^^^^^^^^^^^\\
     //--------------------\\
    //---Update Functions---\\

    void Update()
    {
        GetOffsets();
        Link();
    }
}
