using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLight : MonoBehaviour
{

    [SerializeField] private Light redLight;
    [SerializeField] private Light blueLight;

    public float flickerOffset = 0.02f;
    private float nextSwitchTime = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        blueLight.gameObject.SetActive(false);
        redLight.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSwitchTime)
        {
            nextSwitchTime += flickerOffset;
            if (blueLight.gameObject.activeInHierarchy)
            {
                redLight.gameObject.SetActive(true);
                blueLight.gameObject.SetActive(false);
            }
            else
            {
                redLight.gameObject.SetActive(false);
                blueLight.gameObject.SetActive(true);
            }
            
        }
    }
}
