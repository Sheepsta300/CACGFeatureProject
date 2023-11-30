using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    protected bool isActive;
    protected PortalType portalType;
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    public PortalType getType()
    {
        return portalType;
    }
    public void setActive()
    {
        isActive = true;
    }
    public void setInactive()
    {
        isActive = false;
    }
    public bool getState()
    {
        return isActive;
    }
}
