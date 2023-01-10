using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Default", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public DirectionalSprite usePlayerSprite;
    public virtual void Use(Vector3 position)
    {
        Debug.Log("Using " + itemName);
    } 
}
