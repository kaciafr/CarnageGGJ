using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RunTime.TpTSystem
{
	public class HandCardHolder : MonoBehaviour
	{
		
	[Header("Cartes et slots")] [SerializeField]
	private Card selectCard;

	[SerializeField] private GameObject cardHolder;
	[SerializeField] private GameObject slotPrefab;
	[SerializeField] private GameObject bank;

	[SerializeField] private List<Card> cards;
	[SerializeField] private List<Transform> slots;

	[SerializeField] private int maxSlot = 5;
	private Vector3 targetPosition;
	private BankCard bankCard;


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
			
			card.BeginDragEvent.AddListener(BeginDrag);
			card.EndDragEvent.AddListener(EndDrag);
			cards.Add(card);
		}

	}
	
	private void BeginDrag(Card cards)
	{
		selectCard = cards;

	}

	private void EndDrag(Card cards)
	{
		if (selectCard == null)
			return;
		
		if(selectCard)
			targetPosition = selectCard.transform.localPosition;
			targetPosition = Vector3.zero;
			bool tweenCardReturn = true;
			selectCard.transform.DOLocalMove(targetPosition, 0.12f).SetEase(Ease.OutBack);
			selectCard = null;
		

	}

	void Update()
	{
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

		// Tween de la carte qui va être croisée
		cards[index].transform.SetParent(focusedParent);
		cards[index].transform.DOLocalMove(cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero, 0.12f).SetEase(Ease.OutBack);

		// Tween de la carte sélectionnée
		selectCard.transform.SetParent(crossedParent);
		selectCard.transform.DOLocalMove(selectCard.selected ? new Vector3(0, selectCard.selectionOffset, 0) : Vector3.zero, 0.12f).SetEase(Ease.OutBack);

		isCrossing = false;
	}
	public void RemoveCard(Card card)
	{
		cards.Remove(card);
		Destroy(card.gameObject);
	} 
	
	public void Add()
	{
		if (maxSlot <= 5)
		{
			for (int i = 0; i < maxSlot; i++)
			{
				GameObject slot = Instantiate(slotPrefab, slotPrefab.transform);
				slot.name = "BankSlot_" + i;
                    
				Card card = Instantiate(selectCard, slot.transform);
				card.name = "Card_" + (i + 1);
				card.transform.localPosition = Vector3.zero;

				card.BeginDragEvent.AddListener(BeginDrag);
				card.EndDragEvent.AddListener(EndDrag);
                    
				return;
			}
		}
            
	}
	}
	
}
