using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2Movement : MonoBehaviour
{

    [SerializeField] GameObject door;
    [SerializeField] AudioClip doorSqueak;


    Animator anim;
    AudioSource source;

    private void Start()
    {
        anim = door.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(anim == null)
        {
            print("animator has not been assigned.");
        }
        else if(other.gameObject.CompareTag("Player") && anim != null)
        {
            anim.SetTrigger("Open");
            source.PlayOneShot(doorSqueak);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (anim == null)
        {
            print("animator has not been assigned.");
        }
        else if (other.gameObject.CompareTag("Player") && anim != null)
        {
            anim.SetTrigger("Close");
            source.PlayOneShot(doorSqueak);
        }

    }

}
