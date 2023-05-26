using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRPlayerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 1f;
    public CharacterController characterController;
    public GameObject gameManager;

    public MoveToTarget robot;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        //transform.position += speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0,9.81f,0) * Time.deltaTime);
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
