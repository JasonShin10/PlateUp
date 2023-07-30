using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Meat;

public class Chicken : Food
{
    public enum ChickenState
    {
        Raw,
        HalfCooked,
        FullyCooked,
        Burned
    }

    public ChickenState CurrentChickenState { get; set; } = ChickenState.Raw;
    public override int CookingState => (int)CurrentChickenState;
}
