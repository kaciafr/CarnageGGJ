using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
    public class CardVisual : MonoBehaviour
    {
	    
	    [Header("References")]
	    public Cards cards;
	    public Transform shadow;
	    private float shadowOffset = 20;
	    private Vector2 shadowDistance;
	    private Canvas shadowCanvas;
	    [SerializeField] private Transform shakeParent;
	    [SerializeField] private Transform tiltParent;
	    [SerializeField] private Image cardImage;

	    [Header("Scale Parameters")]
	    
	    [Header("Select Parameters")]
	    [SerializeField] private float selectPunchAmount = 20;
	    
	    private Transform cardsTransform;
	    private Canvas canvas;
	    private void Start()
	    {
		    shadowDistance = shadow.localPosition;
		    
		    canvas = GetComponent<Canvas>();
		    shadowCanvas = shadow.GetComponent<Canvas>();
		    cards = GetComponent<Cards>();
	
		    cards.BeginDragEvent.AddListener(BeginDragEvent);
		    cards.EndDragEvent.AddListener(EndDragEvent);
		    cards.EnterEvent.AddListener(EnterEvent);
		    cards.ExitEvent.AddListener(ExitEvent);
		    
	    }

	    private void EnterEvent(Cards card)
	    {
		    cards = card;
		    Debug.Log("Enculer");
	    }

	    private void ExitEvent(Cards card)
	    {
		    cards = card;
		    Debug.Log("Rat");
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
