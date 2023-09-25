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

