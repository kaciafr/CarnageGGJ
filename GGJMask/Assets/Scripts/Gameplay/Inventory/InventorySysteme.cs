using System;
using System.Collections.Generic;
using Gameplay.Inventory;
using Masque;
using UnityEngine;

public class InventorySysteme : MonoBehaviour
{
	[SerializeField] private GameObject inventoryPrefab;
	[SerializeField] private GameObject slotPrefab;
	
	[SerializeField] private List<MaskData> slots;
	private MaskData maskData;
	
	private int maxSlots = 6;
	private bool isActive = true;

	private void Start()
	{
		inventoryPrefab.SetActive(!isActive);
	}
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			inventoryPrefab.SetActive(isActive);
			isActive = !isActive;
			Debug.Log(isActive);
		}
			
	}
	
	public event Action<MaskData> onInventory;
	
	public void AddMAsk(MaskData maskWin)
	{
		if(slots.Count > maxSlots)
			return;

		foreach (MaskData slot in slots)
		{
			if (maskWin.ID == slot.ID)
			{
				Debug.Log($"Mask Exist {slot.Name}");
				return;
			}
		}
		
		slots.Add(maskWin);
		onInventory?.Invoke(maskWin);
		Debug.Log("Added " + maskWin.ID);
		
		GameObject newSlot = Instantiate(slotPrefab, inventoryPrefab.transform);

		
		SlotUI slotUI = newSlot.GetComponent<SlotUI>();
		slotUI.UpdateSlotUI(maskWin);
	}
}
