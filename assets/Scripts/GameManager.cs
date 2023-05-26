using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject smokeManager;
    public GameObject fireManager;
    public bool tutorialHasEnded = false;
    public bool firstStart = true;

    
    // Start is called before the first frame update
    void Start()
    {
        smokeManager.SetActive(false);
        fireManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialHasEnded && firstStart)
        {
            smokeManager.SetActive(true);
            smokeManager.GetComponent<SmokeManager>().smokeIsActive = true; 
            fireManager.SetActive(true);
            fireManager.GetComponent<FireManager>().fireIsActive = true;
            firstStart = false;
            //smokeManager = FindObjectOfType<SmokeManager>(true);
        }    
    }
}
