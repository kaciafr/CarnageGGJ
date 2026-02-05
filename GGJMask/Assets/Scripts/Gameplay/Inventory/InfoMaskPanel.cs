using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Inventory
{
	public class InfoMaskPanel : MonoBehaviour
	{
		[Header("Ui Description")]
		[SerializeField] private GameObject UI;
		[SerializeField] private Image UIImage;
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private TextMeshProUGUI NameText;
	
		[Header("Reference")]
		[SerializeField] private SlotUI slotUI;

		private void Awake()
		{
			UI.SetActive(true);
		}
		private void Start()
		{
			UI.SetActive(false);
		}

		private void OnEnable()
		{
			SlotUI.OnEnter += SeeInterface;
			SlotUI.OnExit += HideInterface;
		}


		private void OnDisable()
		{
			SlotUI.OnEnter -= SeeInterface;
			SlotUI.OnExit -= HideInterface;
		
		}

		private void SeeInterface(SlotUI obj)
		{
			UIImage.sprite = obj.maskData.Icon;
			NameText.text = obj.maskData.Name;
			text.text = obj.maskData.Description;
			UI.gameObject.SetActive(true);
		}

		private void HideInterface(SlotUI obj)
		{
			UI.SetActive(false);
		}

	}
}
