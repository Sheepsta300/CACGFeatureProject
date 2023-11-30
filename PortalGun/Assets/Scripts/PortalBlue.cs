using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBlue : Portal
{

    private void Awake()
    {
        isActive = false;
        portalType = PortalType.blue;
    }
}
