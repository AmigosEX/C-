using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Змейка
{
    public class Moving
    {
        public static void Move(object obj)
        {
            if (Program.walls.IsHit(Program.snake.GetHead()) || Program.snake.IsHit(Program.snake.GetHead())) //Касание (стены, себя)
            {
                Program.time.Change(0, Timeout.Infinite);
            }
            else if (Program.snake.Eat(Program.food.food)) //Иначе касание (еда)
            {
                Program.food.CreateFood(); //Создание новой еды
            }
            else
            {
                Program.snake.Move();
            }
        }
    }
    struct Point //Задавание точек для стен
    {
        public int x { get; set; }
        public int y { get; set; }
        public char ch { get; set; }

        public static implicit operator Point((int, int, char) value) =>
              new Point { x = value.Item1, y = value.Item2, ch = value.Item3 };

        public static bool operator ==(Point a, Point b) =>
                (a.x == b.x && a.y == b.y) ? true : false;
        public static bool operator !=(Point a, Point b) =>
                (a.x != b.x || a.y != b.y) ? true : false;

        public void Draw()
        {
            DrawPoint(ch);
        }
        public void Clear()
        {
            DrawPoint(' ');
        }

        private void DrawPoint(char _ch)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(_ch);
        }
    }
    class Walls //Рисование стен
    {
        private char ch;
        private List<Point> wall = new List<Point>();

        public Walls(int x, int y, char ch)
        {
            this.ch = ch;

            DrawHorizontal(x, 0);
            DrawHorizontal(x, y);
            DrawVertical(0, y);
            DrawVertical(x, y);
        }
        private void DrawHorizontal(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                Point p = (i, y, ch);
                p.Draw();
                wall.Add(p);
            }
        }
        private void DrawVertical(int x, int y)
        {
            for (int i = 0; i < y; i++)
            {
                Point p = (x, i, ch);
                p.Draw();
                wall.Add(p);
            }
        }
        public bool IsHit(Point p) //Условие  поедания
        {
            foreach (var w in wall)
            {
                if (p == w)
                {
                    return true;
                }
            }
            return false;
        }
    }
    enum Direction //Объявление перечисления
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    class Snake
    {
        private List<Point> snake;

        private Direction direction;
        private int step = 1;
        private Point tail;
        private Point head;

        bool rotate = true;

        public Snake(int x, int y, int length) //Внешний вид змейки
        {
            direction = Direction.RIGHT;

            snake = new List<Point>();
            for (int i = x - length; i < x; i++)
            {
                Point p = (i, y, '#');
                Point p2 = (i, y, '8');
                snake.Add(p);

                p.Draw();
            }
        }
        public Point GetHead() => snake.Last();
        public void Move()
        {
            head = GetNextPoint();
            snake.Add(head);

            tail = snake.First();
            snake.Remove(tail);

            tail.Clear();
            head.Draw();

            rotate = true;
        }
        public bool Eat(Point p)
        {
            head = GetNextPoint();
            if (head == p)
            {
                snake.Add(head);
                head.Draw();
                return true;
            }
            return false;
        }
        public Point GetNextPoint()
        {
            Point p = GetHead();

            switch (direction) //Управление
            {
                case Direction.LEFT:
                    p.x -= step;
                    break;
                case Direction.RIGHT:
                    p.x += step;
                    break;
                case Direction.UP:
                    p.y -= step;
                    break;
                case Direction.DOWN:
                    p.y += step;
                    break;
            }
            return p;
        }
        public void Rotation(ConsoleKey key)
        {
            if (rotate)
            {
                switch (direction)
                {
                    case Direction.LEFT:
                    case Direction.RIGHT:
                        if (key == ConsoleKey.DownArrow)
                            direction = Direction.DOWN;
                        else if (key == ConsoleKey.UpArrow)
                            direction = Direction.UP;
                        break;
                    case Direction.UP:
                    case Direction.DOWN:
                        if (key == ConsoleKey.LeftArrow)
                            direction = Direction.LEFT;
                        else if (key == ConsoleKey.RightArrow)
                            direction = Direction.RIGHT;
                        break;
                }
                rotate = false;
            }
        }
        public bool IsHit(Point p)
        {
            for (int i = snake.Count - 2; i > 0; i--)
            {
                if (snake[i] == p)
                {
                    return true;
                }
            }
            return false;
        }
    }
    class Food
    {
        int x;
        int y;
        char ch;
        public Point food { get; private set; }

        Random random = new Random();

        public Food(int x, int y, char ch) //Рандомайзер спавна еды
        {
            this.x = x;
            this.y = y;
            this.ch = ch;
        }
        public void CreateFood()
        {
            food = (random.Next(2, x - 2), random.Next(2, y - 2), ch);
            food.Draw();
        }
    }
}