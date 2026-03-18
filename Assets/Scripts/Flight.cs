using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Flight : MonoBehaviour
{

    public float speed = 50f;
    public float rotationSpeed;
    public Transform cameraTransform;
    public Vector2 movementInput;

    // Iteration controller
    public int turbulenceIterations = 100000;

    // Calculated vectors of position list
    private List<Vector3> turbulenceForces = new List<Vector3>();

    // Method to move the spaceship
    public void OnMovement(InputValue value){

        movementInput = value.Get<Vector2>();

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

        if (cameraTransform == null) {

            Debug.LogError("No hay camara asiganda...");
            return;

        }

        // ACTIVITY 1: Heavy process that waste resoruces
        SimulateTurbulence();

        // Move the spaceship linearly

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        // Move the spaceship rotationarly

        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);
        
    }

    public void SimulateTurbulence(){

        turbulenceForces.Clear();

        // Repeatitions

        for (int i = 0; i < turbulenceIterations; i++) {

            Vector3 force = new Vector3(
                    Mathf.PerlinNoise(i * 0.001f, Time.time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.002f, Time.time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.003f, Time.time) * 2 - 1
                );

            turbulenceForces.Add(force);

        }

    }

}
