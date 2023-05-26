using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    
    private GameObject player;
    //fireManager is not used so far
    private GameObject fireManager;
    private FireManager fireSettings;

    //distances to player in that the particle system is active or not
    private float maxDistanceToPlayer = 20f;
    private float inSightDistanceToPlayer = 15f;
    private float minDistanceToPlayer = 5f;

    //values for checking if ParticleSystem is in Field of View of Player
    public Camera mainCamera;
    Vector3 screenPoint;
    bool onScreen = false;

    private Light thisLight;

    void Start()
    {
        //set find all objects & components
        fireManager = GameObject.Find("FireManager");
        fireSettings = fireManager.GetComponent<FireManager>();
        player = GameObject.Find("FirstPersonPlayer");
        thisLight = GetComponentInChildren<Light>();
        if(thisLight != null)
        {
            thisLight.enabled = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        ParticleSupervising();

    }


    void ParticleSupervising()
    {

        //check if ParticleSystem is In Field of View
        screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //get current Distance to Player
        var currentDistance = Vector3.Distance(player.transform.position, transform.position);
        //if Particle System is in Field of View && Player is close, play Particle System, else Stop & Clear
        if ((onScreen && currentDistance <= inSightDistanceToPlayer) || currentDistance <= minDistanceToPlayer)
        {

            foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
            {
                if ((particleSystem.isStopped || particleSystem.isPaused))
                {
                    particleSystem.Play();
                }
            }

            if(thisLight != null && thisLight.enabled == false)
            {
                thisLight.enabled = true;
            }

        }
        else if (onScreen == false || currentDistance > inSightDistanceToPlayer)
        {
            if (currentDistance > maxDistanceToPlayer)
            {
                foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
                {
                    if (particleSystem.isPlaying || particleSystem.isPaused)
                    {
                        particleSystem.Clear();
                        particleSystem.Stop();
                    }
                }

                if (thisLight != null && thisLight.enabled == true)
                {
                    thisLight.enabled = false;
                }

            }
            else
            {
                foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
                {
                    if (particleSystem.isPlaying)
                    {
                        particleSystem.Pause();
                    }
                }

                if (thisLight != null && thisLight.enabled == true)
                {
                    thisLight.enabled = false;
                }
            }
        }

    }
}
