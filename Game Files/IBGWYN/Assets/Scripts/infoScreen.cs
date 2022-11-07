using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoScreen : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Pull up", Input.GetKey(KeyCode.Tab));
    }
}
