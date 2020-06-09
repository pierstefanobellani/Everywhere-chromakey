using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PositionScript : MonoBehaviour
{

    public Camera cam;
    public Image colorSelected;
    Vector3 screenPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, cam, out localpoint);

        Debug.Log(localpoint);
        colorSelected.transform.position = localpoint;*/

            screenPoint.x = Input.mousePosition.x;
        screenPoint.y = Input.mousePosition.y;
        screenPoint.z = 100.0f; //distance of the plane from the camera
            colorSelected.transform.position = cam.ScreenToWorldPoint(screenPoint);

    }
}
