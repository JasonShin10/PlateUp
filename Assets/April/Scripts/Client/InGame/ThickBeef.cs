using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class ThickBeef : Food
    {
        public override MenuList MenuType => MenuList.ThickBeef;
        public enum ThickBeefState
        {
            Raw,
            Medium,
            WellDone,
            Burned
        }

        public ThickBeefState CurrentThickBeefState { get; set; } = ThickBeefState.Raw;
        public override int CookingState => (int)CurrentThickBeefState;
    }
}

