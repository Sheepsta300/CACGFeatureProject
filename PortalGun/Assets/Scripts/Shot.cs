using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    protected bool _isActive;
    protected PortalType portalType;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MoonRock")
        {
            _isActive = false;
        }
    }

    public bool getState()
    {
        return _isActive;
    }

    public PortalType getType()
    {
        return portalType;
    }
}
