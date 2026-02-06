using UnityEngine;

public class SimpleCanvasOpener : MonoBehaviour
{
    public Canvas canvas;
    public KeyCode toggleKey = KeyCode.F;
    
    private bool playerInZone = false;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError(" CANVAS PAS ASSIGNÉ !");
            return;
        }

        HideCanvas();
        Debug.Log("Canvas caché au démarrage");
    }

    CanvasGroup GetCanvasGroup()
    {
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        return canvasGroup;
    }
    
    void HideCanvas()
    {
        CanvasGroup canvasGroup = GetCanvasGroup();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    void ShowCanvas()
    {
        CanvasGroup canvasGroup = GetCanvasGroup();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvas.enabled = true;
    }

    void Update()
    {
        if (!playerInZone) return;

        if (Input.GetKeyDown(toggleKey))
        {
            ShowCanvas();
            Debug.Log(" Canvas OUVERT !");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter avec : {other.name} (Tag: {other.tag})");
        
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log(" PLAYER DANS LA ZONE !");
        }
    }

   
}