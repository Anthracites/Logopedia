namespace Logopedia
{
    public class EventsLibrary
    {
        public static string ShowPopUp = "ShowPopUp"; //Запрос на показ попапа, данные для показа в менеждере

        public static string GoToNewStoryCreation = "GoToNewStoryCreation"; // Переход к созданию нового сюжета
        public static string CreateNewScene = "CreateNewScene"; // Создание новой сцены
        public static string RemoveCurrentScene = "RemoveCurrentScene"; //Удалить текущую сцену и перейти к предыдущей (нельзя удалять единственную сцену, предложить очистить текущую)
        public static string GoToEditStory = "GoToEditStory"; // Переход к  редактированию старого сюжета
        public static string GoToNewGame = "GoToNewGame"; //Переход к новой игре
        public static string GoToContinueGame = "GoToContinueGame"; // Переход к продолжению игры
        public static string GoToMenu = "GoToMenu"; // Переход/возвращение в меню
        public static string GoToSettings = "GoToSettings"; // Переход в настройки

        public static string StoryConvertedForSave = "StoryConvertedForSave"; // Конвертация объектов в сцену
        public static string SaveStory = "SaveStory"; // Сохранить сюжет

        public static string SceneSwiched = "SceneSwiched"; // Переключение сцены
        public static string CharacterSwiched = "CharacterSwiched"; // Переключение героя
        public static string ItemInSlot = "ItemInSlot"; // Предмет положили в слот
        public static string LevelComplete = "LevelComplete"; //Уровень пройден

        public static string TopicSwiched = "TopicSwiched"; // Тема переключена
        public static string TopicSwich = "TopicSwich"; // Переключение сцены

        public static string ItemCreated = "ItemCreated"; // Предмет одежды создан
        public static string BGSpriteChanged = "BGSpriteChanged"; // Изменен фон сцены
        public static string CharacterSpriteChanged = "CharacterSpriteChanged"; // Изменен персонаж
        public static string AnimationChanged = "AnimationChanged"; // Изменена анимация
        public static string PreviewScene = "PreviewScene"; // Скрывает footer и headr на cretion view
        public static string BackFromPreviewScene = "BackFromPreviewScene"; // Отображает footer и headr на cretion view

        //UI to items
        public static string ItemSelected = "ItemSelected"; //Предмет выбран
        public static string DeleteItem = "DeleteItem";
        public static string MirrorItem = "MirrorItem";
        public static string SetShadowOnItem = "SetShadowOnItem";
        public static string ResetRotation = "ResetRotation";
        public static string SwichItemShadow = "SwichItemShadow";
        public static string SwichItemShadowVisible = "SwichItemShadowVisible";
        public static string SearchShadow = "SearchShadow";
        public static string CharacterSelected = "CharacterSelected";
        public static string RotateSelectedItem = "RotateSelectedItem";
        public static string ScaleSelectedItem = "ScaleSelectedItem";
        public static string CopySelectedItem = "CopySelectedItem";
        public static string SendItemsToManager = "SendItemsToManager";




        //Sounds
        public static string CorrectAnswerSoundChanged = "CorrectAnswerSoundChanged";
        public static string BGMusicSoundChanged = "BGMusicSoundChanged";
        public static string GoNextSceneSoundChanged = "GoNextSceneSoundChanged";
        public static string TakeItemSoundChanged = "TakeItemSoundChanged";


        public static string ClipsAddedToFolder = "ClipsAddedToFolder"; // Звуки загружены в папку звуков
        public static string ClipsAddedToGame = "ClipsAddedToGame"; //Звуки загружены в игру
    }
}
