using DG.Tweening;
using Gameplay;
using Gameplay.Inventory;
using Masque;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [Header("Ui Description")]
    [SerializeField] private GameObject UI;
    [SerializeField] private Image UIImage;
    [SerializeField] private TextMeshProUGUI NameText;
    
    [SerializeField] private SlotMachine machine;
    
    [Header("Reference")]
    [SerializeField] private InventorySysteme popUp;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Animation")]
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float hideDuration = 0.5f;
    
    private CanvasGroup canvasGroup;
    private bool isAnimating;

    private void Awake()
    {
        UI.SetActive(true);
    }
    private void Start()
    {
       UI.SetActive(false);
       
    }
    

    public void OnEnable()
    {
        popUp.onInventory += popUpAnim;
    }


    public void OnDisable()
    {
        popUp.onInventory -= popUpAnim;

    }
    private void popUpAnim(MaskData maskData)
    {
        UI.SetActive(true);
        UIImage.sprite = maskData.Icon;
        NameText.text = maskData.Name;
        
        
        audioSource.Play();
        
        Sequence seq = DOTween.Sequence();
        seq.Append(UI.transform.DOScale(Vector3.one, showDuration).SetEase(Ease.OutBack));
        seq.Join(canvasGroup.DOFade(1, showDuration));
        
        seq.AppendInterval(displayTime); 
        
        seq.Append(UI.transform.DOScale(Vector3.zero, hideDuration).SetEase(Ease.InBack));
        seq.Join(canvasGroup.DOFade(0, hideDuration));
    }

    
}
