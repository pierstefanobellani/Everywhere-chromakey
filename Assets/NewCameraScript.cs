using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Picture()
    {
        if(NativeCamera.DeviceHasCamera()){
            NativeCamera.TakePicture( CameraCallback, -1, preferredCamera = PreferredCamera.Default );
        }
    }

    /*public void Video() {
        if(NativeCamera.DeviceHasCamera()){
            NativeCamera.RecordVideo( CameraCallback, quality = Quality.Default, maxDuration = 0, maxSizeBytes = 0L, preferredCamera = PreferredCamera.Default );
        }
    }*/
}
