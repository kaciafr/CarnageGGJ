using UnityEngine;
using UnityEngine.UI;

public class CameraViewer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private Button Buttoncam;
    [SerializeField] private Button Buttoncam2;

    private void Start()
    {

        SwitchToCamera(cam);
    }

    private void SwitchToCamera(Camera targetCamera)
    {
        cam.gameObject.SetActive(cam == targetCamera);
        cam2.gameObject.SetActive(cam2 == targetCamera);
    }
}