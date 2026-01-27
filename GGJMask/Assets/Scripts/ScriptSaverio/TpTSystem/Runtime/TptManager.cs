using System;
using RunTime.TpTSystem.Data;
using UnityEngine;

namespace RunTime.TpTSystem
{
	public class TptManager : MonoBehaviour
	{
		[SerializeField] private PlayerData playerData;
		[SerializeField] private PlayerTurnBase player;
		[SerializeField] private IATurnBase Ia;
		
		

		private void Awake()
		{
			player.OnPlayerTurn += PlayerTurnFinish;
			Ia.OnIATurn += IaTurnFinish;
		}

		private void Start()
		{
			StartPlayerTurn();
		}
		
		public void StartPlayerTurn()
		{
			if (!Ia.isMyTurn)
			{
				player.StartTurn();
			}
			else
			{
				StartIATurn();
			}
		}

		private void PlayerTurnFinish(PlayerTurnBase player)
		{
			Debug.Log("PlayerTurnFinish");
			StartIATurn();
		}

		private void StartIATurn()
		{
			Debug.Log("StartIATurn");
			if (!player.isMyTurn)
			{
				Ia.StartIATurn();
			}
			else
			{
				player.StartTurn();
			}
		}

		private void IaTurnFinish(IATurnBase ia)
		{
			Debug.Log("IaTurnFinish");
			EndTurn();
		}

		private void EndTurn()
		{
			CalculatePoints();
			
			WhoWin();
			
			Debug.Log("End of the manche");
			
			if (playerData.PV <= 1)
			{
				EndGame();
			}
			else
			{
				StartPlayerTurn();
				playerData.PV--;
			}
		}

		private void EndGame()
		{
			Debug.Log("EndGame");
		}

		private void CalculatePoints()
		{
			Debug.Log("Faire la Logique des calcules");
		}
		private void WhoWin()
		{
			Debug.Log("Logique de qui à gagné");
		}
	}
}