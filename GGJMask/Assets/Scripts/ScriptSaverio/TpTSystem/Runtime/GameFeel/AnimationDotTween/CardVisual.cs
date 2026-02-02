using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
    public class CardVisual : MonoBehaviour
    {
	    
	    [Header("Cards")]
	    public Cards cards;

	    [SerializeField] private GameObject cardPrefab;
	    private Transform cardTransform;
	    
	    [Header("References")]
	    public Transform shadow;
	    private float shadowOffset = 20;
	    private Canvas shadowCanvas;
	    [SerializeField] private float shake;
	    [SerializeField] private float scale;
	    [SerializeField] private Image cardImage;
	    
	    [Header("Select Parameters")]
	    [SerializeField] private float selectPunchAmount = 20;
	    
	    private Transform cardsTransform;
	    private Canvas canvas;
	    private bool initialize = false;

	    public void Initialize(Cards target)
	    {
		    Debug.Log("Initializing CardVisual");
		    cards = target;
		    cardsTransform = target.transform;
		    canvas = GetComponent<Canvas>();
		    shadowCanvas = shadow.GetComponent<Canvas>();
		    
		    cards.BeginDragEvent.AddListener(BeginDragEvent);
		    cards.EndDragEvent.AddListener(EndDragEvent);
		    cards.EnterEvent.AddListener(EnterEvent);
		    cards.ExitEvent.AddListener(ExitEvent);
		    
		    initialize = true;
	    }
	    

	    private void EnterEvent(Cards card)
	    {
		    cards = card;
		    Debug.Log("Aigle");
		    shadow.transform.localPosition -= transform.up * 30;
            
		    transform.DOScale(scale, 0.5f);
            
		    transform.DOShakePosition(1f, shake);
            
		    transform.DOShakeRotation(1f, shake);
	    }

	    private void ExitEvent(Cards card)
	    {
		    cards = card;
		    Debug.Log("Rat");
		    transform.DOKill();
		    shadow.transform.localPosition = Vector3.zero;
            
		    transform.DOScale(1, 0.5f);
		    transform.localRotation = Quaternion.Euler(0,0,0);
	    }

	    private void BeginDragEvent(Cards card)
	    {
		    cards = card;
		    Debug.Log("Chat");
	    }

	    private void EndDragEvent(Cards card)
	    {
		    cards = card;
		    Debug.Log("Chien");
	    }
    }
}
