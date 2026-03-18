using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class FlightThreadAsync : MonoBehaviour
{

    public float speed = 50f;
    public float rotationSpeed;
    public Transform cameraTransform;
    public Vector2 movementInput;

    // Iteration controller
    public int turbulenceIterations = 100000;

    // Calculated vectors of position list
    private List<Vector3> turbulenceForces = new List<Vector3>();

    // Variables to work on the secondary thread

    private Thread turbulenceThread;
    private bool isTurbulenceRunning = false; // Flag to kwow if the calculates continue
    private bool stopTurbulenceThread = false; // Flag to know if the thread has finished
    private float capturedTime = 0; // Variable to get the transcurred time

    // Controller flags above reading
    public bool read = false;
    public string filepath;


    // Method to move the spaceship
    public void OnMovement(InputValue value) {

        movementInput = value.Get<Vector2>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        filepath = Application.dataPath + "/TurbulenceData.txt";
        Debug.Log("rUTA DEL ARCHIVO: " + filepath);

    }

    // Update is called once per frame
    void Update() {

        if (cameraTransform == null) {

            Debug.LogError("No hay camara asignada...");
            return;

        }

        // ACTIVITY 1: Proccess in the secondary Thread
        capturedTime = Time.time;

        // Heavy procces in secondary thread

        if (!isTurbulenceRunning) {

            isTurbulenceRunning = true;
            stopTurbulenceThread = false;
            turbulenceThread = new Thread(() => SimulateTurbulence(capturedTime));
            turbulenceThread.Start();

        }


        // Move the spaceship linearly

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        // Move the spaceship rotationarly

        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);

    }

    public void SimulateTurbulence(float time) {

        turbulenceForces.Clear();

        // Repeatitions

        for (int i = 0; i < turbulenceIterations; i++) {

            // Verify if the thread must be stoped
            if (stopTurbulenceThread) break;

            Vector3 force = new Vector3(
                    Mathf.PerlinNoise(i * 0.001f, time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.002f, time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.003f, time) * 2 - 1
                );

            turbulenceForces.Add(force);

        }


        //Signal in console 
        Debug.Log("Iniciando simulación de turbulencia...");

        // Simulation completed
        isTurbulenceRunning = false;

    }

    private void OnDestroy() {

        // Signalize the close of the secondary thread
        stopTurbulenceThread = true;

        // Verify if the thread exist and is on executing
        if (turbulenceThread != null && turbulenceThread.IsAlive) {

            turbulenceThread.Join();

        }
    }

}
