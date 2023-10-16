using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CatCSVReader
{
    public static string stageDataFileName = "ExcelCSVFolder/CatStage";
    public static string characterDataFileName = "ExcelCSVFolder/CatCharacter";

    private static Dictionary<CatStageEnum, List<float>> catStageDic;
    private static Dictionary<CatCharacterEnum, List<float>> catCharacterDic;

    private static bool isLoaded = false; 


    // Start is called before the first frame update


    public static void LoadData() 
    {
        if (isLoaded) 
        {
            return;
        }
        catStageDic = new Dictionary<CatStageEnum, List<float>>();
        catCharacterDic = new Dictionary<CatCharacterEnum, List<float>>();
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
                List<float> stateData = new List<float>();

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
        //Debug.Log(c_rows.Length);
        for (int i = 1; i < c_rows.Length; i++)  // Skip the header row
        {
            string[] c_row = c_rows[i].Split(',');
            
            string key = c_row[0];
            //Debug.Log(key + ":" + " Have " + c_row.Length + " Elements is" + c_row.ToString());
            

            CatCharacterEnum catEnum;
            if (Enum.TryParse<CatCharacterEnum>(c_row[0], true, out catEnum))
            {
                List<float> catCharacterData = new List<float>();
                //Debug.Log("c_row: "+c_row.Length);
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

        //遍历一下字典
        for (int i = 0; i < catCharacterDic[CatCharacterEnum.Friendly].Count; i++)
        {
            Debug.Log("Friendly" + catCharacterDic[CatCharacterEnum.Friendly][i]);
        }
        for (int i = 0; i < catCharacterDic[CatCharacterEnum.Timid].Count; i++)
        {
            Debug.Log("Timid" + catCharacterDic[CatCharacterEnum.Timid][i]);
        }
        for (int i = 0; i < catCharacterDic[CatCharacterEnum.Cunning].Count; i++)
        {
            Debug.Log("Cunning" + catCharacterDic[CatCharacterEnum.Cunning][i]);
        }
        for (int i = 0; i < catCharacterDic[CatCharacterEnum.Lazy].Count; i++)
        {
            Debug.Log("Lazy" + catCharacterDic[CatCharacterEnum.Lazy][i]);
        }
        for (int i = 0; i < catCharacterDic[CatCharacterEnum.King].Count; i++)
        {
            Debug.Log("King" + catCharacterDic[CatCharacterEnum.King][i]);
        }
    }

    public static List<float> GetStageDataList(CatStageEnum key)
    {
        if (catStageDic.ContainsKey(key))
            return catStageDic[key];
        else
            return null;
    }

    public static float GetStageSpeed(CatStageEnum key) 
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

    public static float GetStageDetectRadius(CatStageEnum key)
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
    public static float GetStageSize(CatStageEnum key)
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

    public static List<float> GetCharacterList(CatCharacterEnum key) 
    {
        if (catCharacterDic.ContainsKey(key))
            return catCharacterDic[key];
        else
            return null;
    }

    public static float GetBaseSpeed(CatCharacterEnum key)
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
    public static float GetBaseDetectSize(CatCharacterEnum key)
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

    public static float GetBaseSize(CatCharacterEnum key) 
    {
        if (catCharacterDic.ContainsKey(key))
        {
            try
            {
                return catCharacterDic[key][2];
            }
            catch (Exception e) 
            {
                foreach (var item in catCharacterDic[key])
                {
                    //Debug.Log(item);
                }
                //Debug.LogError(catCharacterDic[key].Count);
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }
}
