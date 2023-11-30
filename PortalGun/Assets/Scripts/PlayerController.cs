using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    public Rigidbody rb;
    public GameObject ground;

    private Vector2 movement;
    private float _playerSpeed = .25f, _jumpSpeed = 30f, _halfPlayerWidth, _bounds;

    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }
    public Vector3 pos
    {
        get { return (Instance.transform.position); }
        set { Instance.transform.position = value; }
    }

    public Quaternion rot
    {
        get { return (Instance.transform.rotation); }
        set { Instance.transform.rotation = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _halfPlayerWidth = this.gameObject.transform.localScale.x / 2;
        Debug.Log(_halfPlayerWidth);
        _bounds = (ground.transform.localScale.x / 2) - 2.6f;
        Debug.Log(_bounds);
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        //lookMove = 0f;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
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
        Debug.Log(moveVec);
        temp.z += moveVec.z * _playerSpeed;
        temp.x += moveVec.x * _playerSpeed;
        if (temp.x > _bounds - _halfPlayerWidth)
        {
            temp.x = _bounds - _halfPlayerWidth;
            pos = temp;
            moveVec = Vector3.zero;
        }
        if (temp.x < (_bounds * -1) + _halfPlayerWidth)
        {
            temp.x = (_bounds * -1) + _halfPlayerWidth;
            pos = temp;
            moveVec = Vector3.zero;
        }
        if (temp.z > _bounds - _halfPlayerWidth)
        {
            temp.z = _bounds - _halfPlayerWidth;
            pos = temp;
            moveVec = Vector3.zero;
        }
        if (temp.z < (_bounds * -1) + _halfPlayerWidth)
        {
            temp.z = (_bounds * -1) + _halfPlayerWidth;
            pos = temp;
            moveVec = Vector3.zero;
        }
        
        this.transform.Translate(new Vector3(moveVec.x * _playerSpeed, 0, moveVec.z * _playerSpeed));
    }

        void lookAround()
    {
        float HorizontalSensitivity = 2.0f;
        //float VerticalSensitivity = 2.0f;

        //mouseLook = this.transform.rotation.eulerAngles;

        movement = playerInputActions.Movement.Rotation.ReadValue<Vector2>();

        

        float RotationX = HorizontalSensitivity * movement.x * Time.deltaTime;
        //float RotationY = VerticalSensitivity * movement.y * Time.deltaTime;

        //Vector3 CameraRotation = this.transform.rotation.eulerAngles;

        //mouseLook.x -= RotationY;
        //mouseLook.y += RotationX;
        //lookMove = Mathf.Clamp(lookMove, -180f, 180);
        //lookMove += RotationY;

        //transform.localRotation = Quaternion.Euler(lookMove, 0, 0);
        //transform.Rotate(Vector2.left * RotationY);
        //transform.Rotate(Vector3.up * RotationX);
        this.transform.rotation *= Quaternion.Slerp(Quaternion.identity, Quaternion.LookRotation(RotationX < 0 ? Vector3.left : Vector3.right), Mathf.Abs(RotationX) * HorizontalSensitivity * Time.deltaTime);
        //this.transform.rotation *= Quaternion.Slerp(Quaternion.identity, Quaternion.LookRotation(RotationY < 0 ? Vector3.down : Vector3.up), Mathf.Abs(RotationY) * VerticalSensitivity * Time.deltaTime);
        //this.transform.rotation = Quaternion.Euler(mouseLook);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(PortalGun.Instance._bluePortal && PortalGun.Instance._orangePortal)
        {
            if (collision.gameObject.tag == "Portal")
            {
                if (collision.gameObject.GetComponent<Portal>().getType() == PortalType.orange)
                {
                    Vector3 tempP = PortalGun.Instance._bluePortal.gameObject.transform.localPosition;
                    Vector3 tempR = rot.eulerAngles;
                    tempP.z -= 3f;
                    tempR.y += 180f;
                    pos = tempP;
                    rot = Quaternion.Euler(tempR);
                }
                else if (collision.gameObject.GetComponent<Portal>().getType() == PortalType.blue)
                {
                    Vector3 tempP = PortalGun.Instance._orangePortal.gameObject.transform.localPosition;
                    Vector3 tempR = rot.eulerAngles;
                    tempR.y += 180f;
                    tempP.z -= 3f;
                    pos = tempP;
                    rot = Quaternion.Euler(tempR);

                }
            }
        }
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            //Debug.Log("yeet");
            rb.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
        }
    }

    public bool GroundCheck()
    {
        Vector3 center = gameObject.GetComponent<CapsuleCollider>().bounds.min;
        center.x += transform.localScale.x / 2;

        return Physics.Raycast(center, Vector3.down, .2f);
    }
}
