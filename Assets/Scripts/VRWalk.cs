using UnityEngine;
using UnityEngine.TextCore.Text;

public class VRWalk : MonoBehaviour
{
    //Atributos/Variables de Clase
    public Transform vrCameraTransform;
    public float move_angle = 30.0f;
    public float max_move_angle = 60.0f;
    public float speed = 3.0f;
    public bool move;

    private CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       controller = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (vrCameraTransform.eulerAngles.x >= move_angle && vrCameraTransform.eulerAngles.x < max_move_angle) {
            move = true;

        } else {
            move = false;
        }

        if (move) {
            Vector3 direction = vrCameraTransform.TransformDirection(Vector3.forward);
            controller.SimpleMove(direction*speed);
        }
    }
}
