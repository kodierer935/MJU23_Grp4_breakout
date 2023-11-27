using System;


namespace BREAKOUT_1
{
    internal class Program
    {
        
       
        static void Main(string[] args)
        {
            //Console.WindowWidth = 14;
            //Console.WindowHeight = 16;
            
            string Intact = "[_____]";
            string Broken = "     ";

            int AntalRad = 8;
            int AntalBrickor = 14;
            int i ;
            int j ;
            int y = 3;
            int x = 7;
            string[,] BrickLista = SkapaPlan(AntalRad, AntalBrickor);
            string[,] boll = new string[x, y];
            i = x;
            j = y;
            while(true)
            {
                // boll implementering

                SkrivUtSpelplan(BrickLista);
                Thread.Sleep(1000);
                if (x == i && y == j)
                    TaBortBricka(BrickLista, x, y);
                Console.Clear();
            }
            
            
        }
        static string[,] SkapaPlan(int AntalRad, int AntalBrickor)
        {
            string Intact = "[___]";
            string[,] SkapaPlan = new string[AntalRad, AntalBrickor];

            for (int i = 0; i < AntalRad; i++)
            {
                for (int j = 0; j < AntalBrickor; j++)
                {
                    SkapaPlan[i,j] = Intact;
                }
            }
            return SkapaPlan;
        }
        static void SkrivUtSpelplan(string[,] SkapaPlan)
        {
            for (int i = 0; i < SkapaPlan.GetLength(0); i++)
            {
                for (int j = 0; j < SkapaPlan.GetLength(1); j++)
                {
                    Console.Write(SkapaPlan[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void TaBortBricka(string[,] SpelPlan, int rad, int kolumn)
        {
            string Broken = "     ";
            if (rad >= 0 && rad < SpelPlan.GetLength(0) && kolumn >= 0 && kolumn < SpelPlan.GetLength(1))
            {
                SpelPlan[rad, kolumn] = Broken;
            }  
        }

        
        //static string[,] SpelPlan (int AntalRad, int AntalBrickor)
        //{
        //    string[,] SpelPlan = new string[AntalRad, AntalBrickor];

        //    for(int i = 0 ; i < AntalBrickor; i++)
        //    {
        //        for(int j ; j < AntalBrickor; j++)
        //        {
        //            SpelPlan[i,j] = "[____]";
        //        }
        //    }
        //    return SpelPlan;
        //}
    }
}