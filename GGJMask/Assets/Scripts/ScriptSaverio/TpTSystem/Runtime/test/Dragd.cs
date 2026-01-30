using System;
using RunTime.TpTSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dragd : MonoBehaviour
{
	private Camera cam;
	private bool isDragging;
	private Vector3 offset;
	private float zDistance;
	
	public event Action<Dragd> OnDrag;
	public event Action<Dragd> OnEndDrag;

	void Awake()
	{
		cam = Camera.main;
	}

	void Update()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame)
			TryStartDrag();

		if (Mouse.current.leftButton.isPressed && isDragging)
			Drag();

		if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
			EndDrag();
	}

	void TryStartDrag()
	{
		Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			if (hit.transform == transform)
			{
				zDistance = cam.WorldToScreenPoint(transform.position).z;
				offset = transform.position - MouseWorldPosition();
				isDragging = true;
			}
		}
	}

	void Drag()
	{
		transform.position = MouseWorldPosition() + offset;
		OnDrag?.Invoke(this);
	}

	void EndDrag()
	{
		isDragging = false;
		OnEndDrag?.Invoke(this);
	}

	Vector3 MouseWorldPosition()
	{
		Vector3 mouse = Mouse.current.position.ReadValue();
		mouse.z = zDistance;
		return cam.ScreenToWorldPoint(mouse);
	}
}