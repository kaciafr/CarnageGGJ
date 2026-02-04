using Masque;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Inventory
{
	public class SlotUI :  MonoBehaviour
	{
		[SerializeField] private Image image;

		public void UpdateSlotUI(MaskData maskData)
		{
			image.sprite =  maskData.Icon;
		}
	}
}