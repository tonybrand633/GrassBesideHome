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
        //用于Resource.Load
        readonly TemplateContainer templateContainer;
        readonly List<VisualElement> characterList;

        public new class UxmlFactory : UxmlFactory<CharacterListPanel> { }

        public CharacterListPanel()     
        {
            //Resources.Load载入的是VisualTreeAsset的对象
            //Instantiate生成的是TemplateContainer的对象
            templateContainer = Resources.Load<VisualTreeAsset>(path:"CharacterSingle").Instantiate();
            templateContainer.style.flexGrow = 1;
            templateContainer.Q("Character").style.flexGrow = 1;
            //实例化添加-这一步的实例化添加是添加到UIBuilder里面去,让他在UIBuilder里展示出来
            hierarchy.Add(templateContainer);


            Clickable onLeftMouseClick = new Clickable(LeftMouseClicked);
            onLeftMouseClick.activators.Clear();
            onLeftMouseClick.activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
            templateContainer.AddManipulator(onLeftMouseClick);

            ////添加UI的操作器-是为这个CharacterListPanel面板单独添加操作器
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

        //实际使用leftMouseClick
        private void LeftMouseClicked(EventBase obj)
        {
            VisualElement e = (VisualElement)obj.currentTarget;
            float y = e.worldTransform.GetPosition().y/200;
            int yAxis = Convert.ToInt32(y);
            ScreenUI.Singleton.SetStatsPanel(ScreenUI.Singleton.partyData.CharacterDataList[yAxis]);
        }

        //再创建一个带参数的构造函数实现数据绑定
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
            level.text = "等级: "+data.CharacterLevel.ToString();
        }
    }
}

