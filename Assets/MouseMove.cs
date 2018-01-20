using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour {
    public float ScrollSpeed = 15;
    public float ScrollEdge = 0.01f;
 
    //private int HorizontalScroll = 1;
    //private int VerticalScroll   = 1;
    //private int DiagonalScroll   = 1;
 
    public float PanSpeed = 10f;
    
    private float TargetZoomZ  = 0f;
    public float ZoomSpeed    = 1f;
    public float MaxZoomIn    = 150f;
    public float MaxZoomOut    = 500f;

    private Vector3 InitPos;
    private Vector3 InitRotation;
    private float MinZoomZ, MaxZoomZ;
 
 
    public void Start()
    {
        //Instantiate(Arrow, Vector3.zero, Quaternion.identity);

        InitPos = transform.position;
        InitRotation = transform.eulerAngles;
        TargetZoomZ = InitPos.z;
        MaxZoomZ = InitPos.z - MaxZoomOut;
        MinZoomZ = InitPos.z + MaxZoomIn;
    }

    public void Update()
    {
        //PAN
        if (Input.GetMouseButton(2))
        {
            //(Input.mousePosition.x - Screen.width * 0.5)/(Screen.width * 0.5)
            transform.Translate(Vector3.right   * (Time.deltaTime * PanSpeed) * ((Input.mousePosition.x - Screen.width  * 0.5f) / (Screen.width * 0.5f)), Space.World);
            transform.Translate(Vector3.up * (Time.deltaTime * PanSpeed) * ((Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f)), Space.World);
        }
        else
        {
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
            {
                transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
            {
                transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed, Space.World);
            }

            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
            {
                transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
            {
                transform.Translate(Vector3.up * Time.deltaTime * -ScrollSpeed, Space.World);
            }
        }

        //ZOOM IN/OUT
        float newZoom = Input.GetAxis("Mouse ScrollWheel");
        if (newZoom != 0f)
        {
            TargetZoomZ += newZoom * ZoomSpeed;
            //print("zoom target: " + TargetZoomZ);
            TargetZoomZ = Mathf.Clamp(TargetZoomZ, MaxZoomZ, MinZoomZ);
            //print("capping zoom to: " + TargetZoomZ);
        }

        if (TargetZoomZ != transform.position.z)
        {
            if (Mathf.Abs(TargetZoomZ - transform.position.z) <= .25f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, TargetZoomZ);
                //print("snapping zoom: " + CurrentZoomZ);
            }
            else
            {
                float z = Mathf.Lerp(transform.position.z, TargetZoomZ, Time.deltaTime);
                //print("lerping zoom: " + z);
                transform.position = new Vector3(transform.position.x, transform.position.y, z);
            }
        }
    }
}
