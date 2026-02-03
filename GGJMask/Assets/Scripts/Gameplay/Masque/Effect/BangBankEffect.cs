using System.Collections.Generic;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Gameplay.CardSystem.UI;
using Masque;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/BangBankEffect")]
public class BangBankEffect :  MaskEffect
{
	public override void ApplyEffect(UtilitiesCards cards)
	{
		cards.defenseSize = 0;
	}

	public override void RemoveEffect(UtilitiesCards cards)
	{
	}

}
