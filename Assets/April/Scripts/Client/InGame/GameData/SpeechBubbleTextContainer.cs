using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    [CreateAssetMenu(fileName = "Runtime Collection", menuName = "April/Runtime/SeechBubbleTextContainer")]
public class SpeechBubbleTextContainer : ScriptableObject
{
    public List<string> textContainer = new List<string>();

    public Image angryImage;
}
