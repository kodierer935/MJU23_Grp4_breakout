﻿namespace BrakeOut1
{

    using System;
    using System.Threading;

    public class Ball
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ChangeX { get; set; }
        public int ChangeY { get; set; }

        public Ball(int x, int y, int changeX, int changeY)
        {
            X = x;
            Y = y;
            ChangeX = changeX;
            ChangeY = changeY;
        }
    }

    public class Paddle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Length { get; set; }

        public Paddle(int x, int y, int length)
        {
            X = x;
            Y = y;
            Length = length;
        }
    }

    public class Brick
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsVisible { get; set; }

        public Brick(int x, int y)
        {
            X = x;
            Y = y;
            IsVisible = true;
        }
    }

    internal class Program
    {
        protected static int origRow;
        protected static int origCol;

        static void Main(string[] args)
        {
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;
            int poäng = 0;
            int brickIndex = 0;

            Ball ball = new Ball(Console.WindowWidth / 2, Console.WindowHeight - 2, 1, -1);
            Paddle paddle = new Paddle(Console.WindowWidth / 2 - 3, Console.WindowHeight - 1, 6);

            bool IsPlaying = false;
            bool IsPaused = false;
            bool restartGame = false;

            string command = "m";
            int brickRows = 8;
            int brickCols = 14;
            Brick[,] bricks = new Brick[brickRows, brickCols];

            for (int i = 0; i < brickRows; i++)
            {
                for (int j = 0; j < brickCols; j++)
                {
                    bricks[i, j] = new Brick(j * 5, i * 2);
                }
            }

            while (true)
            {
                while (IsPlaying == false)
                {
                    if (command == "M" || command == "m")
                    {
                        Menu_func(out restartGame, out command);
                    }
                    else if (command == "N" || command == "n")
                    {
                        IsPlaying = true;
                        restartGame = true;
                    }
                    else if (command == "H" || command == "h")
                    {
                        command = Help_Func();
                    }
                    else if (command == "S" || command == "s")
                    {
                        Console.WriteLine("Bye Bye !!");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Fel Kommand !!!");
                        Console.Write("> ");
                        command = Console.ReadLine();
                    }
                }

                while (IsPlaying)
                {
                    if (!IsPaused)
                    {
                        Console.Clear();
                        WriteAt($"Poäng: {poäng}", 0, 20);

                        ball.X += ball.ChangeX;
                        ball.Y += ball.ChangeY;

                        // Bounce på sidoväggarna
                        if (ball.X <= 0 || ball.X >= Console.WindowWidth - 2)
                            ball.ChangeX *= -1;

                        // Bounce på taket
                        if (ball.Y < 0)
                            ball.ChangeY *= -1;

                        // Bounce på paddeln
                        if (ball.Y == paddle.Y - 1 && ball.X >= paddle.X && ball.X <= paddle.X + paddle.Length - 1)
                            ball.ChangeY *= -1;

                        for (int i = 0; i < brickRows; i++)
                        {
                            for (int j = 0; j < brickCols; j++)
                            {
                                Brick brick = bricks[i, j];
                                if (brick.IsVisible && ball.X >= brick.X && ball.X < brick.X + 5 && ball.Y == brick.Y)
                                {
                                    brick.IsVisible = false;
                                    ball.ChangeY *= -1;
                                    poäng = poäng + 1;
                                }
                            }
                        }

                        // Rita paddeln
                        Console.SetCursorPosition(paddle.X, paddle.Y);
                        Console.Write(new string('-', paddle.Length));

                        // Rita bollen
                        if (Console.WindowHeight >= ball.Y && Console.WindowWidth >= ball.X)
                        {
                            int clampedX = Math.Clamp(ball.X, 0, Console.WindowWidth - 1);
                            int clampedY = Math.Clamp(ball.Y, 0, Console.WindowHeight - 1);

                            Console.SetCursorPosition(clampedX, clampedY);
                            Console.Write("O");
                        }

                        // Rita brickorna
                        foreach (var brick in bricks)
                        {
                            if (brick.IsVisible)
                            {
                                Console.SetCursorPosition(brick.X, brick.Y);
                                Console.Write("[___]");
                            }
                        }
                    }

                    // Rörelse av paddeln
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.LeftArrow && paddle.X > 0)
                        {
                            paddle.X -= 3;
                        }
                        else if (key.Key == ConsoleKey.RightArrow && paddle.X < Console.WindowWidth - paddle.Length)
                        {
                            paddle.X += 3;
                        }
                        else if (key.Key == ConsoleKey.Spacebar)
                        {
                            IsPaused = !IsPaused;
                            Console.Clear();
                            if (IsPaused)
                            {
                                Console.WriteLine("Spel pausat. tryck 'Spacebar' för att försätta.\ntryck 'm' för att gå till meny.\ntryck 's' för att avsluta spelet.");
                            }
                        }
                    }

                    Thread.Sleep(60);
                }

                Restart(ref ball, ref restartGame, brickRows, brickCols, ref bricks);
            }
        }

        static void Restart(ref Ball ball, ref bool restartGame, int brickRows, int brickCols, ref Brick[,] bricks)
        {
            if (restartGame)
            {
                ball = new Ball(Console.WindowWidth / 2, Console.WindowHeight - 2, 1, -1);
                bricks = new Brick[brickRows, brickCols];
                for (int i = 0; i < brickRows; i++)
                {
                    for (int j = 0; j < brickCols; j++)
                    {
                        bricks[i, j] = new Brick(j * 5, i * 2);
                    }
                }

                restartGame = false;
            }
        }

        static void Menu_func(out bool restartGame, out string command)
        {
            Console.Clear();
            Meny();
            Console.Write("\n> ");
            command = Console.ReadLine();
            restartGame = false;
        }

        static string Help_Func()
        {
            string command;
            Console.Clear();
            Hjälp();
            Console.Write("> ");
            command = Console.ReadLine();
            return command;
        }

        public static void Meny()
        {
            Console.WriteLine("Välkommen till Breakout spel, Skriv bokstav i hakparenteser för att välja !!!\n\n");

            Console.WriteLine("[N]y spel");
            Console.WriteLine("[H]jälp");
            Console.WriteLine("[S]luta");
        }

        public static void Hjälp()
        {
            Console.WriteLine("Här hittar du spelinstruktionerna!\n");

            Console.WriteLine("Använd pilarna för att styra padel.\n");
            Console.WriteLine("tryck 'Spacebar' för att pausa spelet\n");

            Console.WriteLine("Du måste få bollen att träffa brickan. När bollen träffar brickan försvinner brickan.\nDin uppgift är att få alla brickor på skärmen att försvinna.\n");
            Console.WriteLine("När du är klar första gången bör du göra samma sak igen. Efter att ha förstört brickorna andra gången vinner du.\n");
            Console.WriteLine("Bollen får inte falla till botten. Om det går ner kommer du att förlora ett av dina tre försök.\n ");
            Console.WriteLine("När antal försök når noll kommer du att förlora spelet. du får starta om spelet.\n\n");

            Console.WriteLine("skriv [m] om du vill gå tillbaka till meny, eller [s] om vill avsluta application");
        }

        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}