using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmokeBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    ParticleSystem thisParticleSystem;
    private GameObject player;
    private GameObject smokeManager;
    private SmokeManager smokeSettings;

    //distances to player in that the particle system is active or not
    private float maxDistanceToPlayer = 20f;
    private float inSightDistanceToPlayer = 15f;
    private float minDistanceToPlayer = 5f;

    //new values from the Smokemanager
    private float newStartLifeTime;
    private float newStartSize;
    private float newParticleHight;

    //variable to check if the smoke values have been updated
    private bool smokeValuesUpdated = true; 

    //values for checking if ParticleSystem is in Field of View of Player
    public Camera mainCamera;
    Vector3 screenPoint;
    bool onScreen = false;

    void Start()
    {
        //set find all objects & components
        thisParticleSystem = GetComponent<ParticleSystem>();
        smokeManager = GameObject.Find("SmokeManager");
        player = GameObject.Find("FirstPersonPlayer");
        smokeSettings = smokeManager.GetComponent <SmokeManager> ();

        newStartLifeTime = smokeSettings.particleLifetime;
        newStartSize = smokeSettings.particleSize;
        newParticleHight = smokeSettings.particleHeight;
        setSmokeValues();

    }

    // Update is called once per frame
    void Update()
    {
        getCurrentValues();
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
        //set Smoke values if particle System is playing
        if ((thisParticleSystem.isPaused || thisParticleSystem.isStopped) && ((onScreen && currentDistance <= inSightDistanceToPlayer) ||
            currentDistance <= minDistanceToPlayer))
        {
            setSmokeValues();
            thisParticleSystem.Play();

        }
        else if ((onScreen == false || currentDistance > inSightDistanceToPlayer) && (thisParticleSystem.isPlaying || thisParticleSystem.isPaused))
        {
            if (currentDistance > maxDistanceToPlayer)
            {
                thisParticleSystem.Clear();
                thisParticleSystem.Stop();
            }
            else
            {
                thisParticleSystem.Pause();
            }
        }else if (thisParticleSystem.isPlaying)
        {
            setSmokeValues();
        }
    }

    //get new values for Lifetime, Size & Height of the particle System from SmokeManager, if they changed
    void getCurrentValues()
    {
        if (smokeSettings.smokeValuesHaveChanged)
        {
            newStartLifeTime = smokeSettings.particleLifetime;
            newStartSize = smokeSettings.particleSize;

            newParticleHight = smokeSettings.particleHeight;
           
            smokeValuesUpdated = true;     
        }
    }

    //set the new smoke values
    void setSmokeValues()
    {
        if(smokeValuesUpdated)
        {
            var main = thisParticleSystem.main;
            main.startLifetime = newStartLifeTime;
            main.startSize = new ParticleSystem.MinMaxCurve((newStartSize - 0.3f), newStartSize);
            transform.position = new Vector3(transform.position.x, newParticleHight, transform.position.z);
            smokeValuesUpdated = false;
        }
    }
}
