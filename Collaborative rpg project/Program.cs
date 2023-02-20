using System;

namespace Collaborative_rpg_project
{
    class Program
    {
        static void Main(string[] args)
        {
            //VARIABLES                 =========================================================
            //System Variables
            string TempInput;
            bool IsGameRunning = true;

            //Player Variables
            float PlayerHP = 10;

            //RUN LOOP                  =========================================================
            while(IsGameRunning == true)
            {
                PlayerTurn();
                EnemyTurn();
            }

            //SYSTEM FUNCTION           =========================================================

            //COMMON FUNCTIONS          =========================================================

            void NextText_Placeholder()
            {
                Console.WriteLine("Press ENTER to continue");
                TempInput = Console.ReadLine();
                Console.Clear();
            }

            //PLAYER + ENEMY FUNCTIONS  =========================================================

            void PlayerTurn()
            {
                Console.WriteLine("It is the Players turn");
                NextText_Placeholder();
            }

            void EnemyTurn()
            {
                Console.WriteLine("It is the Enemy's turn");
                NextText_Placeholder();
            }
            //ACTION FUNCTIONS          =========================================================
        }
    }
}
