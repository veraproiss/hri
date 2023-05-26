using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Valve.Newtonsoft.Json.Utilities;

public class MoveToTarget: MonoBehaviour
{
    [SerializeField] public Transform[] track;

    [SerializeField] private int maximumDistanceFromPlayer = 5;
    [SerializeField] private float defaultSpeed=2;
    public float speed;

    [SerializeField] private float rotationSpeedModifier;
    public float precision;
    [SerializeField] private int _current;
    [SerializeField] private int _next=0;
    [SerializeField] private int _goal;
    [SerializeField] private GameObject _player;
    
    //animators
    [SerializeField] private RotateRight ScriptRotateRight;
    [SerializeField] private RotateLeft ScriptRotateLeft;
    
    
    public float waitTime = 0f;
    public float travelingBackTime = 0f;
    
    public bool isPlayerOffTrack = false;
    public bool isPlayerTooFarBehind = false;
    public bool isRobotWaiting = false;//Todo

    public bool hasStarted = false;
    
    
    
    void Start()
    {
        speed = defaultSpeed;
    }

    void FixedUpdate()
    {
        if (hasStarted)
        {
            //finding out where everything is
            Transform closestPlayerTarget = GetClosestTargetToPlayer(track);
            int closestPlayerTargetIndex = GetIndex(track, closestPlayerTarget);


            //Setting Goal
            if (GetPlayerDistanceToTrack(track) > 15) //if player is off the track: 
            {
                //send debug message, if the user initialy leaves the track
                if (!isPlayerOffTrack)
                {
                //Debug.Log("The User left the track");
                }

                isPlayerOffTrack = true;

                if (closestPlayerTargetIndex == _current) // if closest target to player is this one: wait
                {
                    wait();
                }
                else // if closest target to player is another one: follow the track to that one
                {
                    //set new goal
                    _goal = closestPlayerTargetIndex;
                }
            }
            else //the player is on the track
            {
                isPlayerOffTrack = false;
                if (closestPlayerTargetIndex + maximumDistanceFromPlayer < _current
                ) //if player is too far behind the robot on the track
                {
                    //send out a debug message the first time the user falls back.
                    if (!isPlayerTooFarBehind)
                    {
                        Debug.Log("The Robot has to move back to the Player");
                    }

                    isPlayerTooFarBehind = true;

                    //choose a goal, that is not too far away from the players current location
                    _goal = closestPlayerTargetIndex + maximumDistanceFromPlayer - 1;
                }
                else if (closestPlayerTargetIndex + maximumDistanceFromPlayer == _current
                ) //if the robot is as far away from the user as possible
                {
                    isPlayerTooFarBehind = false;
                    _goal = _current;
                }
                else if (closestPlayerTargetIndex <= _current) //if player is behind the robot
                {
                    isPlayerTooFarBehind = false;
                    //choose a goal, that is just out of the players reach
                    _goal = closestPlayerTargetIndex + maximumDistanceFromPlayer;
                    //move with normal speed
                    speed = defaultSpeed;
                }

                else if (closestPlayerTargetIndex > _current) //if player is in front of the robot
                {
                    //choose a goal, that is just out of the players reach
                    _goal = closestPlayerTargetIndex + maximumDistanceFromPlayer;
                    //move with high speed 
                    speed = defaultSpeed * 3;
                }
            }




            //choose the next target
            //dependent if the index of the goal is bigger than the index of the current checkpoint
            //the robot chooses a target up or down the track.
            if (_goal == _current)
            {
                _next = _current;
            }
            else if (_goal > _current)
            {
                _next = (_current + 1) % track.Length;
            }
            else if (_goal < _current)
            {
                if (_current != 0)
                {
                    _next = (_current - 1) % track.Length;
                }

            }

            //let the robot move to _next, unless we have arrived at the end of track or our goal 
            if (_current == track.Length - 1)
            {
                //rotate to player
                Vector3 relativePos = _player.transform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
            }
            else if (_next == _current) //if goal is reached:
            {
                wait();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, track[_next].position);
                if (distance <= precision)
                {
                    _current = _next;
                }

                isRobotWaiting = false;
                startRotationAnimation();
                Rotate(speed, _next);
                Move(speed);

                if (isPlayerTooFarBehind)
                {
                    travelingBackTime += Time.fixedDeltaTime;
                }


            }


        }

    }

    void wait()
        {
            if (!isRobotWaiting)
            {
                Debug.Log("The Robot is Waiting for the player");
            }
            isRobotWaiting = true;
            stopRotationAnimation();
            

            
            //rotate to player
            Vector3 relativePos = _player.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp( transform.rotation, toRotation, speed * Time.deltaTime );
            
            //add to waitTime
            waitTime += Time.fixedDeltaTime;
        }

    

    void Move(float Speed)
    {
        transform.Translate(Vector3.forward * ((Speed) * Time.deltaTime));
    }

    void Rotate(float Speed, int index)
    {
        // Rotate the Robot so it keeps looking at the next target
        Vector3 relativePos = track[index].position - transform.position;
        float distance = Vector3.Distance(track[index].position, transform.position);
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        //rotationSpeedModifier = 0;//inverter(distance, 15);
        transform.rotation = Quaternion.Lerp( transform.rotation, toRotation, (Speed+rotationSpeedModifier) * Time.deltaTime );
    }



    Transform GetClosestTargetToPlayer (Transform[] targets)
    {
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = _player.transform.position;
        foreach(Transform potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = potentialTarget;
            }
        }
     
        return closestTarget;
    }

   public float GetPlayerDistanceToTrack(Transform[] targets)
    {
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = _player.transform.position;
        foreach(Transform potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
            }
        }

        return closestDistanceSqr;
    }
    
    int GetIndex(Transform[] track, Transform target)
    {
        for (int i = 0; i < track.Length-1; i++)
        {
            Transform a = track[i];
            if (a==target)
            {
                return i;
            }
        }

        return -1;
    }

    public void startRunning()
    {
        hasStarted = true;
    }
    public void stopRunning()
    {
        hasStarted = false;
        stopRotationAnimation();
    }

    public void stopRotationAnimation()
    {
        ScriptRotateRight.direction = 0;
        ScriptRotateLeft.direction = 0;
    }

    public void startRotationAnimation()
    {
        ScriptRotateRight.direction = 1;
        ScriptRotateLeft.direction = 1;
    }

}
