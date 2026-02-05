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
            canvas.gameObject.SetActive(true);
            Debug.Log(" Canvas OUVERT !");
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

   
}