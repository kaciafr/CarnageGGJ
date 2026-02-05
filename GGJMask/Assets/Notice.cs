using UnityEngine;

public class Notice : MonoBehaviour
{
    [SerializeField] GameObject NoticePrefab;
    private bool isSee = false;

    private void Start()
    {
        NoticePrefab.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isSee = !isSee;
            NoticePrefab.SetActive(isSee);
        }
    }

    public void ShowNotice()
    {
        isSee = !isSee;
        NoticePrefab.SetActive(isSee);
    }

}
