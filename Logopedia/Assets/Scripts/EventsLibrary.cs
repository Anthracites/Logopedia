namespace Logopedia
{
    public class EventsLibrary
    {
        public static string ShowPopUp = "ShowPopUp"; //Запрос на показ попапа, данные для показа в менеждере
        public static string GoToNewStoryCreation = "GoToNewStoryCreation"; // Переход к созданию нового сюжета
        public static string CreateNewScene = "CreateNewScene"; // Создание новой сцены
        public static string RemoveCurrentCsene = "RemoveCurrentCsene"; //Удалить текущую сцену и перейти к предыдущей (нельзя удалять единственную сцену, предложить очистить текущую)
        public static string GoToEditStory = "GoToEditStory"; // Переход к  редактированию старого сюжета
        public static string GoToNewGame = "GoToNewGame"; //Переход к новой игре
        public static string GoToContinueGame = "GoToContinueGame"; // Переход к продолжению игры
        public static string GoToMenu = "GoToMenu"; // Переход/возвращение в меню
        public static string SaveStory = "SaveStory"; //Сохранить сюжет

        public static string ItemCreated = "ItemCreated"; // Предмет одежды создан
        public static string BGSpriteChanged = "BGSpriteChanged"; // Изменен фон сцены
        public static string CharacterSpriteChanged = "CharacterSpriteChanged"; // Изменен персонаж
        public static string ItemSelected = "ItemSelected"; //Предмет выбран
    }
}
