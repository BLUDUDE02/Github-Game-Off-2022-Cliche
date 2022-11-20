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
    public Camera cam;

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
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, range, layer))
        {
            Debug.Log(hitinfo.transform.name);
            if (hitinfo.transform.CompareTag("NPC"))
            {
                Instantiate(bulletHole2, hitinfo.point, Quaternion.LookRotation(hitinfo.normal), hitinfo.transform);
                if (hitinfo.transform.GetComponentInParent<NPCData>().isTarget)
                {
                    GetComponent<Interaction>().Won();
                }
                hitinfo.transform.GetComponentInParent<NPCBehavior>().Die(hitinfo.point);
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
