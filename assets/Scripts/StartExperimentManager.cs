using System;
using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartExperimentManager : MonoBehaviour
{

    public InputField participantNoInput;

    public TMPro.TMP_Dropdown conditionDropdown;

    public Button startButton;


    private Condition condition;
    private string participantNo;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public string GetParticipantNumberAsStr()
    {
        Debug.Log(String.Format("Participant No: {0}", participantNo));
        return participantNo;
    }

    public Condition GetExpCondition()
    {

        Debug.Log(String.Format("Experiment Condition: {0}", condition.ToString()));
        return condition;
    }

    public void StartExperimentScene()
    {
        participantNo = participantNoInput.text;
        condition =  Condition.None;
        
        if (conditionDropdown.options[conditionDropdown.value].text == "AudioBot")
        {
            condition = Condition.AudioRobot;
        } else if (conditionDropdown.options[conditionDropdown.value].text == "VisualBot")
        {
            condition = Condition.VisualRobot;
        }
        
        SceneManager.LoadScene(1);
    }
}
