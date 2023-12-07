using UnityEngine;

//Wood, Jordan
//jbwood
//12/5/2023
//A Parent class for shots, that manages portal positioning

public class Shot : MonoBehaviour
{
    protected bool _isActive;
    protected PortalType portalType;
    public GameObject orangePortal, bluePortal;

    //manages shot collision and calls a corresponding spawnPortal function based
    //on collision tag type
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            _isActive = false;
        }
        else if (collision.gameObject.tag == "MoonRockForward")
        {
            _isActive = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spawnPortalForward(this.portalType, collision);
        }
        else if (collision.gameObject.tag == "MoonRockBackward")
        {
            _isActive = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spawnPortalBackward(this.portalType, collision);
        }
        else if (collision.gameObject.tag == "MoonRockLeft")
        {
            _isActive = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spawnPortalLeft(this.portalType, collision);
        }
        else if (collision.gameObject.tag == "MoonRockRight")
        {
            _isActive = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spawnPortalRight(this.portalType, collision);
        }
        else if(collision.gameObject.tag == "MoonRockUp")
        {
            _isActive = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spawnPortalUp(this.portalType, collision);
        }
        else if(collision.gameObject.tag == "MoonRockDown")
        {
            _isActive = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            spawnPortalDown(this.portalType, collision);
        }
        else if (collision.gameObject.tag == "Portal")
        {
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            switch (collision.gameObject.GetComponent<Portal>().getType())
            {
                case PortalType.blue:
                    PortalGun.Instance._bluePortal = null;

                    break;
                case PortalType.orange:
                    PortalGun.Instance._orangePortal = null;

                    break;
                default:
                    break;
            }


            Destroy(collision.gameObject);
            _isActive = false;

            if (PortalGun.Instance._bluePortal == null || PortalGun.Instance._orangePortal == null)
            {
                if (PortalGun.Instance._bluePortal)
                    foreach (Transform t in PortalGun.Instance._bluePortal.transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                if (PortalGun.Instance._orangePortal)
                    foreach (Transform t in PortalGun.Instance._orangePortal.transform)
                    {
                        t.gameObject.SetActive(false);
                    }
            }
        }
        if (PortalGun.Instance._orangePortal && PortalGun.Instance._bluePortal)
        {
            foreach (Transform t in PortalGun.Instance._bluePortal.transform)
            {
                t.gameObject.SetActive(true);
            }
            foreach (Transform t in PortalGun.Instance._orangePortal.transform)
            {
                t.gameObject.SetActive(true);
            }
        }
    }
    //returns if the shot has hit something or not
    public bool getState()
    {
        return _isActive;
    }

    public PortalType getType()
    {
        return portalType;
    }
    //spawns a portal on a forward facing wall
    private void spawnPortalForward(PortalType portalType, Collision collision)
    {
        float zPos = collision.transform.gameObject.GetComponent<BoxCollider>().bounds.min.z;
        float yPos = this.transform.position.y;
        float xPos = this.transform.position.x;

        Vector3 minLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.min;
        //Debug.Log(minLimit);
        Vector3 maxLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.max;
        //Debug.Log(maxLimit);

        if (yPos < minLimit.y + bluePortal.transform.localScale.y / 2) yPos = minLimit.y + bluePortal.transform.localScale.y / 2;
        if (yPos > maxLimit.y - bluePortal.transform.localScale.y / 2) yPos = maxLimit.y - bluePortal.transform.localScale.y / 2;
        if (xPos < minLimit.x + bluePortal.transform.localScale.x / 2) xPos = minLimit.x + bluePortal.transform.localScale.x / 2;
        if (xPos > maxLimit.x - bluePortal.transform.localScale.x / 2) xPos = maxLimit.x - bluePortal.transform.localScale.x / 2;

        switch (portalType)
        {
            case PortalType.blue:
                if (PortalGun.Instance._bluePortal != null)
                    Destroy(PortalGun.Instance._bluePortal);

                PortalGun.Instance._bluePortal = Instantiate(bluePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                //PortalGun.Instance._bluePortal.GetComponent<PortalBlue>().setActive();
                break;
            case PortalType.orange:
                if (PortalGun.Instance._orangePortal != null)
                    Destroy(PortalGun.Instance._orangePortal);
                PortalGun.Instance._orangePortal = Instantiate(orangePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                //PortalGun.Instance._orangePortal.GetComponent<PortalOrange>().setActive();
                break;
            default:
                break;
        }
    }
    //spawns a wall on an 180 deg rotated wall
    private void spawnPortalBackward(PortalType portalType, Collision collision)
    {
        float zPos = collision.transform.gameObject.GetComponent<BoxCollider>().bounds.max.z;
        float yPos = this.transform.position.y;
        float xPos = this.transform.position.x;
        Vector3 rPos = collision.transform.rotation.eulerAngles;
        rPos.y += 180f;

        Vector3 minLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.min;
        //Debug.Log(minLimit);
        Vector3 maxLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.max;
        //Debug.Log(maxLimit);

        if (yPos < minLimit.y + bluePortal.transform.localScale.y / 2) yPos = minLimit.y + bluePortal.transform.localScale.y / 2;
        if (yPos > maxLimit.y - bluePortal.transform.localScale.y / 2) yPos = maxLimit.y - bluePortal.transform.localScale.y / 2;
        if (xPos < minLimit.x + bluePortal.transform.localScale.x / 2) xPos = minLimit.x + bluePortal.transform.localScale.x / 2;
        if (xPos > maxLimit.x - bluePortal.transform.localScale.x / 2) xPos = maxLimit.x - bluePortal.transform.localScale.x / 2;

        switch (portalType)
        {
            case PortalType.blue:
                if (PortalGun.Instance._bluePortal != null)
                    Destroy(PortalGun.Instance._bluePortal);

                PortalGun.Instance._bluePortal = Instantiate(bluePortal, new Vector3(xPos, yPos, zPos), Quaternion.Euler(rPos));
                //PortalGun.Instance._bluePortal.GetComponent<PortalBlue>().setActive();
                break;
            case PortalType.orange:
                if (PortalGun.Instance._orangePortal != null)
                    Destroy(PortalGun.Instance._orangePortal);
                PortalGun.Instance._orangePortal = Instantiate(orangePortal, new Vector3(xPos, yPos, zPos), Quaternion.Euler(rPos));
                //PortalGun.Instance._orangePortal.GetComponent<PortalOrange>().setActive();
                break;
            default:
                break;
        }
    }
    //spawns a wall on a 90 deg rotated wall
    private void spawnPortalLeft(PortalType portalType, Collision collision)
    {
        float zPos = this.transform.position.z;
        float yPos = this.transform.position.y;
        float xPos = collision.transform.gameObject.GetComponent<BoxCollider>().bounds.max.x;
        Vector3 rPos = collision.transform.rotation.eulerAngles;
        rPos.y += 180f;

        Vector3 minLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.min;
        //Debug.Log(minLimit);
        Vector3 maxLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.max;
        //Debug.Log(maxLimit);

        if (yPos < minLimit.y + bluePortal.transform.localScale.y / 2) yPos = minLimit.y + bluePortal.transform.localScale.y / 2;
        if (yPos > maxLimit.y - bluePortal.transform.localScale.y / 2) yPos = maxLimit.y - bluePortal.transform.localScale.y / 2;
        if (zPos < minLimit.z + bluePortal.transform.localScale.x / 2) zPos = minLimit.z + bluePortal.transform.localScale.x / 2;
        if (zPos > maxLimit.z - bluePortal.transform.localScale.x / 2) zPos = maxLimit.z - bluePortal.transform.localScale.x / 2;

        switch (portalType)
        {
            case PortalType.blue:
                if (PortalGun.Instance._bluePortal != null)
                    Destroy(PortalGun.Instance._bluePortal);

                PortalGun.Instance._bluePortal = Instantiate(bluePortal, new Vector3(xPos, yPos, zPos), Quaternion.Euler(rPos));
                //PortalGun.Instance._bluePortal.GetComponent<PortalBlue>().setActive();
                break;
            case PortalType.orange:
                if (PortalGun.Instance._orangePortal != null)
                    Destroy(PortalGun.Instance._orangePortal);
                PortalGun.Instance._orangePortal = Instantiate(orangePortal, new Vector3(xPos, yPos, zPos), Quaternion.Euler(rPos));
                //PortalGun.Instance._orangePortal.GetComponent<PortalOrange>().setActive();
                break;
            default:
                break;
        }
    }
    //spanws a wall on a 270 deg rotated wall
    private void spawnPortalRight(PortalType portalType, Collision collision)
    {
        float zPos = this.transform.position.z;
        float yPos = this.transform.position.y;
        float xPos = collision.transform.gameObject.GetComponent<BoxCollider>().bounds.min.x;

        Vector3 minLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.min;
        //Debug.Log(minLimit);
        Vector3 maxLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.max;
        //Debug.Log(maxLimit);

        if (yPos < minLimit.y + bluePortal.transform.localScale.y / 2) yPos = minLimit.y + bluePortal.transform.localScale.y / 2;
        if (yPos > maxLimit.y - bluePortal.transform.localScale.y / 2) yPos = maxLimit.y - bluePortal.transform.localScale.y / 2;
        if (zPos < minLimit.z + bluePortal.transform.localScale.x / 2) zPos = minLimit.z + bluePortal.transform.localScale.x / 2;
        if (zPos > maxLimit.z - bluePortal.transform.localScale.x / 2) zPos = maxLimit.z - bluePortal.transform.localScale.x / 2;

        switch (portalType)
        {
            case PortalType.blue:
                if (PortalGun.Instance._bluePortal != null)
                    Destroy(PortalGun.Instance._bluePortal);

                PortalGun.Instance._bluePortal = Instantiate(bluePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                //PortalGun.Instance._bluePortal.GetComponent<PortalBlue>().setActive();
                break;
            case PortalType.orange:
                if (PortalGun.Instance._orangePortal != null)
                    Destroy(PortalGun.Instance._orangePortal);
                PortalGun.Instance._orangePortal = Instantiate(orangePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                //PortalGun.Instance._orangePortal.GetComponent<PortalOrange>().setActive();
                break;
            default:
                break;
        }
    }
    //spawns a portal on the ceiling
    private void spawnPortalUp(PortalType portalType, Collision collision)
    {
        float zPos = this.transform.position.z;
        float yPos = collision.transform.gameObject.GetComponent<BoxCollider>().bounds.min.y;
        float xPos = this.transform.position.x;

        Vector3 minLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.min;
        //Debug.Log(minLimit);
        Vector3 maxLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.max;
        //Debug.Log(maxLimit);

        if (xPos < minLimit.x + bluePortal.transform.localScale.x / 2) xPos = minLimit.x + bluePortal.transform.localScale.x / 2;
        if (xPos > maxLimit.x - bluePortal.transform.localScale.x / 2) xPos = maxLimit.x - bluePortal.transform.localScale.x / 2;
        if (zPos < minLimit.z + bluePortal.transform.localScale.y / 2) zPos = minLimit.z + bluePortal.transform.localScale.y / 2;
        if (zPos > maxLimit.z - bluePortal.transform.localScale.y / 2) zPos = maxLimit.z - bluePortal.transform.localScale.y / 2;

        switch (portalType)
        {
            case PortalType.blue:
                if (PortalGun.Instance._bluePortal != null)
                    Destroy(PortalGun.Instance._bluePortal);
                PortalGun.Instance._bluePortal = Instantiate(bluePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                
                break;
            case PortalType.orange:
                if (PortalGun.Instance._orangePortal != null)
                    Destroy(PortalGun.Instance._orangePortal);
                PortalGun.Instance._orangePortal = Instantiate(orangePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                
                break;
            default:
                break;
        }
    }
    //spawns a portal on the floor
    private void spawnPortalDown(PortalType portalType, Collision collision)
    {
        float zPos = this.transform.position.z;
        float yPos = collision.transform.gameObject.GetComponent<BoxCollider>().bounds.max.y;
        float xPos = this.transform.position.x;

        Vector3 minLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.min;
        //Debug.Log(minLimit);
        Vector3 maxLimit = collision.gameObject.GetComponent<BoxCollider>().bounds.max;
        //Debug.Log(maxLimit);

        if (xPos < minLimit.x + bluePortal.transform.localScale.x / 2) xPos = minLimit.x + bluePortal.transform.localScale.x / 2;
        if (xPos > maxLimit.x - bluePortal.transform.localScale.x / 2) xPos = maxLimit.x - bluePortal.transform.localScale.x / 2;
        if (zPos < minLimit.z + bluePortal.transform.localScale.y / 2) zPos = minLimit.z + bluePortal.transform.localScale.y / 2;
        if (zPos > maxLimit.z - bluePortal.transform.localScale.y / 2) zPos = maxLimit.z - bluePortal.transform.localScale.y / 2;

        switch (portalType)
        {
            case PortalType.blue:
                if (PortalGun.Instance._bluePortal != null)
                    Destroy(PortalGun.Instance._bluePortal);

                PortalGun.Instance._bluePortal = Instantiate(bluePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                PortalGun.Instance._bluePortal.gameObject.tag = "PortalDown";

                break;
            case PortalType.orange:
                if (PortalGun.Instance._orangePortal != null)
                    Destroy(PortalGun.Instance._orangePortal);
                PortalGun.Instance._orangePortal = Instantiate(orangePortal, new Vector3(xPos, yPos, zPos), collision.transform.rotation);
                PortalGun.Instance._orangePortal.gameObject.tag = "PortalDown";

                break;
            default:
                break;
        }
    }
}
