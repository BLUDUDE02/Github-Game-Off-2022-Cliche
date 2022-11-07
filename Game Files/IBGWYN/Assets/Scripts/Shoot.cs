using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Settings")]
    public float range = 100;
    public int ammo = 6;
    public GameObject bulletHole;
    public ParticleSystem flash;
    public Animator anim;
    public LayerMask layer;
    public AudioSource source;

    bool canShoot = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0 && canShoot)
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
            Instantiate(bulletHole, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
        }
        StartCoroutine(cooldown());
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(0.125f);
        canShoot = true;
    }
}
