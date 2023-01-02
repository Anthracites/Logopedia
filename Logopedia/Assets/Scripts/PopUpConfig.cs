using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logopedia.UserInterface
{
    public struct PopUpConfig
    {
        public string PopUpName;
        public string IconWay;
        public string Title;
        public bool IsActiveCloseButton;
        public bool IsActiveInputField;
        public bool IsActiveCancelButton;
        public bool IsActiveDropDown;
        public bool CloseAnywareClick;
    }


    public class PopUpConfigLibrary
    {
        public static PopUpConfig NewStory = new PopUpConfig()
        {
            PopUpName = "NewStory",
            IconWay = "Sprites/UI/AddNew",
            Title = "Создать новый сюжет. Введите имя нового сюжета...",
            IsActiveCloseButton = true,
            IsActiveInputField = true,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = false
        };

        public static PopUpConfig PlayGame = new PopUpConfig()
        {
            PopUpName = "PlayNewGame",
            IconWay = "Sprites/UI/play",
            Title = "Выберите сюжет:",
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = true,
            CloseAnywareClick = false
        };

        public static PopUpConfig EditStory = new PopUpConfig()
        {
            PopUpName = "EditStory",
            IconWay = "Sprites/UI/edit",
            Title = "Выберите сюжет:",
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = true,
            CloseAnywareClick = false
        };

        public static PopUpConfig СonfirmRemove = new PopUpConfig()
        {
            PopUpName = "СonfirmRemove",
            IconWay = "Sprites/UI/broom",
            Title = "Очистить текущую сцену?",
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = true
        };
    }
}