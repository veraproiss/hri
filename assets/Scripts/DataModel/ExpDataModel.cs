using System;
using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ExpDataModel 
{
    /* Model for saving experiment data set 
     * every 3 seconds
     */
  
    #region data fields
    
    //No encapsulation for performance sake
    public DateTime timestamp;
    public string room;
    public Coordinate playerPosition;
    public Coordinate robotPosition;
    public float p2DistanceToRobot;
    public string expStage;
    public string condition;
    public float robotWaitTime;
    public float idealTrackDistance;
    public float particleDensity;
    
    #endregion

    public ExpDataModel(
        DateTime timestamp, 
        string room, 
        Coordinate playerPosition, 
        Coordinate robotPosition,
        float p2DistanceToRobot,
        string expStage,
        string condition,
        float robotWaitTime, 
        float idealTrackDistance,
        float particleDensity)
    {
        this.timestamp = timestamp;
        this.room = room;
        this.playerPosition = playerPosition;
        this.robotPosition = robotPosition;
        this.p2DistanceToRobot = p2DistanceToRobot;
        this.expStage = expStage;
        this.condition = condition;
        this.robotWaitTime = robotWaitTime;
        this.idealTrackDistance = idealTrackDistance;
        this.particleDensity = particleDensity;
    }
    
}
