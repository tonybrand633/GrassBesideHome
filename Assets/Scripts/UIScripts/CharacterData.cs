using UnityEngine;


[CreateAssetMenu(menuName = ("Data/CharacterData"),fileName = ("CharacterData_"))]

public class CharacterData : ScriptableObject
{
    [SerializeField] Texture2D characterAvatarImage;
    [SerializeField] string characterName;
    [SerializeField] CharacterStats characterStats;
    [SerializeField,Range(1,MaxCharacterLevel)] int characterStartLevel = 1;

    int characterLevel;
    const int MaxCharacterLevel = 10;

    public Texture2D CharacterAvatarImage => characterAvatarImage;

    public string CharacterName => characterName;

    public int CharacterLevel 
    {
        get { return characterLevel; }
        set 
        {
            if (value > MaxCharacterLevel || value < 1) return;
            characterLevel = value;
        }
    }

    public CharacterStats CharacterStats => characterStats;



}