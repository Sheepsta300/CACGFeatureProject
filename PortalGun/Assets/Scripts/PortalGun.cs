using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public GameObject bluePortal, orangePortal;
    private GameObject _bluePortal = null, _orangePortal = null;

    [SerializeField] private float _shotSpeed = .5f;

    private void Awake()
    {
        shots = new List<GameObject>();
        deleteShots = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveShots();
    }

    private void moveShots()
    {
        foreach(GameObject shot in shots)
        {
            Vector3 temp = shot.transform.position;
            temp.z += _shotSpeed;
            shot.transform.position = temp;
            if (!shot.GetComponent<Shot>().getState())
            {
                deleteShots.Add(shot);
                spawnPortal(shot.GetComponent<Shot>().getType(), shot);
            }
        }
        foreach(GameObject shot in deleteShots)
        {
            shots.Remove(shot);
            Destroy(shot);
        }
        deleteShots.Clear();

    }

    private void spawnPortal(PortalType portalType, GameObject shot)
    {
        switch (portalType)
        {
            case PortalType.blue:
                if (_bluePortal != null)
                    Destroy(_bluePortal);

                _bluePortal = Instantiate(bluePortal, shot.transform.position, Quaternion.identity);
                break;
            case PortalType.orange:
                if (_orangePortal != null)
                    Destroy(_orangePortal);
                _orangePortal = Instantiate(orangePortal, shot.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }
    public void onLeftClick(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            shoot(PortalType.orange);
        }
    }

    private void shoot(PortalType portalType)
    {
        switch (portalType)
        {
            case PortalType.blue:
                shots.Add(Instantiate(blueShot, transform.position, new Quaternion(90,0,0,0)));
                break;
            case PortalType.orange:
                shots.Add(Instantiate(orangeShot, transform.position, Quaternion.identity));
                break;
            default:
                break;
        }

    }

    private void onRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }
}
