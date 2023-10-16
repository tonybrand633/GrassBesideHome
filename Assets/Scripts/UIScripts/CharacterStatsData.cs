using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = ("Data/CharacterStatsData"), fileName = ("CharacterStatsData_"))]

public class CharacterStatsData : ScriptableObject
{
    [SerializeField] TextAsset levelData;
    [SerializeField] List<CharacterStats> characterStatsList = new List<CharacterStats>();

    public CharacterStats GetCurrentLevelStats(int level) 
    {        
        return characterStatsList[level-1];
    }

    void OnValidate()
    {
        characterStatsList.Clear();
        ReadLevelStatsExcel();
    }

    void ReadLevelStatsExcel() 
    {
        string DATA = levelData.text;
        string[]res = DATA.Split("\n");
        for (int i = 1; i < res.Length-1; i++) 
        {
            //Debug.Log(res[i]+"ÕâÊÇµÚ£º"+ i.ToString());
            CharacterStats characterStats = new CharacterStats();
            
            string[] resDetail = res[i].Split(",");
            characterStats.Strength =  int.Parse(resDetail[0]);
            characterStats.Endurance = int.Parse(resDetail[1]);
            characterStats.Agile= int.Parse(resDetail[2]);
            characterStats.Intelligence = int.Parse(resDetail[3]);
            characterStats.Innocence = int.Parse(resDetail[4]);
            characterStatsList.Add(characterStats);
        }
    }
}