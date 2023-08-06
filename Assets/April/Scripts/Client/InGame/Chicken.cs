using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class Chicken : Food
    {
        public override MenuList MenuType => MenuList.Chicken;
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
}

