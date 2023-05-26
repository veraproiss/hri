using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPickUp : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;

    [SerializeField] private GameObject key;
    [SerializeField] private Text uiText;
    [SerializeField] private GameObject keyImage;
    [SerializeField] private ExperimentManager experimentManager;
     public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            key.SetActive(false);
            experimentManager.playerHasKey = true;
            uiText.text = "You found the key! Proceed to the closed green office doors to proceed!";
            keyImage.SetActive(true);
        }
    }
}
