using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShot : Shot
{

    private static BlueShot instance;
    public static BlueShot Instance { get { return instance; } }
    private void Awake()
    {
        portalType = PortalType.blue;
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
