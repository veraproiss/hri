using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeManager : MonoBehaviour
{
    //time interval in that the values for smoke are changed
    WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

    //variables to check if player has left trainings section
    public bool smokeIsActive = false;
    public bool firstActivation = true;
    
    //values to change for increased smoke (initialised in Start function, or have to be set in the editor)
    public float particleLifetime;
    public float particleSize;
    public float particleHeight;

    //steps in which to increase the values
    private float stepsParticleLifetime = 1f;
    private float stepsParticleSize = 0.1f;
    private float stepsParticleHeight = 0.1f;

    //variables to check if the smokevalues have changed (will be called from smokebehaviour)
    public bool smokeValuesHaveChanged = false;
    private bool resetSmokeBool = false;

    //max values for increased smoke
    private float maxParticleLifetime = 20f;
    private float maxParticleSize = 2.8f;
    private float minParticleHeight = 1.1f;
    private float thresholdParticleSize = 2f;

    //start values for increased smoke
    private float startParticleLifetime = 5f;
    private float startParticleSize = 1f;
    private float startHeight = 1.8f;

    //switch up the changing values (0 for LT, 1 for Size, 2 for Height)
    private int changeSettingNumber = 0;

    //variables to check if all values have reached their maximum
    private bool maxParticleSizeReached = false;
    private bool maxParticleLTReached = false;
    private bool minParticleHeightReached = false;

    //value for Smoke Density
    public float currentSmokeDensity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //set start values (to overwrite editor settings)
        particleLifetime = startParticleLifetime;
        particleSize = startParticleSize;
        particleHeight = startHeight;

}

    // Update is called once per frame
    void Update()
    {
        if(smokeIsActive && firstActivation)
        {
            StartCoroutine(IncreaseSmokeValues());
            firstActivation = false; 
        }
        resetChangedValue();
    }

    //resets the bool value that is called from smoke behaviour, to check if changes were made to the smoke variables
    void resetChangedValue()
    {
        if (smokeValuesHaveChanged && resetSmokeBool == false)
        {
            resetSmokeBool = true;
        }
        else if (smokeValuesHaveChanged && resetSmokeBool)
        {
            //update the current smoke Density value
            currentSmokeDensity = getSmokeDensity();
            smokeValuesHaveChanged = false;
        }
        else if (smokeValuesHaveChanged == false && resetSmokeBool)
        {
            resetSmokeBool = false;
        }

    }

    //calculate the value for the smoke density from the size, lifetime and y-position of the particle system
    //the size value is more noticable for the occlusion of the visual field and is therefore calculated as 3/5th of the total value
    float getSmokeDensity() {

        float GroundValueSize = maxParticleSize - startParticleSize;
        float PercentValueSize = particleSize - startParticleSize;
        float portionSize = (PercentValueSize / GroundValueSize)*(3f/5f);

        float GroundValueLT = maxParticleLifetime - startParticleLifetime;
        float PercentValueLT = particleLifetime - startParticleLifetime;
        float portionLT = (PercentValueLT / GroundValueLT) * (1f / 5f);

        float GroundValueHeight = 1 - (minParticleHeight - startHeight);
        float PercentValueHeight = 1- (particleHeight - startHeight);
        float portionHeight = (PercentValueHeight / GroundValueHeight) * (1f / 5f);

        float smokeDensity = portionSize + portionLT + portionHeight;
        return smokeDensity; 
    }
    IEnumerator IncreaseSmokeValues()
    {
        //increase smoke values every x seconds, if smoke is active
        while (true)
        {
                //switch only one setting at a time, starting with lifetime
                switch (changeSettingNumber)
                {
                    case 0:
                        if (particleLifetime < maxParticleLifetime)
                        {
                            particleLifetime += stepsParticleLifetime;
                            smokeValuesHaveChanged = true;
                        }
                        else if(maxParticleLTReached == false)
                        {
                            maxParticleLTReached = true;
                        }
                        break;
                    case 1:
                        if (particleSize < maxParticleSize)
                        {
                            particleSize += stepsParticleSize;
                            smokeValuesHaveChanged = true;

                        }
                        else if (maxParticleSizeReached == false)
                        {
                            maxParticleSizeReached = true;
                        }
                        break;
                    case 2:
                        if (particleSize > thresholdParticleSize && particleHeight > minParticleHeight)
                        {
                            particleHeight -= stepsParticleHeight;
                            smokeValuesHaveChanged = true;
                        }
                        else if (particleHeight < minParticleHeight && minParticleHeightReached == false)
                        {
                            minParticleHeightReached = true;
                        }
                        break;
                }
                //change to next setting
                changeSettingNumber += 1;
                changeSettingNumber %= 3;

                //stop coroutine, if all values have reached their maximum
                if(maxParticleLTReached && maxParticleSizeReached && minParticleHeightReached)
                {
                    yield break; 
                }
            
            yield return waitForSeconds;
        }
    }
}
