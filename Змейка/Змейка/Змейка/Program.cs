using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Змейка
{
    class Program
    {
        public static Walls walls;  //Объявление классов
        public static Snake snake;
        public static Food food;
        public static Timer time;

        static void Main()
        {
            Console.CursorVisible = false;

            int x = 0;
            int y = 0;
            int s = 0;
            Console.WriteLine("Сложность:" +  //Выбор сложности
                "\n1 - Легко" +
                "\n2 - Средне" +
                "\n3 - Сложно");
            int D = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            switch (D) //Данные для сложностей
            {
                case 1:
                    x = 10;
                    y = 10;
                    s = 500;
                    break;
                case 2:
                    x = 15;
                    y = 15;
                    s = 200;
                    break;
                case 3:
                    x = 20;
                    y = 20;
                    s = 100;
                    break;
            }

            walls = new Walls(x, y, '*');     //Стены с зависимостью от сложности
            snake = new Snake(x / 2, y / 2, 3); // Спавн  змейки по координатам

            food = new Food(x, y, 'O');  //Спавн еды по координатам
            food.CreateFood();

            time = new Timer(Moving.Move, null, 0, s);  //Скорость движения с зависимостью от сложности

            while (true)  //Продолжение движения с зависимостью к последней нажатой клавише
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    snake.Rotation(key.Key);
                }
            }
        }
    }
}