using System;
using System.Collections.Generic;
using Masque;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.Inventory
{
	public class SlotUI :  MonoBehaviour,IPointerUpHandler,IPointerExitHandler
	{
		[SerializeField] private Image image;
		[SerializeField] private MaskManager maskManager;
		public MaskData maskData;
		
		public static event Action<SlotUI> OnEnter; 
		public static event Action<SlotUI> OnExit; 
		

		private void Start()
		{
			maskManager =  FindObjectOfType<MaskManager>();
		}
		public void UpdateSlotUI(MaskData maskData)
		{
			image.sprite =  maskData.Icon;
			this.maskData = maskData;
		}

		public void OnClick()
		{
			maskManager.currentMask = this.maskData;
			Debug.Log(maskData.Name);
		}
		
		public void OnPointerExit(PointerEventData eventData)
		{
			OnExit?.Invoke(this);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			OnEnter?.Invoke(this);
		}
	}
}