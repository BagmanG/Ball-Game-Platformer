using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Localization
{
    public static class Lang
    {
        public static int CurrentLang = 0;//0 - Ru, 1 - En
        public static new Dictionary<int, string> ru = new Dictionary<int, string>()
        {
            { 0, "Играть"},
            { 1, "Как играть"},
            { 2, "Уровни"},
            { 3, "Выйти"},
            { 4, "Закрыть"},
            { 5, "Пробел — Прыгать\r\nA, D — Двигаться в стороны\r\nR — Перезапустить уровень\r\nEsc — Открыть меню паузы\r\n___________________________________________\r\n\r\nНа каждом уровне есть финиш. Тебе нужно дойти до финиша, преодолевая препятствия. Берегись лавы!"},
            { 6, "Выберите уровень"},
            { 7, "Уровень"},
            { 8, "Победа"},
            { 9, "Продолжить"},
            { 10, "Перезапустить"},
            { 11, "Меню паузы"},
        };
        public static new Dictionary<int, string> en = new Dictionary<int, string>()
        {
            { 0, "Play"},
            { 1, "How to play"},
            { 2, "Levels"},
            { 3, "Exit"},
            { 4, "Close"},
            { 5, "Space — Jump\r\nA, D — Move sideways\r\nR — Restart level\r\nEsc — Open pause menu\r\n___________________________________________\r\n\r\nEach level has a finish point. You need to reach the finish while overcoming obstacles. Beware of lava!"},
            { 6, "Select level"},
            { 7, "Level"},
            { 8, "Victory"},
            { 9, "Continue"},
            { 10, "Restart"},
            { 11, "Pause menu"},
        };
        public static String Get(int key)
        {
            return CurrentLang == 0 ? ru[key] : en[key];
        }
    }
}
