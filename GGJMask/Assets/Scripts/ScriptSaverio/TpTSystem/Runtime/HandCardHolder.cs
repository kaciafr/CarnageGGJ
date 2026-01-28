using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RunTime.TpTSystem
{
    public class HandCardHolder : MonoBehaviour
    {
       [Header("Cartes et slots")]
       [SerializeField] private Card selectCard;
       [SerializeField] private GameObject cardHolder;
       [SerializeField] private GameObject slotPrefab;
       
       [SerializeField] private List<Card> cards;
       [SerializeField] private List<Transform> slots;
       
       [SerializeField] private int maxSlot = 2;
       
       private RectTransform rect;
       private Transform referenceLocation;

       private bool isCrossing = false;
       

       private void Start()
       {
	      
	       for (int i = 0; i < maxSlot; i++)
	       {
		       GameObject slot = Instantiate(slotPrefab, cardHolder.transform);
		       slots.Add(slot.transform);
	       }
	       
	       for (int i = 0; i < maxSlot; i++)
	       {
		       Card card = Instantiate(selectCard, slots[i]);
		       card.name = "Card_" + i;
		       card.transform.localPosition = Vector3.zero;

		       // Ajouter les listeners
		       card.BeginDragEvent.AddListener(BeginDrag);
		       card.EndDragEvent.AddListener(EndDrag);

		       cards.Add(card);
	       }
	       
       }
       private void BeginDrag(Card cards)
       {
	       selectCard = cards;
	       // Debug.Log("Quitte la main");
       }

       private void EndDrag(Card cards)
       {
	       if (selectCard == null)
		       return;
	       
	       //Debug.Log("Retourne la main");
	       Vector3 targetPosition = selectCard.transform.localPosition;
	       targetPosition = Vector3.zero;
	       bool tweenCardReturn=true;
	       
	       selectCard.transform.DOLocalMove(targetPosition, 0.12f).SetEase(Ease.OutBack);
	       
	       
	       selectCard =  null;
	       
       }

       void Update()
       {
	       if(selectCard == null) return;

	       if (isCrossing) return;
	       for (int i = 0; i < cards.Count; i++)
	       {

		       if (selectCard.transform.position.x > cards[i].transform.position.x)
		       {
			       if (selectCard.ParentIndex() < cards[i].ParentIndex())
			       {
				       Swap(i);
				       break;
			       }
		       }

		       if (selectCard.transform.position.x < cards[i].transform.position.x)
		       {
			       if (selectCard.ParentIndex() > cards[i].ParentIndex())
			       {
				       Swap(i);
				       break;
			       }
		       }
	       }
	       
       }
       
       void Swap(int index)
       {
	       isCrossing = true;

	       Transform focusedParent = selectCard.transform.parent;
	       Transform crossedParent = cards[index].transform.parent;

	       cards[index].transform.SetParent(focusedParent);
	       cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
	       selectCard.transform.SetParent(crossedParent);

	       isCrossing = false;

	      // if (cards[index].cardVisual == null)
		       return;

	       //bool swapIsRight = cards[index].ParentIndex() > selectCard.ParentIndex();
	       
	      /* cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

	       //Updated Visual Indexes
	       foreach (Card card in cards)
	       {
		       card.cardVisual.UpdateIndex(transform.childCount);
	       }*/
       }
       
    }
}
