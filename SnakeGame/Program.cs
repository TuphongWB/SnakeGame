using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class SnakeGame
    {
        private const int Width = 60;
        private const int Height = 20;
        private static readonly char SnakeHeadChar = '\u25CF'; // Ký hiệu đầu rắn (hình tròn đen)
        private static readonly char SnakeBodyChar = '\u25A0'; // Ký hiệu cơ thể rắn (hình vuông đen)
        private static readonly char FoodChar = '*';

        private static int score;
        private static bool gameOver;

        private static int snakeHeadX;
        private static int snakeHeadY;
        private static int foodX;
        private static int foodY;
        private static int directionX;
        private static int directionY;
        private static List<int> snakeBodyX;
        private static List<int> snakeBodyY;

        public static void Main(string[] args)
        {
            Console.Clear();
            InitializeGame();
            DrawGame();
            MoveSnake();
            

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    ChangeDirection(keyInfo.Key);
                }

                MoveSnake();
                CheckCollision();
                DrawGame();

                System.Threading.Thread.Sleep(100);
            }

            Console.WriteLine("Game over! Final score: " + score);
            Console.ReadLine();
        }

        private static void InitializeGame()
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;

            int screenWidth = Width + 2;
            int screenHeight = Height + 3;

            int windowWidth = Console.LargestWindowWidth < screenWidth ? Console.LargestWindowWidth : screenWidth;
            int windowHeight = Console.LargestWindowHeight < screenHeight ? Console.LargestWindowHeight : screenHeight;

            Console.SetWindowSize(windowWidth, windowHeight);
            Console.SetBufferSize(screenWidth, screenHeight);

            gameOver = false;
            score = 0;

            snakeHeadX = screenWidth / 2;
            snakeHeadY = screenHeight / 2;
            directionX = 1;
            directionY = 0;

            snakeBodyX = new List<int>();
            snakeBodyY = new List<int>();

            foodX = new Random().Next(1, screenWidth);
            foodY = new Random().Next(1, screenHeight); 
        }

        private static void DrawGame()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ResetColor();


            // Draw snake body
            Console.SetCursorPosition(snakeHeadX, snakeHeadY);
            Console.ForegroundColor = ConsoleColor.Magenta; // Sử dụng màu đã khai báo
            Console.Write(SnakeHeadChar);

            for (int i = 0; i < snakeBodyX.Count; i++)
            {
                Console.SetCursorPosition(snakeBodyX[i], snakeBodyY[i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(SnakeBodyChar);
            }

            // Draw food
            Console.SetCursorPosition(foodX, foodY);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(FoodChar);

            // Reset console colors
            // Draw score
            Console.SetCursorPosition(0, Height + 2);
            Console.WriteLine("Score: " + score);
        }

        private static void ChangeDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (directionY != 1) // Prevent moving directly down if currently moving up
                    {
                        directionX = 0;
                        directionY = -1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (directionY != -1) // Prevent moving directly up if currently moving down
                    {
                        directionX = 0;
                        directionY = 1;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (directionX != 1) // Prevent moving directly right if currently moving left
                    {
                        directionX = -1;
                        directionY = 0;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (directionX != -1) // Prevent moving directly left if currently moving right
                    {
                        directionX = 1;
                        directionY = 0;
                    }
                    break;
            }
        }

        private static void MoveSnake()
        {
            int newHeadX = snakeHeadX + directionX;
            int newHeadY = snakeHeadY + directionY;

            snakeBodyX.Insert(0, snakeHeadX);
            snakeBodyY.Insert(0, snakeHeadY);
            snakeHeadX = newHeadX;
            snakeHeadY = newHeadY;

            if (snakeBodyX.Count > score)
            {
                snakeBodyX.RemoveAt(score);
                snakeBodyY.RemoveAt(score);
            }
        }

        private static void CheckCollision()
        {
            if (snakeHeadX <= 0 || snakeHeadX >= Width + 1 || snakeHeadY <= 0 || snakeHeadY >= Height + 1)
            {
                gameOver = true;
            }

            for (int i = 0; i < snakeBodyX.Count; i++)
            {
                if (snakeHeadX == snakeBodyX[i] && snakeHeadY == snakeBodyY[i])
                {
                    gameOver = true;
                    break;
                }
            }

            if (snakeHeadX == foodX && snakeHeadY == foodY)
            {
                score++;
                foodX = new Random().Next(1, Width);
                foodY = new Random().Next(1, Height);
            }
        }
    }
}
