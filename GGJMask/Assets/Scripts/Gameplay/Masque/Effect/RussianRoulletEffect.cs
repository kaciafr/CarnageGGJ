using System;
using Gameplay.CardSystem;
using Gameplay.CardSystem.Collections;
using Masque;
using Masque.mask.RussianRoulletMask;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Effect/RussianRoulletEffect")]
public class RussianRoulletEffect : MaskEffect
{
    public event Action<RussianRoulletEffect> IsVisibile; 
    
   // public override void ApplyEffect( )
    //{
     //   IsVisibile?.Invoke(this);
    //}

    public override void ApplyEffect(TurnManager manager)
    {
        throw new NotImplementedException();
    }

    public override void RemoveEffect( )
    {
        
    }
    
}
