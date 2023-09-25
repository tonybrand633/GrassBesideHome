using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UIScripts.UIShowScript.Panel;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenUI : MonoBehaviour
{
    [SerializeField]
    PartyData partyData;

    VisualElement rootVisualElement;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            int temp = partyData.CharacterDataList[0].CharacterLevel;
            temp++;
            partyData.CharacterDataList[0].CharacterLevel = temp;
        }
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        SetStatsPanel(partyData.CharacterDataList[0]);
        SetCharacterListPanel(partyData.CharacterDataList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCharacterListPanel(List<CharacterData>characterDataList) 
    {
        var CharacterList = rootVisualElement.Q(name: "CharacterListPanel");

        CharacterList.Clear();
        for (int i = 0; i < characterDataList.Count; i++)
        {
            var CharacterPanel = new CharacterListPanel(characterDataList[i]);
            //CharacterPanel.style.flexGrow = 1;
            CharacterPanel.style.flexBasis = Length.Percent(25.0f);
            CharacterList.Add(CharacterPanel);
        }               
    }

    void SetStatsPanel(CharacterData cData) 
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
}
