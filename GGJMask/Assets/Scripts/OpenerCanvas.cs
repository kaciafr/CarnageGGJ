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
        
        canvas.gameObject.SetActive(false);
        Debug.Log("Canvas désactivé au démarrage");
    }

    void Update()
    {
        if (!playerInZone) return;

        if (Input.GetKeyDown(toggleKey))
        {
            bool newState = !canvas.gameObject.activeSelf;
            canvas.gameObject.SetActive(newState);
            Debug.Log($"Canvas maintenant : {(newState ? "OUVERT" : "FERMÉ")}");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"👤 Trigger Enter avec : {other.name} (Tag: {other.tag})");
        
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log(" PLAYER DANS LA ZONE !");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log(" PLAYER SORTI DE LA ZONE");
        }
    }
}