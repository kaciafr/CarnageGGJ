using Gameplay.CardSystem;
using UnityEngine;

public class StartedGame : MonoBehaviour
{
	[SerializeField] private StartPartie player;
	[SerializeField] private GameObject CardGame;
	[SerializeField] private TurnManager GameManager;
	[SerializeField] private CardPlayer StartPartie;

	private void Start()
	{
		CardGame.SetActive(false);
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && player.canStartPartie)
		{
			player.canStartPartie = !player.canStartPartie;
			CardGame.SetActive(player.canStartPartie);
			
			StartPartie.PrepareForGame(GameManager);
			StartPartie.PrepareTurn(GameManager);
		}
	}
    
}
