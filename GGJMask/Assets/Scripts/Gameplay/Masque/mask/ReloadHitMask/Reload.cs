using System.Linq;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Masque;
using UnityEngine;

public class Reload : MonoBehaviour
{
	/*[SerializeField] private ReloadHitEffect reloadHitEffect;
	[SerializeField] private GameObject uiReload;
	[SerializeField] private MaskManager maskManager;
	
	public Hand hand { get; private set; }
	public Deck deck { get; private set; }
	private void Awake()
	{
		hand = new Hand(5);
		deck = new Deck();
	}
	public void ApplyEffect()
	{
		Debug.Log("ReloadHitEffect");

		int count = hand.MaxCards;
		var removeCard = hand.Cards.ToArray();
		Debug.Log(count);
		Debug.LogError(deck.Count);

		foreach (var c in removeCard)
		{
			hand.RemoveCard(c);
		}
		
		for (int i = 0; i < count; i++)
		{
			var drawn = deck.DrawCard();
			if (drawn != null)
				hand.AddCard(drawn);
			Debug.Log("Removing: " + drawn);
		}
	}*/
}
