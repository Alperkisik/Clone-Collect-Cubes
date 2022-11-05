using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //If this variable set "false", script will use mouse inputs for testing movement on pc 
    //
    [SerializeField] private bool Touch_Movement = false;
    [SerializeField] private float touchSpeedModifier = 50f;
    [SerializeField] private float keyboardSpeedModifier = 100f;
    //this variable only works at keyboard inputs for testing purposes on pc
    [SerializeField] private float keyboardRotationSpeed = 300f;
    private Touch touch;
    Rigidbody rb;
    Vector3 direction;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();

        Listener();
    }

    private void FixedUpdate()
    {
        if (!Game_Manager.instance.IslevelStarted || Game_Manager.instance.IslevelFinished) return;

        if (Touch_Movement) TouchMovement();
        else KeyboarMovement();
    }

    void Listener()
    {
        Collectable_Manager.instance.OnAllCubesCollected += Event_OnAllCubesCollected;
    }

    private void Event_OnAllCubesCollected(object sender, System.EventArgs e)
    {
        rb.velocity = Vector3.zero;
    }

    void TouchMovement()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchDir = new Vector3(transform.position.x + touch.deltaPosition.x, transform.position.y, transform.position.z + touch.deltaPosition.y);
                transform.LookAt(touchDir);

                direction = touchDir.normalized;

                /*transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * touchSpeedModifier,
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.y * touchSpeedModifier
                    );*/

                //rb.MovePosition(direction * touchSpeedModifier * Time.deltaTime);
                rb.velocity = direction * touchSpeedModifier * Time.deltaTime;
            }
        }
        else rb.velocity = Vector3.zero;
    }

    void KeyboarMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += transform.forward * Time.deltaTime * keyboardSpeedModifier;

            //rb.MovePosition(transform.forward * Time.deltaTime * keyboardSpeedModifier);
            rb.velocity = transform.forward * Time.deltaTime * keyboardSpeedModifier;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -1f * Time.deltaTime * keyboardRotationSpeed, 0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.position += -transform.forward * Time.deltaTime * keyboardSpeedModifier;
            //rb.MovePosition(-transform.forward * Time.deltaTime * keyboardSpeedModifier);
            rb.velocity = -transform.forward * Time.deltaTime * keyboardSpeedModifier;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, 1f * Time.deltaTime * keyboardRotationSpeed, 0f);
        }
        else rb.velocity = Vector3.zero;
    }
}
