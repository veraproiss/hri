using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource sound;
    private bool playedOnce;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();

        playedOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(playedOnce)){ 

            sound.Play();

            playedOnce = true;
        }

    }
}
