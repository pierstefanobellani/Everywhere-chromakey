using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CameraChromakey : MonoBehaviour
{
    [Header("Foreground camera")]
    private bool camAvailable;
    private WebCamTexture backCam;

    //private Texture defaultBackground;

    public RawImage foreground;
    public AspectRatioFitter fit;

    public Material material;
    //public Texture sample;  //for debug purpose

    [Header("UI")]
    public Slider smoothness;
    public Slider alpha;
    public Slider threshold;
    public Image colorSelected;

    [Space]
    [Header("Spare")]
    public Color picker = new Color(0, 255, 0, 1);
    public Camera cam;

    private float r;
    private float g;
    private float b;

    private Vector3 screenPoint;

    private Vector2 localPoint;


    private void Start()
    {
        //defaultBackground = material.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        //find device camera(s)
        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }


        //select back camera or not and use it
        for (int i = 0; i < devices.Length; i++)
        {
            if (/*!*/devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        //activate the camera recording
        backCam.Play();

        //foreground.texture = backCam; //foreground does NOT have a texture, it is its materila texture
        material.SetTexture("_MainTex", backCam);

        camAvailable = true;

        threshold.value = .1f;
        smoothness.value = 0;// .1f;
        alpha.value = 0;// .6f;

        colorSelected.gameObject.SetActive(false);

        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        //take camera proportion and keep it to display the image on the "foreground" (UI RawImage)
        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        foreground.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        foreground.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        //sliders values control the shader values
        material.SetFloat("Vector1_5959931A", smoothness.value);
        material.SetFloat("Vector1_CB708A69", alpha.value);
        material.SetFloat("Vector1_D7B42357", threshold.value);

        screenPoint.x = Input.mousePosition.x; //pointless, I was following some unity forum suggestions
        screenPoint.y = Input.mousePosition.y; //pointless, I was following some unity forum suggestions
        screenPoint.z = 100.0f; //distance of the plane from the camera

        //Debug.Log(backCam.width + " " + backCam.height);

        //if I'm NOT touching the sliders, then go on
        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            //show the colorSelected element only if I'm color picking
            colorSelected.gameObject.SetActive(true);

            //read a certain amount of pixels to get an average color value
            for (int x = (int)Input.mousePosition.x - 5; x < (int)Input.mousePosition.x + 5; x++)
            {
                for (int y = (int)Input.mousePosition.y - 5; y < (int)Input.mousePosition.y + 5; y++)
                {
                    //"foreground" coordinates are NOT the same as the "backCam" ones (this depends on screen size and device camera resolution, respectively) 
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(foreground.rectTransform, Input.mousePosition, cam, out localPoint);

                    //remap the coordinates
                    int px = Mathf.Clamp(0, (int)(((localPoint.x - foreground.rectTransform.rect.x) * backCam.width) / foreground.rectTransform.rect.width), backCam.width);
                    int py = Mathf.Clamp(0, (int)(((localPoint.y - foreground.rectTransform.rect.y) * backCam.height) / foreground.rectTransform.rect.height), backCam.height);

                    r += backCam.GetPixel(px, py).r;
                    g += backCam.GetPixel(px, py).g;
                    b += backCam.GetPixel(px, py).b;
                }
            }

            //divide the r, g, b values by the total number of read pixels (otherwise you'll always get white)
            r /= 121;
            g /= 121;
            b /= 121;
            picker = new Color(r, g, b, 1);

            colorSelected.color = picker; // new Color(picker.r, picker.g, picker.b, 1);
            material.SetColor("Color_EAC10D64", picker);
            
            colorSelected.transform.position = cam.ScreenToWorldPoint(screenPoint);
        }

        else
        {
            colorSelected.gameObject.SetActive(false);
        }


        /*if (Input.GetMouseButtonUp(0))
        {
            material.SetColor("Color_EAC10D64", picker);
            //Debug.Log("mouse up");
        }*/
    }
}