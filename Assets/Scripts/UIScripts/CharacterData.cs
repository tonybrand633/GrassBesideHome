using UnityEngine;


[CreateAssetMenu(menuName = ("Data/CharacterData"),fileName = ("CharacterData_"))]

public class CharacterData : ScriptableObject
{
    [SerializeField] Texture2D characterAvatarImage;
    [SerializeField] string characterName;
    [SerializeField] CharacterStats characterStats;
    [SerializeField] int characterLevel;

    public Texture2D CharacterAvatarImage => characterAvatarImage;

    public string CharacterName => characterName;

    public int CharacterLevel => characterLevel;   

    public CharacterStats CharacterStats => characterStats;



}