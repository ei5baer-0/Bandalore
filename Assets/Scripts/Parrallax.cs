using UnityEngine;

public class Parralax : MonoBehaviour
{
    private float lenght, hight, startposx, startposy;
    public GameObject Camera;
    public float parralaxEffectX, parralaxEffectY;

    void Start()
    {
        startposx = transform.position.x;
        startposy = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        hight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        UpdateX();
        UpdateY();
    }

    void UpdateX()
    {
        float temp1 = (Camera.transform.position.x * (1 - parralaxEffectX));
        float dist1 = (Camera.transform.position.x * parralaxEffectX);

        transform.position = new Vector3(startposx + dist1, transform.position.y, transform.position.z);

        if (temp1 > startposx + lenght) startposx += lenght;
        else if (temp1 < startposx - lenght) startposx -= lenght;
    }

    void UpdateY()
    {
        float temp2 = (Camera.transform.position.y * (1 - parralaxEffectY));
        float dist2 = (Camera.transform.position.y * parralaxEffectY);

        transform.position = new Vector3(transform.position.x, startposy + dist2, transform.position.z);

        //if (temp2 > startposy + hight) startposy += hight;
        //else if (temp2 < startposy - hight) startposy -= hight;
    }
}
