using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string sceneName; 
  
   
    [SerializeField] private Button buttonMenu;

    void Start()
    {
        buttonMenu.onClick.AddListener(() => LoadScene(sceneName));
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}