using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CatCSVReader
{
    public static string stageDataFileName = "ExcelCSVFolder/CatStage";
    public static string characterDataFileName = "ExcelCSVFolder/CatCharacter";

    private static Dictionary<CatStageEnum, List<int>> catStageDic;
    private static Dictionary<CatCharacterEnum,List<int>> catCharacterDic = new Dictionary<CatCharacterEnum, List<int>>();

    private static bool isLoaded = false; 


    // Start is called before the first frame update


    public static void LoadData() 
    {
        if (isLoaded) 
        {
            return;
        }
        catStageDic = new Dictionary<CatStageEnum, List<int>>();
        catCharacterDic = new Dictionary<CatCharacterEnum, List<int>>();
        TextAsset stageData = Resources.Load<TextAsset>(stageDataFileName);
        TextAsset characterData = Resources.Load<TextAsset>(characterDataFileName);

        if (stageData == null|| characterData == null)
        {
            Debug.LogError("CatData file not found!");
            return;
        }

        string[] s_rows = stageData.text.Split('\n');

        for (int i = 1; i < s_rows.Length; i++)  // Skip the header row
        {
            string[] s_row = s_rows[i].Split(',');
            string key = s_row[0];
            List<int> values = new List<int>();

            CatStageEnum stateEnum;
            if (Enum.TryParse<CatStageEnum>(s_row[0], true, out stateEnum))
            {
                List<int> stateData = new List<int>();

                for (int j = 1; j < s_row.Length; j++)
                {
                    int value;
                    if (int.TryParse(s_row[j], out value))
                    {
                        stateData.Add(value);
                    }
                }

                catStageDic[stateEnum] = stateData;  // 将数据加入字典
            }
        }

        string[] c_rows = characterData.text.Split('\n');

        for (int i = 1; i < c_rows.Length; i++)  // Skip the header row
        {
            string[] c_row = c_rows[i].Split(',');
            string key = c_row[0];
            List<int> values = new List<int>();

            CatCharacterEnum catEnum;
            if (Enum.TryParse<CatCharacterEnum>(c_row[0], true, out catEnum))
            {
                List<int> catCharacterData = new List<int>();

                for (int j = 1; j < c_row.Length; j++)
                {
                    int value;
                    if (int.TryParse(c_row[j], out value))
                    {
                        catCharacterData.Add(value);
                    }
                }
                catCharacterDic[catEnum] = catCharacterData;
                //将数据加入字典
            }
        }
        isLoaded = true;
    }

    public static List<int> GetStageDataList(CatStageEnum key)
    {
        if (catStageDic.ContainsKey(key))
            return catStageDic[key];
        else
            return null;
    }

    public static int GetStageSpeed(CatStageEnum key) 
    {
        if (catStageDic.ContainsKey(key))
        {
            return catStageDic[key][0];
        }
        else 
        {
            return -1;
        }
    }

    public static int GetStageDetectRadius(CatStageEnum key)
    {
        if (catStageDic.ContainsKey(key))
        {

            return catStageDic[key][1];
        }
        else
        {
            return -1;
        }
    }
    public static int GetStageSize(CatStageEnum key)
    {
        if (catStageDic.ContainsKey(key))
        {
            return catStageDic[key][2];
        }
        else
        {
            return -1;
        }
    }

    public static List<int> GetCharacterList(CatCharacterEnum key) 
    {
        if (catCharacterDic.ContainsKey(key))
            return catCharacterDic[key];
        else
            return null;
    }

    public static int GetBaseSpeed(CatCharacterEnum key)
    {
        if (catCharacterDic == null)
        {
            return -1;
        }
        if (catCharacterDic.ContainsKey(key))
        {
            return catCharacterDic[key][0];
        }


        else
        {
            return -1;
        }
    }
    public static int GetBaseDetectSize(CatCharacterEnum key)
    {
        if (catCharacterDic.ContainsKey(key))
        {
            return catCharacterDic[key][1];
        }
        else
        {
            return -1;
        }
    }
}
