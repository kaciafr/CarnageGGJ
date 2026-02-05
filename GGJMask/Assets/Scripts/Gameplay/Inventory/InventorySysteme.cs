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
		Debug.Log("Added " + maskWin.ID);
		
		GameObject newSlot = Instantiate(slotPrefab, inventoryPrefab.transform);

		
		SlotUI slotUI = newSlot.GetComponent<SlotUI>();
		slotUI.UpdateSlotUI(maskWin);
	}
}
