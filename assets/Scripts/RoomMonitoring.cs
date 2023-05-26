using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomMonitoring : MonoBehaviour
{
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    
    private string currentRoom;

    public string GetCurrentRoom()
    {
        return this.currentRoom;
    }


    // Update is called once per frame
    void Update()
    {
        try
        {
            var tmp = Physics.OverlapSphere(groundCheck.position, groundDistance, groundMask);
            //get name by getting the parent object name of the floor object
            currentRoom = tmp.First().transform.parent.name;
            //Debug.Log(String.Format("Current Room: {0}", currentRoom));

        }
        catch (IndexOutOfRangeException)
        {
            //Debug.Log(String.Format("Room could not be read, try again next iteration!"));
        }
        catch (InvalidOperationException)
        {
            //Debug.Log(String.Format("Room could not be read, try again next iteration!"));
        }
        catch (Exception)
        {
            //Debug.Log(String.Format("Room could not be read, try again next iteration!"));
        }
    }
}
