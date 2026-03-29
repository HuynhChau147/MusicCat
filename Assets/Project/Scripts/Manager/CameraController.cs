using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Camera cam;
    [SerializeField] UIMN uiManager;

    private int lastWidth;
    private int lastHeight;

    void Start()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        UpdateCamera();
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            UpdateCamera();
        }
    }

    void UpdateCamera()
    {
        cam.orthographicSize = Screen.width <= Screen.height ? 5f : 3f;
        uiManager.SetUpCanvas();
    }
}