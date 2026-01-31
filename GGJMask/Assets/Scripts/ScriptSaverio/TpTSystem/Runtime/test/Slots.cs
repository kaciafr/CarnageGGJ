using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
	public class Slots : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
	{
		public bool hovering;
		public Cards currentCard;
		public bool IsFree => currentCard == null;

		public void OnPointerEnter(PointerEventData eventData)
		{
			hovering = true;
			
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			hovering = false;
		}
		
		public void SetCard(Cards cards)
		{
			
			currentCard =  cards;
		}

		public void ClearCard()
		{
			currentCard = null;
			Destroy(currentCard);
		}
	}
}