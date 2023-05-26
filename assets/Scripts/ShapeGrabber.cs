using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGrabber : MonoBehaviour
{
    Collider m_Collider;
    Vector3 m_Center;
    Vector3 m_Size, m_Min, m_Max;
    private string parentName;
    void Start()
    {
        parentName = transform.parent.name;
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<Collider>();
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;
        //Output this data into the console
        OutputData();
    }

    void OutputData()
    {
        //Output to the console the center and size of the Collider volume
        //Debug.Log("Collider Center : " + m_Center);
        //Debug.Log("Collider Size : " + m_Size);
        //Debug.Log("Collider bound Minimum : " + m_Min);
        //Debug.Log("Collider bound Maximum : " + m_Max);

        Vector2 topLeft = new Vector2(m_Max.x,m_Max.z);
        Vector2 topRight = new Vector2(m_Max.x,m_Min.z);
        Vector2 bottomLeft = new Vector2(m_Min.x,m_Min.z);
        Vector2 bottomRight = new Vector2(m_Min.x,m_Max.z);
        //Debug.Log("'"+parentName+"':["+topLeft+","+bottomRight+","+bottomLeft+","+topRight+"]");
        //Debug.Log(topLeft);
        //Debug.Log(bottomLeft);
        //Debug.Log(bottomRight);
        //Debug.Log(topRight);
        
    }
}
