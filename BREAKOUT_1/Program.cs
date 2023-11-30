
using System;
using System.Threading.Channels;
namespace BREAKOUT_1
{
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
            int Lives = 3;
            int current_level = 1;

            Ball ball = new Ball(Console.WindowWidth / 2, Console.WindowHeight - 2, 1, -1);
            Paddle paddle = new Paddle(Console.WindowWidth / 2 - 3, Console.WindowHeight - 1, 6);

            bool IsPlaying = false;
            bool IsPaused = false;
            bool restartGame = false;

            string command = "m";
            int brickRows = 8;
            int brickCols = 14;
            int brickRowsScreen2 = 8;
            int brickColsScreen2 = 14;
            Brick[,] bricks = new Brick[brickRows, brickCols];
            Brick[,] bricksScreen1 = InitializeBricks(brickRows, brickCols);
            Brick[,] bricksScreen2 = InitializeBricks(brickRowsScreen2, brickColsScreen2); ;

            for (int i = 0; i < brickRows; i++)
            {
                for (int j = 0; j < brickCols; j++)
                {
                    bricksScreen1[i, j] = new Brick(j * 5, i * 2);
                }
            }

            if (current_level == 2)
            {
                for (int i = 0; i < brickRowsScreen2; i++)
                {
                    for (int j = 0; j < brickColsScreen2; j++)
                    {
                        bricksScreen2[i, j] = new Brick(j * 5, i * 2);
                    }
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
                        Restart( ref ball, ref restartGame,  brickRows,  brickCols, ref bricks, paddle, ref current_level, bricksScreen1, brickRowsScreen2, brickColsScreen2, bricksScreen2);
                    }
                    else if (command == "H" || command == "h")
                    {
                        command = Help_Func();
                    }
                    else if (command == "S" || command == "s")
                    {
                        Console.Clear();
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
                        WriteAt($"Försök: {Lives}", 0, 21);

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


                        if (ball.Y > Console.WindowHeight + 2)
                        {
                            Lives--;
                            ball = new Ball(Console.WindowWidth / 2, Console.WindowHeight - 2, 1, -1);
                            paddle = new Paddle(Console.WindowWidth / 2 - 3, Console.WindowHeight - 1, 6);
                        }
                        if (Lives == 0)
                        {
                            poäng = Lose(poäng, out Lives, out IsPlaying, out IsPaused, out restartGame, out command);

                        }

                        if (current_level == 1)
                        {
                            for (int i = 0; i < brickRows; i++)
                            {
                                for (int j = 0; j < brickCols; j++)
                                {
                                    Brick brick = bricksScreen1[i, j];
                                    if (brick.IsVisible && ball.X >= brick.X && ball.X < brick.X + 5 && ball.Y == brick.Y)
                                    {
                                        brick.IsVisible = false;
                                        ball.ChangeY *= -1;

                                        if (i >= 6)
                                            poäng += 1;
                                        else if (i >= 3)
                                            poäng += 3;
                                        else if (i == 2)
                                            poäng += 5;
                                        else
                                            poäng += 7;

                                    }
                                }
                            }
                        }

                        if (current_level == 2)
                        {
                            for (int i = 0; i < brickRowsScreen2; i++)
                            {
                                for (int j = 0; j < brickColsScreen2; j++)
                                {
                                    Brick brick = bricksScreen2[i, j];
                                    if (brick.IsVisible && ball.X >= brick.X && ball.X < brick.X + 5 && ball.Y == brick.Y)
                                    {
                                        brick.IsVisible = false;
                                        ball.ChangeY *= -1;
                                        if (i >= 6)
                                            poäng += 1;
                                        else if (i >= 3)
                                            poäng += 3;
                                        else if (i == 2)
                                            poäng += 5;
                                        else
                                            poäng += 7;
                                    }
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

                        if (current_level == 1)
                        {
                            DisplayBricks(bricksScreen1);
                            if (AllBricksInvisible(bricksScreen1))
                            {
                                current_level = 2;
                            }
                        }
                        else if (current_level == 2)
                        {
                            DisplayBricks(bricksScreen2);
                            if (AllBricksInvisible(bricksScreen2))
                            {
                                poäng = Winner(poäng, out Lives, out IsPlaying, out IsPaused, out restartGame, out command, ref current_level);                               
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
                            else if (!IsPaused &&key.Key == ConsoleKey.Spacebar)
                            {
                                IsPaused = true;
                                Console.Clear();
                                if (IsPaused)
                                {
                                    Console.WriteLine("Spel pausat. tryck 'Spacebar' för att försätta.\ntryck 'm' för att gå till meny.\ntryck 's' för att avsluta spelet.");
                                }
                            }
                            
                        }

                        Thread.Sleep(50);
                    }
                    else
                    {
                        pause(ref IsPlaying, ref IsPaused, ref restartGame, ref command); // Introduce a small delay to avoid excessive CPU usage
                    }

                    Restart(ref ball, ref restartGame, brickRows, brickCols, ref bricks, paddle, ref current_level, bricksScreen1, brickRowsScreen2, brickColsScreen2, bricksScreen2);
                }

                static void Restart(ref Ball ball, ref bool restartGame, int brickRows, int brickCols, ref Brick[,] bricks, Paddle paddle, ref int current_level, Brick[,] bricksScreen1, int brickRowsScreen2, int brickColsScreen2, Brick[,] bricksScreen2)
                {
                    if (restartGame)
                    {

                        ball = new Ball(Console.WindowWidth / 2, Console.WindowHeight - 2, 1, -1);
                        bricks = new Brick[brickRows, brickCols];
                        paddle.X = Console.WindowWidth / 2 - 3;

                        if (current_level == 1)
                        {
                            for (int i = 0; i < brickRows; i++)
                            {
                                for (int j = 0; j < brickCols; j++)
                                {
                                    bricksScreen1[i, j] = new Brick(j * 5, i * 2);
                                }
                            }

                            for (int i = 0;i < brickRowsScreen2; i++)
                            {
                                for(int j = 0;j < brickColsScreen2; j++)
                                {
                                    bricksScreen2[i, j] = new Brick(j * 5, i * 2);
                                }
                            }
                        }

                        restartGame = false;
                        current_level = 1;  // Reset the current level
                    }
                }

                static void DisplayBricks(Brick[,] bricks)
                {
                    foreach (var brick in bricks)
                    {
                        if (brick.IsVisible)
                        {
                            Console.SetCursorPosition(brick.X, brick.Y);
                            Console.Write("[___]");
                        }
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

                static int Lose(int poäng, out int Lives, out bool IsPlaying, out bool IsPaused, out bool restartGame, out string command)
                {
                    IsPaused = false;
                    IsPlaying = false;
                    restartGame = true;

                    Console.Clear();
                    WriteAt($"Game Over", Console.WindowWidth / 3, Console.WindowHeight / 3);
                    WriteAt($"poäng: {poäng}", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 1);
                    WriteAt("Tryck [n] för nytt spel", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 2);
                    WriteAt("tryck [m] för meny", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 3);
                    WriteAt("tryck [s] för att sluta", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 4);
                    WriteAt($"> ", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 5);
                    Console.SetCursorPosition((Console.WindowWidth / 3) + 2, (Console.WindowHeight / 3) + 5);
                    command = Console.ReadLine();
                    Lives = 3;
                    poäng = 0;
                    return poäng;
                }

                static int Winner(int poäng, out int Lives, out bool IsPlaying, out bool IsPaused, out bool restartGame, out string command, ref int current_level)
                {
                    IsPaused = false;
                    IsPlaying = false;
                    restartGame = true;

                    Console.Clear();
                    WriteAt($"** Du Vinner **", Console.WindowWidth / 3, Console.WindowHeight / 3);
                    WriteAt($" poäng: {poäng}", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 1);
                    WriteAt("  Tryck [n] för nytt spel", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 2);
                    WriteAt("  tryck [m] för meny", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 3);
                    WriteAt("  tryck [s] för att sluta", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 4);
                    WriteAt($"> ", Console.WindowWidth / 3, (Console.WindowHeight / 3) + 5);
                    Console.SetCursorPosition((Console.WindowWidth / 3) + 2, (Console.WindowHeight / 3) + 5);
                    command = Console.ReadLine();
                    Lives = 3;
                    poäng = 0;
                    return poäng;
                }
            }

            static bool AllBricksInvisible(Brick[,] bricks)
            {
                foreach (var brick in bricks)
                {
                    if (brick.IsVisible)
                    {
                        return false;
                    }
                }
                return true;
            }

            static void Meny()
            {
                Console.WriteLine("Välkommen till Breakout spel, Skriv bokstav i hakparenteser för att välja !!!\n\n");

                Console.WriteLine("[N]y spel");
                Console.WriteLine("[H]jälp");
                Console.WriteLine("[S]luta");
            }
            static void Hjälp()
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
            static void WriteAt(string s, int x, int y)
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

            static void pause(ref bool IsPlaying, ref bool IsPaused, ref bool restartGame, ref string command)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.M)
                    {
                        IsPlaying = false;
                        command = "M";
                        IsPaused = false;
                        restartGame = true;
                    }
                    else if (key.Key == ConsoleKey.S)
                    {
                        IsPlaying = false;
                        command = "s";
                        IsPaused = false;
                    }
                    else if (key.Key == ConsoleKey.Spacebar)
                    {
                        IsPaused = false;
                        Console.Clear();
                    }
                }

                Thread.Sleep(10);
            }
        }

        private static Brick[,] InitializeBricks(int rows, int cols)
        {
            Brick[,] bricks = new Brick[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    bricks[i, j] = new Brick(j * 5, i * 2);
                }
            }

            return bricks;
        }
    } 
}

