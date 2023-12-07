using UnityEngine;

//Wood, Jordan
//jbwood
//12/5/2023
//Camera position for the orange portal camera

public class FollowCamOrange : MonoBehaviour
{
    private GameObject inPortal, outPortal;

    private Vector3 _playerPos;

    public static FollowCamOrange Instance { get { return instance; } }
    private static FollowCamOrange instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
            instance = this;
        }
        else
        {
            instance = this;
        }

    }

    // update changes camera position each fram based on player position
    void Update()
    {
        //if one portal isn't active do nothing
        if (!PortalGun.Instance._bluePortal || !PortalGun.Instance._orangePortal)
            return;
        if (!FollowCamBlue.Instance)
        {
            return;
        }

        inPortal = PortalGun.Instance._orangePortal;
        outPortal = PortalGun.Instance._bluePortal;
        _playerPos = PlayerController.Instance.transform.position;
        Vector3 inPortalPos = inPortal.gameObject.transform.position;

        //destination - source
        Vector3 direction = _playerPos - inPortalPos;
        //Debug.DrawRay(inPortalPos, direction, Color.red);
        //Debug.DrawRay(outPortalPos, direction, Color.green);
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        Quaternion quarterTurn = Quaternion.Euler(0f, -90f, 0f);
        Quaternion OportalR = PortalGun.Instance._orangePortal.gameObject.transform.rotation;
        Quaternion BportalR = PortalGun.Instance._bluePortal.gameObject.transform.rotation;

        Quaternion tempR = Quaternion.Inverse(OportalR) * Quaternion.Euler(0f, 180f - angle, 0f);
        tempR *= quarterTurn;
        
        float newRot = (tempR * BportalR).eulerAngles.y;
        FollowCamBlue.Instance.transform.rotation = Quaternion.Euler(0f, newRot, 0f);
    }
}
