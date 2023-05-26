using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField] private Transform groundCheck;
    public float groundDistance = 0.4f;

    public MoveToTarget robot;
    
    
    public float speed = 12f;
    public float gravity = -9.81f;
    public LayerMask groundMask;

    private Vector3 velocity;

    private bool isGrounded;
    
    private GameObject gameManager;

    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        # region Basic Movement
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        #endregion
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExperimentStart"))
        {
            var gameSettings = gameManager.GetComponent<GameManager>();
            gameSettings.tutorialHasEnded = true;
            robot.startRunning();
            var emergencyLights = Resources.FindObjectsOfTypeAll<EmergencyLight>();
            emergencyLights[0].gameObject.SetActive(true);
        } else if (other.CompareTag("ExperimentEnd"))
        {
            GameObject.FindObjectOfType<ExperimentManager>().WriteOutExpData();
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene(2);
        }
    }
}
