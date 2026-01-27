using System;
using UnityEngine;

namespace RunTime.TpTSystem
{
	public class IATurnBase :  MonoBehaviour
	{
		public event Action<IATurnBase> OnIATurn;
		public bool isMyTurn = false;

		public void StartIATurn()
		{
			isMyTurn = true;
		}

		private void ChooseCart()
		{
			if (!isMyTurn)
				return;
			
			
		}

		private void Validate()
		{
			
		}

		public void EndIATurn()
		{
			if (!isMyTurn)
				return;
			
			isMyTurn = false;
			OnIATurn?.Invoke(this);
		}
	}
}