using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UIScripts.UIShowScript.Panel;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenUI
{
    public PartyData partyData;
    public VisualElement characterList;


    int curCharacterPanelListIndex;
    VisualElement rootVisualElement;

    private static ScreenUI singleton;

    public static ScreenUI Singleton 
    {
        get
        {
            if (singleton == null)
            {
                singleton = new ScreenUI();
            }
            return singleton;
        }
    }

    public void InitStart(VisualElement root,PartyData data)
    {
        rootVisualElement = root;
        partyData = data;

        SetStatsPanel(partyData.CharacterDataList[0]);
        SetCharacterListPanel(partyData.CharacterDataList);
    }

    void SetCharacterListPanel(List<CharacterData>characterDataList) 
    {
        characterList = rootVisualElement.Q(name: "CharacterListPanel");

        characterList.Clear();
        for (int i = 0; i < characterDataList.Count; i++)
        {
            var CharacterPanel = new CharacterListPanel(characterDataList[i]);
            //CharacterPanel.style.flexGrow = 1;
            CharacterPanel.style.flexBasis = Length.Percent(25.0f);
            characterList.Add(CharacterPanel);
        }               
    }

    public void SetStatsPanel(CharacterData cData) 
    {
        var CharacterStatsPanel = rootVisualElement.Q(name: "StatsPanelBack");
        CharacterStatsPanel.Clear();
        CharacterStatsPanel.style.flexBasis = Length.Percent(30.0f);
        CharacterStatsPanel.style.flexDirection = FlexDirection.Column;
        CharacterStatsPanel.style.backgroundColor = Color.white;
        var detailsPanel = new CharacterStatsPanel(cData);

        //这个才是生成出来的Panel，要将它的缩放调整为1
        detailsPanel.style.flexGrow = 1;
        CharacterStatsPanel.Add(detailsPanel);
    }

    public void SetCharacterBorder(int index)
    {
        //Debug.Log(curCharacterPanelListIndex);
        if (curCharacterPanelListIndex == -1)
        {
            curCharacterPanelListIndex = index;
            //Debug.Log("First Touch" + " CurSelect Index is: " + curCharacterPanelListIndex);
        }
        if (curCharacterPanelListIndex != index)
        {
            ClearCharacterBorder(curCharacterPanelListIndex);
            //Debug.Log("ClearBorder,CurrentSelectBorder is :" + index + " ForBorder is :" + curCharacterPanelListIndex);
            curCharacterPanelListIndex = index;
        }

        characterList[index].style.borderBottomWidth = 5;
        characterList[index].style.borderTopWidth = 5;
        characterList[index].style.borderRightWidth = 5;
        characterList[index].style.borderLeftWidth = 5;
        characterList[index].style.borderBottomColor = Color.red;
        characterList[index].style.borderTopColor = Color.red;
        characterList[index].style.borderRightColor = Color.red;
        characterList[index].style.borderLeftColor = Color.red;
    }

    public void ClearCharacterBorder(int index)
    {
        characterList[index].style.borderBottomWidth = 0;
        characterList[index].style.borderTopWidth = 0;
        characterList[index].style.borderRightWidth = 0;
        characterList[index].style.borderLeftWidth = 0;
        characterList[index].style.borderBottomColor = Color.clear;
        characterList[index].style.borderTopColor = Color.clear;
        characterList[index].style.borderRightColor = Color.clear;
        characterList[index].style.borderLeftColor = Color.clear;
    }
}
