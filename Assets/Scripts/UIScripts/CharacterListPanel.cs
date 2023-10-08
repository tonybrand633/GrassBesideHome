using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UIScripts.UIShowScript.Panel;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIScripts.UIShowScript.Panel 
{
    public class CharacterListPanel : VisualElement
    {
        //����Resource.Load
        readonly TemplateContainer templateContainer;
        readonly List<VisualElement> characterList;

        public new class UxmlFactory : UxmlFactory<CharacterListPanel> { }

        public CharacterListPanel()     
        {
            //Resources.Load�������VisualTreeAsset�Ķ���
            //Instantiate���ɵ���TemplateContainer�Ķ���
            templateContainer = Resources.Load<VisualTreeAsset>(path:"CharacterSingle").Instantiate();
            templateContainer.style.flexGrow = 1;
            templateContainer.Q("Character").style.flexGrow = 1;
            //ʵ�������-��һ����ʵ�����������ӵ�UIBuilder����ȥ,������UIBuilder��չʾ����
            hierarchy.Add(templateContainer);


            Clickable onLeftMouseClick = new Clickable(LeftMouseClicked);
            onLeftMouseClick.activators.Clear();
            onLeftMouseClick.activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
            templateContainer.AddManipulator(onLeftMouseClick);

            ////���UI�Ĳ�����-��Ϊ���CharacterListPanel��嵥����Ӳ�����
            //ExampleClick exp = new ExampleClick();
            //templateContainer.AddManipulator(exp);

            Clickable onRightMouseClick = new Clickable(RightMouseClicked);
            onRightMouseClick.activators.Clear();
            onRightMouseClick.activators.Add(new ManipulatorActivationFilter() { button = MouseButton.RightMouse});
            templateContainer.AddManipulator(onRightMouseClick);

        }

        private void RightMouseClicked(EventBase obj)
        {
            VisualElement e = (VisualElement)obj.currentTarget;
            //string clickInfo = obj.currentTarget.ToString();
            float y = e.worldTransform.GetPosition().y / 200;
            int yAxis = Convert.ToInt32(y);
            ScreenUI.Singleton.partyData.CharacterDataList[yAxis].CharacterLevel++;
            UpdateCharacter(ScreenUI.Singleton.partyData.CharacterDataList[yAxis]);
            ScreenUI.Singleton.SetStatsPanel(ScreenUI.Singleton.partyData.CharacterDataList[yAxis]);
        }

        //ʵ��ʹ��leftMouseClick
        private void LeftMouseClicked(EventBase obj)
        {
            VisualElement e = (VisualElement)obj.currentTarget;
            float y = e.worldTransform.GetPosition().y/200;
            int yAxis = Convert.ToInt32(y);
            ScreenUI.Singleton.SetStatsPanel(ScreenUI.Singleton.partyData.CharacterDataList[yAxis]);
        }

        //�ٴ���һ���������Ĺ��캯��ʵ�����ݰ�
        public CharacterListPanel(CharacterData characterData) : this() 
        {
            userData = characterData;
            UpdateCharacter(characterData);                                          
        }


        public void UpdateCharacter(CharacterData data) 
        {
            SetSingleCharacter(templateContainer,data);
        }

        public void SetSingleCharacter(TemplateContainer character,CharacterData data) 
        {
            var headicon = character.Q("Character").Q("image");
            headicon.style.backgroundImage = data.CharacterAvatarImage;
            headicon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            var name = character.Q("Character").Q<Label>("name");
            var level = character.Q("Character").Q<Label>("level");            
            name.text = data.CharacterName;
            level.text = "�ȼ�: "+data.CharacterLevel.ToString();
        }
    }
}

