using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    public void Die(Vector3 pos)
    {
        Transform[] parts = GetComponentsInChildren<Transform>();
        foreach (Transform p in parts)
        {
            if(p.GetComponent<MeshRenderer>())
            {
                Rigidbody rb = p.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.AddExplosionForce(1000, pos, 0.1f);
                rb.transform.tag = "Object";
            }
            
        }
        Destroy(transform.GetComponent<NavMeshAgent>());
        Destroy(transform.GetComponent<NPCBehavior>());
        Destroy(transform.GetComponent<NPCData>());

        
    }
}
