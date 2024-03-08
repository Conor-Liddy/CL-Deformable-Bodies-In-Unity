using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCollider : MonoBehaviour
{
    bool ba = Make3DArrayMass.BreakApart;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Voxel")
        {
            GameObject Voxel = collision.gameObject;

            if (!ba)
                Physics.IgnoreCollision(Voxel.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
