using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataModel;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ExperimentManager : MonoBehaviour
{
    //List of data recordings (filled every 3s)
    private List<ExpDataModel> expData = new List<ExpDataModel>();

    //Holding a direct reference on player
    [SerializeField] private GameObject player;
    [SerializeField] private RoomMonitoring roomMonitoring;
    [SerializeField] private MoveToTarget robot;
    [SerializeField] private SmokeManager smokeManager;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject visualBot;
    [SerializeField] private GameObject audioBot;

    public Condition condition;
    public GameObject soundBotTrigger;
    
    
    [SerializeField] private string savePath;
    
    private string participantId = "NONE";
    
    private float nextRecordTime = 0.0f;
    public float recordInterval = 2f;
    private float nextWriteOutTime = 10.0f;
    public float writeOutInterval = 10f;

    public bool playerHasKey { get; set; } = false;

    private StartExperimentManager startExperimentManager;

    // Start is called before the first frame update
    void Start()
    {
        startExperimentManager = GameObject.FindObjectOfType<StartExperimentManager>();
        
        //test csv 
        savePath = Application.dataPath + "/ExperimentData/test.tsv";
        EnsureDirectoryExists(savePath);
        //Debug.Log("Experiment Data Test File Path: " + savePath);

        
        using (StreamWriter writer = new StreamWriter(savePath))
        {
            writer.WriteLine("test");
            writer.Flush(); 
        }


        if (startExperimentManager.GetExpCondition() == Condition.VisualRobot)
        {
            visualBot.gameObject.SetActive(true);
            audioBot.gameObject.SetActive(false);
            condition = Condition.VisualRobot;
            soundBotTrigger.gameObject.SetActive(false);
        }
        else if(startExperimentManager.GetExpCondition() == Condition.AudioRobot)
        {
            audioBot.gameObject.SetActive(true);
            visualBot.gameObject.SetActive(false);
            condition = Condition.AudioRobot;
            soundBotTrigger.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("What is this condition?");
            condition = Condition.None;
        }

        participantId = startExperimentManager.GetParticipantNumberAsStr();

        //Init tsv file
        WriteOutHead();
    }

    // Update is called once per frame
    void Update()
    {
        //record data
        if (Time.time > nextRecordTime)
        {
            nextRecordTime += recordInterval;
            if (gameManager.tutorialHasEnded)
            {
                SaveDataRecord(isTutorial: false);
            }
            else
            {
                SaveDataRecord(isTutorial: true);
            }
           
        }
        
        //write out data every 30s (or on key press)
        if (Input.GetKeyDown(KeyCode.F))
        {
            WriteOutExpData();
        }
        if (Time.time > nextWriteOutTime)
        {
            nextWriteOutTime += writeOutInterval;
            WriteOutExpData();
        }
        
        //Debug.Log(String.Format("ExpRecord Count: {0}", expData.Count));
    }
    
    private void EnsureDirectoryExists(string filePath) 
    { 
        FileInfo fi = new FileInfo(filePath);
        if (!fi.Directory.Exists) 
        { 
            System.IO.Directory.CreateDirectory(fi.DirectoryName); 
        } 
    }

    private void SaveDataRecord(bool isTutorial)
    {
        if (!isTutorial)
        {
            //Record Experiment Data, will be called every 3s 
        
            //initiate one set of data
            DateTime timestamp = DateTime.Now;
            string currentRoom = roomMonitoring.GetCurrentRoom();
            Coordinate playerPosition = new Coordinate(player.transform.position.x, player.transform.position.z);
            
            Coordinate robotPosition = new Coordinate(robot.transform.position.x, robot.transform.position.z);
            float p2DistanceToRobot = Vector2.Distance(new Vector2(playerPosition.x, playerPosition.z), 
                                                        new Vector2(robotPosition.x, robotPosition.z));

            string stage = "experiment";
            string expCondition = condition.ToString();
            
            float robotWaitTime = robot.waitTime;
            float idealTrackDistance = robot.GetPlayerDistanceToTrack(robot.track);
            float particleDensity = smokeManager.currentSmokeDensity;
            
            
            ExpDataModel newRecord =
                new ExpDataModel(timestamp, 
                                 currentRoom, 
                                 playerPosition, 
                                 robotPosition, 
                                 p2DistanceToRobot, 
                                 stage, 
                                 expCondition,
                                 robotWaitTime,
                                 idealTrackDistance,
                                 particleDensity);
            
            expData.Add(newRecord);
        }
        // During the Tutorial only player position and time will be recorded
        else
        {
            //initiate one set of data
            DateTime timestamp = DateTime.Now;
            string currentRoom = roomMonitoring.GetCurrentRoom();
            Coordinate playerPosition = new Coordinate(player.transform.position.x, player.transform.position.z);
            
            //Todo
            Coordinate robotPosition = new Coordinate(-1f, -1f);
            float p2DistanceToRobot = -1f;

            string stage = "tutorial";
            string expCondition = condition.ToString();
            
            //Todo
            float robotWaitTime = robot.waitTime;
            float idealTrackDistance = robot.GetPlayerDistanceToTrack(robot.track);
            float particleDensity = -1f;
            
            
            ExpDataModel newRecord =
                new ExpDataModel(timestamp, 
                    currentRoom, 
                    playerPosition, 
                    robotPosition, 
                    p2DistanceToRobot, 
                    stage, 
                    expCondition,
                    robotWaitTime,
                    idealTrackDistance,
                    particleDensity);
            
            expData.Add(newRecord);
        }
    }

    private void WriteOutHead()
    {
        savePath = Application.dataPath + "/ExperimentData/experiment_" + participantId + ".tsv"; 
        EnsureDirectoryExists(savePath);

        using (StreamWriter writer = new StreamWriter(savePath))
        {
            writer.WriteLine("Timestamp\t" +
                             "Room\t" +
                             "PlayerX\t" +
                             "PlayerZ\t" +
                             "RobotX\t" +
                             "RobotZ\t" +
                             "P2RDistance\t" +
                             "RobotWaitTime\t" +
                             "IdealTrackDistance\t" +
                             "ParticleDensity\t" +
                             "ExpStage\t" +
                             "ExpCondition");
            
            writer.Flush();
        }
    }
    public void WriteOutExpData()
    {
        string delimiter = "\t";        

        using (StreamWriter writer = new StreamWriter(savePath, append: true))
        {
            foreach (ExpDataModel record in expData)
            {
                writer.WriteLine(record.timestamp + delimiter
                                 + record.room + delimiter
                                 + record.playerPosition.x + delimiter
                                 + record.playerPosition.z + delimiter
                                 + record.robotPosition.x + delimiter
                                 + record.robotPosition.z + delimiter
                                 + record.p2DistanceToRobot + delimiter
                                 + record.robotWaitTime + delimiter
                                 + record.idealTrackDistance + delimiter
                                 + record.particleDensity + delimiter
                                 + record.expStage + delimiter
                                 + record.condition);
            }
            
            writer.Flush();
        }

        //Debug.Log(String.Format("Experiment Data File Path of Participant {0} : {1} ", participantId, savePath));
        
        expData.Clear();
    }
}
