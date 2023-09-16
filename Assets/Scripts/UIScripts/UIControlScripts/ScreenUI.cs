using System.Collections;
using System.Collections.Generic;
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
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        SetStatsPanel(partyData.CharacterDataList[0]);

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStatsPanel(CharacterData cData) 
    {

        var CharacterStatsPanel = rootVisualElement.Q(name: "StatsPanelBack");
        CharacterStatsPanel.Clear();
        CharacterStatsPanel.style.flexBasis = Length.Percent(30.0f);
        CharacterStatsPanel.style.flexDirection = FlexDirection.Column;
        CharacterStatsPanel.style.backgroundColor = Color.white;
        var detailsPanel = new CharacterPanel(cData);

        //这个才是生成出来的Panel，要将它的缩放调整为1
        detailsPanel.style.flexGrow = 1;
        CharacterStatsPanel.Add(detailsPanel);
    }
}
