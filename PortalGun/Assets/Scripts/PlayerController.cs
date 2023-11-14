using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerInputActions;

    private Vector2 movement;
    private Vector2 mouseLook;
    private float _playerSpeed = .5f;

    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }
    public Vector3 pos
    {
        get { return (Instance.transform.position); }
        set { Instance.transform.position = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        //lookMove = 0f;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        lookAround();
        move();
    }

    private void move()
    {
        Vector3 moveVec = playerInputActions.Movement.WASD.ReadValue<Vector3 >();
        Vector3 temp = pos;

        temp.x += moveVec.x * _playerSpeed;
        temp.z += moveVec.z * _playerSpeed;

        pos = temp;
    }

        void lookAround()
    {
        float HorizontalSensitivity = 30.0f;
        float VerticalSensitivity = 30.0f;

        mouseLook = this.transform.rotation.eulerAngles;

        movement = playerInputActions.Movement.Rotation.ReadValue<Vector2>();

        float RotationX = HorizontalSensitivity * movement.x * Time.deltaTime;
        float RotationY = VerticalSensitivity * movement.y * Time.deltaTime;

        //Vector3 CameraRotation = this.transform.rotation.eulerAngles;

        mouseLook.x -= RotationY;
        mouseLook.y += RotationX;
        //lookMove = Mathf.Clamp(lookMove, -180f, 180);
        //lookMove += RotationY;

        //transform.localRotation = Quaternion.Euler(lookMove, 0, 0);
        //transform.Rotate(Vector2.left * RotationY);
        //transform.Rotate(Vector3.up * RotationX);
        this.transform.rotation = Quaternion.Euler(mouseLook);
    }

    public void OnRotationX(InputAction.CallbackContext Context)
    {
        //MouseX = Context.ReadValue<float>();
    }

    public void OnRotationY(InputAction.CallbackContext Context)
    {
        //MouseY = Context.ReadValue<float>();
    }
}
