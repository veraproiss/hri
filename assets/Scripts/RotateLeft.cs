using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeft : MonoBehaviour
{
    [SerializeField] public float direction = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 100f * direction) * Time.deltaTime);
    }
}
