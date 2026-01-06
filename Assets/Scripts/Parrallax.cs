using UnityEngine;

public class Parralax : MonoBehaviour
{
    [Header("References")]
    public GameObject Camera;

    [Header("Variables")]
    [SerializeField] private float parralaxEffectX;
    [SerializeField] private float parralaxEffectY;
    [SerializeField] private float idleMovement;

    private float lenght;
    private float hight;
    private float idleOffset;
    private float startposx;
    private float startposy;

    void Start()
    {
        startposx = transform.position.x;
        startposy = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        hight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        if (idleMovement == 0)
        {
            UpdateX();
        }
        else
        {
            Idle();
        }

        UpdateY();
        
    }

    private void UpdateX()
    {
        float temp1 = (Camera.transform.position.x * (1 - parralaxEffectX));
        float dist1 = (Camera.transform.position.x * parralaxEffectX);

        transform.position = new Vector3(startposx + dist1, transform.position.y, transform.position.z);

        if (temp1 > startposx + lenght) startposx += lenght;
        else if (temp1 < startposx - lenght) startposx -= lenght;
    }

    private void UpdateY()
    {
        float temp2 = (Camera.transform.position.y * (1 - parralaxEffectY));
        float dist2 = (Camera.transform.position.y * parralaxEffectY);

        transform.position = new Vector3(transform.position.x, startposy + dist2, transform.position.z);
    }

    private void Idle()
    {
        float temp1 = Camera.transform.position.x;
        float dist1 = (temp1 * parralaxEffectX);

        idleOffset += idleMovement * Time.deltaTime;

        if (idleOffset > lenght)
            idleOffset -= lenght;
        else if (idleOffset < -lenght)
            idleOffset += lenght;

        transform.position = new Vector3(startposx + dist1 + idleOffset, transform.position.y, transform.position.z);
    }
}
