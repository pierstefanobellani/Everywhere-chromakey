using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SmartDLL;
using UnityEngine.Video;

public class Explorer : MonoBehaviour
{
    //public Text eText;
    public Button openExplorerButton;

    //public RawImage background;
    public Material backgroundMaterial;
    public GameObject backgroundSystem;
    public RenderTexture videoTexture;


    public SmartFileExplorer fileExplorer = new SmartFileExplorer();

    private bool readText = false;

    void OnEnable()
    {
        openExplorerButton.onClick.AddListener(delegate { ShowExplorer(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fileExplorer.resultOK && readText)
        {
            ReadText(fileExplorer.fileName);
            readText = false;
        }
    }

    void ShowExplorer()
    {
        string initialDir = @"C:\";
        bool restoreDir = false;
        string title = "Open a Media File";
        string defExt = "";
        string filter = "";

        fileExplorer.OpenExplorer(initialDir,restoreDir,title,defExt,filter);
        readText = true;
    }

    void ReadText(string path)
    {
        //eText.text = File.ReadAllText(path);
        WWW www = new WWW("file:///" + path);
        if (path.EndsWith(".mp4"))
        {
            backgroundSystem.SetActive(true);
            backgroundMaterial.SetTexture("_BaseMap", videoTexture);
            backgroundMaterial.SetTexture("_EmissionMap", videoTexture);
            backgroundSystem.GetComponent<VideoPlayer>().url = path;
            Debug.Log("VIDEO");
        }
        else
        {
            backgroundSystem.SetActive(false);
            backgroundMaterial.SetTexture("_BaseMap", www.texture);
            backgroundMaterial.SetTexture("_EmissionMap", www.texture);
            Debug.Log("IMAGE");
        }
    }
}
