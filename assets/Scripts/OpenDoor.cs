using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private ExperimentManager experimentManager;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private Text uiText;
    [SerializeField] private GameObject keyImage;

    private bool doorIsOpen = false;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (experimentManager.playerHasKey)
            {
                if (!doorIsOpen)
                {
                    doorIsOpen = true;
                    keyImage.gameObject.SetActive(false);
                    leftDoor.GetComponent<Animation>().Play();
                    rightDoor.GetComponent<Animation>().Play();
                    StartCoroutine(this.Emergency());
                }
            }
            else
            {
                StartCoroutine(this.SearchForKeys());
            }
        }
    }

    private IEnumerator SearchForKeys()
    {
        uiText.text = "Search for the key to open the door!";
        yield return new WaitForSeconds(5);
        uiText.text = "Objectives: \n "+
                      "- Get familiar with the controls \n" +
                      "- Find key to open the office Doors";
    }

    private IEnumerator Emergency()
    {
        yield return new WaitForSeconds(1);
        uiText.text = "An Emergency happened, leave the building as fast as possible!";
    }
}
