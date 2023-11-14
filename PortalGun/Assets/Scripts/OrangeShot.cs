using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeShot : Shot
{
    private static OrangeShot instance;
    public static OrangeShot Instance { get { return instance; } }
    private void Awake()
    {
        portalType = PortalType.orange;
        if (instance != null && instance != this)
        {
            _isActive = false;
        }
        else
        {
            instance = this;
            _isActive = true;
        }
    }

}
