using UnityEngine;
using System.Collections;

public class ArrowMove : MonoBehaviour
{
    public float maxUpAndDown = 5;// amount of meters going up and down
    private float speed = 200;    // up and down speed
    private float angle = 0;            // angle to determin the height by using the sinus
    private float toDegrees = Mathf.PI / 180;    // radians to degrees
    private float startHeight;
    // Use this for initialization
    void Awake()
    {
        startHeight = this.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        if (angle > 360) angle -= 360;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x,startHeight + maxUpAndDown * (1 + Mathf.Sin(angle * toDegrees)));
    }
}
