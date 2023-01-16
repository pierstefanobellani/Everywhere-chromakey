using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditChromakey : MonoBehaviour
{
    private Color ChromakeyColor;
    private Texture2D ChromakeyTexture;
    public float ChromakeyAlpha;
    public float ChromakeyThreshold;
    public float ChromakeySmoothness;
    public GameObject Foreground;

    void Start ()
    {
        ChromakeyTexture = Foreground.GetComponent<Texture2D>();
        ChromakeyColor = Foreground.GetComponent<Color>();
    }
}
