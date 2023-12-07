using UnityEngine;

//Wood, Jordan
//jbwood
//12/5/2023
//A parent class for both portal types, managing functions and member vars

public class Portal : MonoBehaviour
{
    protected PortalType portalType;
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }
    //returns the type of portal enum
    public PortalType getType()
    {
        return portalType;
    }

}
