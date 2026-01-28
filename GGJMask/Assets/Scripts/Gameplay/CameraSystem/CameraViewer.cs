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
        Buttoncam.onClick.AddListener(() => SwitchToCamera(cam));
        Buttoncam2.onClick.AddListener(() => SwitchToCamera(cam2));

        SwitchToCamera(cam);
    }

    private void SwitchToCamera(Camera targetCamera)
    {
        cam.gameObject.SetActive(cam == targetCamera);
        cam2.gameObject.SetActive(cam2 == targetCamera);
    }
}