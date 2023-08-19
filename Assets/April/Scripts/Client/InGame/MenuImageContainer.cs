using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Runtime Collection", menuName = "April/Runtime/SpawnedObjectContainer")]
public class MenuImageContainer : ScriptableObject
{
    public List<List<Sprite>> MenuSpriteGroups = new List<List<Sprite>>();
    public List<Sprite> normalBeefSprites;
    public List<Sprite> thickBeefSprites;

    private void OnEnable()
    {
        MenuSpriteGroups.Add(normalBeefSprites);
        MenuSpriteGroups.Add(thickBeefSprites);
    }
}
