using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = ("PartyData"),fileName = ("PartyData_"))]

public class PartyData : ScriptableObject
{
    [SerializeField]
    List<CharacterData> characterDataList = new List<CharacterData>();

    public List<CharacterData> CharacterDataList 
    {
        get { return characterDataList; }
        set 
        {
            characterDataList = value;
        }
    }
}