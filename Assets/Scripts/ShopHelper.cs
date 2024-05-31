using System.Collections.Generic;
using Enums;
using UnityEngine;

public class ShopHelper : MonoBehaviour
{
    public List<GameObject> warriorsGameObjects;
    public readonly Dictionary<CharacterType, Warrior> CharTypeToFeatureDict = new();

    // Start is called before the first frame update
    private void Awake()
    {
        PopulateCharTypeToFeatureDict();
    }

    private void PopulateCharTypeToFeatureDict()
    {
        foreach (var warrior in warriorsGameObjects)
        {
            var charFeature = warrior.GetComponent<Warrior>();
            CharTypeToFeatureDict.Add(charFeature.characterType, charFeature);
        }
    }
}