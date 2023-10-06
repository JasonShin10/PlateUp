using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonInteract
{
     float ProgressValue { get; set; }
     float MaxValue { get; set; }


    void ButtonInteract();


}
