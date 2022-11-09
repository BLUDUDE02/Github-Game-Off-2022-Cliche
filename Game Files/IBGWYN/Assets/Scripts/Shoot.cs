using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Settings")]
    public float range = 100;
    public int ammo = 6;
    public GameObject bulletHole;
    public GameObject bulletHole2;
    public ParticleSystem flash;
    public Animator anim;
    public LayerMask layer;
    public AudioSource source;

    bool canShoot = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0 && canShoot && Time.timeScale > 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        flash.Play();
        source.Play();
        anim.SetTrigger("Fire");
        ammo -= 1;
        canShoot = false;
        RaycastHit hitinfo;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitinfo, range, layer))
        {
            Debug.Log(hitinfo.transform.name);
            if (hitinfo.transform.tag == "NPC")
            {
                Instantiate(bulletHole2, hitinfo.point, Quaternion.LookRotation(hitinfo.normal), hitinfo.transform);
                hitinfo.transform.GetComponentInParent<NPCBehavior>().Die(hitinfo.point);
            }
            else if(hitinfo.transform.tag == "Object")
            {
                Instantiate(bulletHole2, hitinfo.point, Quaternion.LookRotation(hitinfo.normal), hitinfo.transform);
                hitinfo.transform.GetComponentInParent<Rigidbody>().AddExplosionForce(1000, hitinfo.point, 0.1f);
            }
            else
            {
                Instantiate(bulletHole, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            }
            
        }
        StartCoroutine(cooldown());
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(0.125f);
        canShoot = true;
    }
}
