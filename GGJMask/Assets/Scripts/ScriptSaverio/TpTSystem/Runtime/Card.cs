using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace RunTime.TpTSystem
{
	public class Card : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerDownHandler
	{
		private bool isDragging = false;
		private bool wasDragged;
		
		private Vector3 offset;
		private Canvas canvas;
		private Image imageComponent;
		
		[Header("FollowSpeed")]
		[SerializeField] private float moveSpeedLimit = 50f;
		
		[Header("Rotation followSpeed")]
		[SerializeField] private float rotateSpeedLimit = 1f;
		[SerializeField] private float rotatebreakLimit = 2f;
		
		
		
		[HideInInspector] public UnityEvent<Card> BeginDragEvent ;
		[HideInInspector] public UnityEvent<Card> EndDragEvent ;
		[HideInInspector] public UnityEvent<Card> PointerEnterEvent ;
		[HideInInspector] public UnityEvent<Card> PointerExitEvent ;
		[HideInInspector] public UnityEvent<Card,bool> PointerUpEvent ;
		[HideInInspector] public UnityEvent<Card> PointerDownEvent ;
		[HideInInspector] public UnityEvent<Card,bool> SelectedCardEvent;

		private bool selected;


		void Start()
		{
			canvas = GetComponentInParent<Canvas>();
			imageComponent = GetComponent<Image>();
		}

		void Update()
		{
			if (isDragging)
			{
				Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
				mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
				
				Vector2 targetPosition = Camera.main.ScreenToWorldPoint(mouseScreenPos) - offset;
				Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
				Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
				//transform.Translate(velocity * Time.deltaTime);
				transform.position = Vector2.MoveTowards(transform.position, mouseScreenPos, moveSpeedLimit * Time.deltaTime);
				
				float mouseX = Mouse.current.delta.value.x /rotatebreakLimit;

				transform.rotation *= Quaternion.Euler(0, 0, mouseX);

			}
			
		}
		
		public void OnDrag(PointerEventData eventData)
		{
			
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			Debug.Log("OnBeginDrag");
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
			offset = mousePosition - (Vector2)transform.position;
			isDragging = true;
			
			
			canvas.GetComponent<GraphicRaycaster>().enabled = false;
			imageComponent.raycastTarget = false;
			
			

			wasDragged = true;
			BeginDragEvent.Invoke(this);
		}
		

		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log("OnEndDrag");
			EndDragEvent.Invoke(this);
			transform.eulerAngles = new Vector3(0,0,0);
			isDragging = false;
			canvas.GetComponent<GraphicRaycaster>().enabled = true;
			imageComponent.raycastTarget = true;
			
			StartCoroutine(FrameWait());

			IEnumerator FrameWait()
			{
				yield return new WaitForEndOfFrame();
				wasDragged = false;
			}
			
			wasDragged=false;
			
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
			if (selected)
				transform.localPosition += transform.up*90;
			else
				transform.localPosition = Vector3.zero;

		}

		public void OnPointerDown(PointerEventData eventData)
		{
			
		}
		
	}
}