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
        public bool IsActiveNoButton;
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
            IsActiveNoButton = false,
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
            IsActiveNoButton = false,
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
            IsActiveNoButton = false,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = true,
            CloseAnywareClick = false
        };

        public static PopUpConfig СonfirmCleanScene = new PopUpConfig()
        {
            PopUpName = "СonfirmClearScene",
            IconWay = "Sprites/UI/broom",
            Title = "Очистить текущую сцену?",
            IsActiveNoButton = false,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = true
        };

        public static PopUpConfig СonfirmRemoveScene = new PopUpConfig()
        {
            PopUpName = "СonfirmRemoveScene",
            IconWay = "Sprites/UI/delete",
            Title = "Удалить текущую сцену?",
            IsActiveNoButton = false,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = true
        };


        public static PopUpConfig StorySaved = new PopUpConfig()
        {
            PopUpName = "StorySaved",
            IconWay = "Sprites/UI/Save",
            Title = "Сюжет сохранён",
            IsActiveNoButton = false,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = false,
            IsActiveDropDown = false,
            CloseAnywareClick = true
        };

        public static PopUpConfig СonfirmSaveStory = new PopUpConfig()
        {
            PopUpName = "СonfirmSaveStory",
            IconWay = "Sprites/UI/Save",
            Title = "Сохранить сюжет?",
            IsActiveNoButton = true,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = false
        };

        public static PopUpConfig NoSpritesNotification = new PopUpConfig()
        {
            PopUpName = "NoSpritesNotification",
            IconWay = "Sprites/UI/Delete",
            Title = "Нужно загрузить изоюражения",
            IsActiveNoButton = false,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = false
        };

        public static PopUpConfig ConfirmExitToMenuFromGame = new PopUpConfig()
        {
            PopUpName = "ConfirmExitToMenuFromGame",
            IconWay = "Sprites/UI/Home",
            Title = "Выйти в меню?",
            IsActiveNoButton = false,
            IsActiveCloseButton = true,
            IsActiveInputField = false,
            IsActiveCancelButton = true,
            IsActiveDropDown = false,
            CloseAnywareClick = true
        };
    }
}