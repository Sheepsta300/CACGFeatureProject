using UnityEngine;
using UnityEngine.InputSystem;

//Wood, Jordan
//jbwood
//12/5/2023
//A player movement and positioning class

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    public Rigidbody rb;
    public GameObject ground;
    public GameObject Pgun;
    public Camera fpView;

    private Vector2 movement;
    private float _playerSpeed = .25f, _jumpSpeed = 30f, _halfPlayerWidth, _bounds;
    [SerializeField] private LayerMask _jumpableGround;

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
    // awake initialises all member variables
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
    
    //controls player movement based on the new unity input system
    //limits movement to the size of the ground terrain
    private void move()
    {
        Vector3 moveVec = playerInputActions.Movement.WASD.ReadValue<Vector3 >();
        Vector3 temp = pos;
        //Debug.Log(moveVec);
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
    //controls camera rotation based on mouse movement from the new unity input system
    void lookAround()
    {
        float HorizontalSensitivity = .1f;
        float VerticalSensitivity = 5f;
        movement = playerInputActions.Movement.Rotation.ReadValue<Vector2>();

        float RotationX = HorizontalSensitivity * movement.x * Time.deltaTime;
        float RotationY = VerticalSensitivity * movement.y * Time.deltaTime;

        this.transform.rotation *= Quaternion.Slerp(Quaternion.identity, Quaternion.LookRotation(RotationX < 0 ? Vector3.left : Vector3.right), Mathf.Abs(RotationX));
        Vector3 temp = fpView.transform.rotation.eulerAngles;
        temp.x -= RotationY;
        fpView.transform.transform.eulerAngles = temp;
        Pgun.transform.transform.eulerAngles = temp;
    }
    //controls player portal collisions and wall collisions
    //will rotate the player based on current player rotation, in portal rotation
    //and out portal rotation
    private void OnCollisionEnter(Collision collision)
    {
        if(PortalGun.Instance._bluePortal && PortalGun.Instance._orangePortal)
        {
            Vector3 moveVec = playerInputActions.Movement.WASD.ReadValue<Vector3>();
            Quaternion halfTurn = Quaternion.Euler(0f, 180f, 0f);
            if (collision.gameObject.tag == "Portal")
            {
                if (collision.gameObject.GetComponent<Portal>().getType() == PortalType.orange)
                {
                    this.GetComponent<CapsuleCollider>().enabled = false;
                    Vector3 tempP = PortalGun.Instance._bluePortal.gameObject.transform.localPosition;
                    Quaternion OportalR = PortalGun.Instance._orangePortal.gameObject.transform.rotation;
                    Quaternion BportalR = PortalGun.Instance._bluePortal.gameObject.transform.rotation;

                    Quaternion tempR = Quaternion.Inverse(OportalR) * this.transform.rotation;
                    tempR *= halfTurn;

                    rot = tempR * BportalR;
                    pos = tempP;
                    this.transform.Translate(moveVec.x * 3f, 0, moveVec.z * 3f);
                    this.GetComponent<CapsuleCollider>().enabled = true;
                }
                else if (collision.gameObject.GetComponent<Portal>().getType() == PortalType.blue)
                {
                    this.GetComponent<CapsuleCollider>().enabled = false; 
                    Vector3 tempP = PortalGun.Instance._orangePortal.gameObject.transform.localPosition;
                    Quaternion OportalR = PortalGun.Instance._orangePortal.gameObject.transform.rotation;
                    Quaternion BportalR = PortalGun.Instance._bluePortal.gameObject.transform.rotation;

                    Quaternion tempR = Quaternion.Inverse(BportalR) * this.transform.rotation;
                    tempR *= halfTurn;

                    rot = tempR * OportalR;
                    pos = tempP;
                    this.transform.Translate(moveVec.x * 3f, 0, moveVec.z * 3f);
                    this.GetComponent<CapsuleCollider>().enabled = true;

                }
                
            } else if (collision.gameObject.tag == "PortalDown")
            {
                if (collision.gameObject.GetComponent<Portal>().getType() == PortalType.orange)
                {
                    this.GetComponent<CapsuleCollider>().enabled = false;
                    Vector3 tempP = PortalGun.Instance._bluePortal.gameObject.transform.position;
                    tempP.y -= 6f;
                    pos = tempP;
                    //this.transform.Translate(0f, moveVec.y * -10f, 0f);
                    this.GetComponent<CapsuleCollider>().enabled = true;
                }
                else if (collision.gameObject.GetComponent<Portal>().getType() == PortalType.blue)
                {
                    this.GetComponent<CapsuleCollider>().enabled = false;
                    Vector3 tempP = PortalGun.Instance._orangePortal.gameObject.transform.position;
                    tempP.y -= 6f;
                    pos = tempP;
                    //this.transform.Translate(0f, moveVec.y * -3f, 0f);
                    this.GetComponent<CapsuleCollider>().enabled = true;
                
                }
            }
        }
    }
    //causes player to jump based on space bar input
    public void Jump(InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            rb.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
        }
    }
    /*
    public bool GroundCheck()
    {
        Vector3 center = gameObject.GetComponent<CapsuleCollider>().bounds.min;
        center.y -= .1f;
        center.x = pos.x;
        center.z = pos.z;
        Debug.DrawRay(center, center + Vector3.down, Color.red);
        return Physics.Raycast(center, Vector3.down, .2f, _jumpableGround);
    }
    */
}
