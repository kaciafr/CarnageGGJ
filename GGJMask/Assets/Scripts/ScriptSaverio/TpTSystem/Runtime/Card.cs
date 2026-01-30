using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
	public class Card : MonoBehaviour, IDragHandler,IDropHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler,
		IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
	{
		public bool isDragging = false;
		public bool wasDragged;
		private Vector3 offset;
		private Canvas canvas;
		private Image imageComponent;

		[Header("FollowSpeed")] 
		[SerializeField]
		private float moveSpeedLimit;


		[Header("Rotation followSpeed")] [SerializeField]
		private float rotateSpeedLimit = 1f;
		[SerializeField] private float rotatebreakLimit = 2f;
		[SerializeField] private float max = 10;
		[SerializeField] private float min = -10;
		[SerializeField] private GameObject shadow;
		
		[HideInInspector] public UnityEvent<Card> BeginDragEvent;
		[HideInInspector] public UnityEvent<Card> EndDragEvent;
		[HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;
		[HideInInspector] public UnityEvent<Card, bool> SelectedCardEvent;
		[HideInInspector] public UnityEvent<Card> SelectCardEvent;
		
		public BankCard bankCard;
		public HandCardHolder handCardHolder;

		public bool selected;
		public float selectionOffset = 50;
		private bool isPlaced = false;

		private float pointerUpTime;
		private float pointerDownTime;
		private float angleY;
		
		void Start()
		{
			canvas = GetComponentInParent<Canvas>();
			handCardHolder = GetComponentInParent<HandCardHolder>();
			imageComponent = GetComponent<Image>();
		}

		void Update()
		{
			if (isDragging)
			{
				Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
				mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
				transform.position = Vector2.MoveTowards(transform.position, mouseScreenPos, moveSpeedLimit * Time.deltaTime);

				float mouseX = Mouse.current.delta.value.x / rotatebreakLimit;
				angleY += mouseX * rotateSpeedLimit;
				angleY = Mathf.Clamp(angleY, -20, 20);
				
				transform.rotation = Quaternion.Euler(0, 0, angleY);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{

		}
		public void OnBeginDrag(PointerEventData eventData)
		{
			Debug.Log("OnBeginDrag");

			BeginDragEvent.Invoke(this);
			
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
			offset = mousePosition - (Vector2)transform.position;
			isDragging = true;
			shadow.transform.localPosition = Vector3.down*90;
			
			canvas.GetComponent<GraphicRaycaster>().enabled = false;
			
			imageComponent.raycastTarget = false;

			wasDragged = true;
			
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			EndDragEvent.Invoke(this);
			Debug.Log("OnEndDrag");
			transform.eulerAngles = new Vector3(0, 0, 0);
			isDragging = false;
			canvas.GetComponent<GraphicRaycaster>().enabled = true;
			imageComponent.raycastTarget = true;
			shadow.transform.localPosition = Vector3.zero;
			
			StartCoroutine(FrameWait());
			IEnumerator FrameWait()
			{
				yield return new WaitForEndOfFrame();
				wasDragged = false;
			}
		}
		public void OnPointerEnter(PointerEventData eventData)
		{

		}
		public void OnPointerExit(PointerEventData eventData)
		{

		}
		public void OnPointerUp(PointerEventData eventData)
		{
			Debug.Log("OnPointerUp");
			selected = !selected;
			pointerUpTime = Time.time;
			PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > 2f);
			Select();

			SelectedCardEvent.Invoke(this, selected);

		}
		public void Select()
		{
			if (selected)
			{
				transform.localPosition += transform.up * 90;
				shadow.transform.localPosition -= transform.up * 90;
				ChangeParent();
			}
			else
			{
				transform.localPosition = Vector3.zero;
				shadow.transform.localPosition = Vector3.zero;
			}
			
		}
		public void OnPointerDown(PointerEventData eventData)
		{

		}
		public void OnDrop(PointerEventData eventData)
		{
			
		}
		public int ParentIndex()
		{
			
			return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
		}

		public void ChangeParent()
		{
			if (bankCard.maxSlots <= 2)
			{
				bankCard.maxSlots++;
				bankCard.Add();
				handCardHolder.RemoveCard(this);
			}
			else
			{
				Debug.LogError("Max slots exceeded");
			}
			Debug.Log(bankCard.maxSlots);

		}
		
	}
}