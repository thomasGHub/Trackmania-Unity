using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //FindObjectOfType<AudioManager>().Play("Electric");
            AudioManager.instance.Play("Explosion");
            AudioManager.instance.Play("Fire");
        }
    }
}