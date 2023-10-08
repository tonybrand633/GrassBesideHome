using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace UIScripts.UIShowScript.Panel 
{
    public class CharacterStatsPanel : VisualElement
    {
        readonly TemplateContainer templateContainer;
        readonly List<VisualElement> statsContainer;

        string[] statsName = new string[] { "����", "����", "����", "����", "ͯ��" };

        public new class UxmlFactory : UxmlFactory<CharacterStatsPanel> { }

        public CharacterStatsPanel()
        {
            templateContainer = Resources.Load<VisualTreeAsset>(path: "StatsPanel").Instantiate();
            templateContainer.style.flexGrow = 1;
            //ʵ�������
            hierarchy.Add(templateContainer);
        }

        //����������԰󶨵���ť���棬ÿ�ε��ɾ��ԭ�е�CharacterPanel,�����µ�Panel
        public CharacterStatsPanel(CharacterData characterData) : this()
        {
            userData = characterData;
            statsContainer = templateContainer.Query(name = "CharacterStats").ToList();
            
            UpdateCharacterStats();
        }

        void UpdateCharacterStats() 
        {
            var characterDATA = userData as CharacterData;

            SetCharacterStats(statsName[0], characterDATA.CharacterStats.Strength.ToString(), statsContainer[0]);
            SetCharacterStats(statsName[1], characterDATA.CharacterStats.Endurance.ToString(), statsContainer[1]);
            SetCharacterStats(statsName[2], characterDATA.CharacterStats.Agile.ToString(), statsContainer[2]);
            SetCharacterStats(statsName[3], characterDATA.CharacterStats.Intelligence.ToString(), statsContainer[3]);
            SetCharacterStats(statsName[4], characterDATA.CharacterStats.Innocence.ToString(), statsContainer[4]);
        }

        void SetCharacterStats(string label,string value,VisualElement container)
        {
            Label l = container.Q<Label>("Label");
            Label v = container.Q<Label>("Value");
            l.style.fontSize = 30;
            //l.style.unityFontStyleAndWeight = FontStyle.Bold;
            v.style.fontSize = 30;  
            l.text = label;            
            v.text = value;            
            container.style.flexBasis = Length.Percent(25.0f);
        }
    }
}



