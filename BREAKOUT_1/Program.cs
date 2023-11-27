 
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
    public class Program
    {
        static void Main(string[] args)
        {
            Ball ball = new Ball(Console.WindowWidth / 2, Console.WindowHeight - 2, 1, -1);

            int brickRows = 8;
            int brickCols = Console.WindowWidth / 5;
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
                Console.Clear();

                ball.X += ball.ChangeX;
                ball.Y += ball.ChangeY;

                if (ball.X <= 1 || ball.X >= Console.WindowWidth - 1)
                    ball.ChangeX *= -1;
                if (ball.Y <= 0 || ball.Y >= Console.WindowHeight - 1)
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
                        }
                    }
                }

                Console.SetCursorPosition(ball.X, ball.Y);
                Console.Write("*");

                foreach (var brick in bricks)
                {
                    if (brick.IsVisible)
                    {
                        Console.SetCursorPosition(brick.X, brick.Y);
                        Console.Write("[___]");
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}
