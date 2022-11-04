using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Settings")]
    public float range = 100;
    public GameObject bulletHole;
    public ParticleSystem flash;
    public Animator anim;
    public LayerMask layer;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        flash.Play();
        anim.SetTrigger("Fire");
        RaycastHit hitinfo;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitinfo, range, layer))
        {
            Debug.Log(hitinfo.transform.name);
            Instantiate(bulletHole, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
        }

    }
}
