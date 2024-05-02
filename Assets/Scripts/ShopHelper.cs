using System.Collections.Generic;
using Enums;
using UnityEngine;

public class ShopHelper : MonoBehaviour
{
    public List<GameObject> warriorsGameObjects;
    public readonly Dictionary<CharacterType, CharacterFeature> CharTypeToFeatureDict = new();

    // Start is called before the first frame update
    private void Start()
    {
        PopulateCharTypeToFeatureDict();
    }

    private void PopulateCharTypeToFeatureDict()
    {
        foreach (var warrior in warriorsGameObjects)
        {
            var charFeature = warrior.GetComponent<CharacterFeature>();
            CharTypeToFeatureDict.Add(charFeature.characterType, charFeature);
        }
    }
}