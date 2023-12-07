using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Wood, Jordan
//jbwood
//12/5/2023
//PortalGun behaviour and shot speed class

public enum PortalType
{
    blue,
    orange
}

public class PortalGun : MonoBehaviour
{
    //public PortalBlue portalBlue;
    //public PortalOrange portalOrange;

    private List<GameObject> shots;
    private List<GameObject> deleteShots;

    public GameObject blueShot, orangeShot;

    public Material blueColour, orangeColour;
    public GameObject gunLight;
    public GameObject bluePortal, orangePortal;
    public GameObject _bluePortal = null, _orangePortal = null;

    [SerializeField] private float _shotSpeed = 2f; 

    private static PortalGun instance;
    public static PortalGun Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        shots = new List<GameObject>();
        deleteShots = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        moveShots();
    }
    //moves shots each frame forward and deletes any shots that have hit something
    private void moveShots()
    {
        foreach(GameObject shot in shots)
        {
            Vector3 temp = shot.transform.position;
            temp += shot.transform.forward * _shotSpeed;
            shot.transform.position = temp;
            if (!shot.GetComponent<Shot>().getState())
            {
                deleteShots.Add(shot);
            }
        }
        foreach(GameObject shot in deleteShots)
        {
            shots.Remove(shot);
            Destroy(shot);
        }
        deleteShots.Clear();

    }
    //fires an orange shot on a leftclick
    public void onLeftClick(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            shoot(PortalType.orange);
           gunLight.GetComponent<Renderer>().material = orangeColour;
        }
    }
    //fires a blue shot on a rightclick
    public void onRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shoot(PortalType.blue);
            gunLight.GetComponent<Renderer>().material = blueColour;
        }
    }
    //shoots a shot of the passed parameter portalType
    private void shoot(PortalType portalType)
    {
        switch (portalType)
        {
            case PortalType.blue:
                shots.Add(Instantiate(blueShot, transform.position, transform.rotation));
                break;
            case PortalType.orange:
                shots.Add(Instantiate(orangeShot, transform.position, transform.rotation));
                break;
            default:
                break;
        }

    }
}
