﻿using System;

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
            float DamageBeingDealt;
            float DamageMultiplier;
            bool IsGameOver = false;

            //Player Variables
            float PlayerHP = 10;
            float PlayerMaxHP = 10;
            float PlayerAttack = 5;
            bool IsPlayerDead = false;
            float[] PlayerFloatList = new float[3] { PlayerHP, PlayerMaxHP, PlayerAttack };
            bool[] PlayerBoolList = new bool[1] { IsPlayerDead };

            //Player Variables
            float EnemyHP = 10;
            float EnemyMaxHP = 10;
            float EnemyAttack = 5;
            bool IsEnemyDead = false;
            float[] EnemyFloatList = new float[3] { EnemyHP, EnemyMaxHP, EnemyAttack };
            bool[] EnemyBoolList = new bool[1] { IsEnemyDead };

            //RUN LOOP                  =========================================================
            while (IsGameRunning == true)
            {
                CheckForGameOver();
                if(IsGameOver == false)
                {
                    PlayerTurn();
                    EnemyTurn();
                }
                else
                {
                    IsGameRunning = false;
                }
            }
            NextText_Placeholder();
            Console.WriteLine("GAME OVER");

            //SYSTEM FUNCTION           =========================================================


            //COMMON FUNCTIONS          =========================================================

            //A placeholder for detecting Enter input to clear the screen
            void NextText_Placeholder()
            {
                Console.WriteLine("Press ENTER to continue");
                TempInput = Console.ReadLine();
                Console.Clear();
            }

            //Updates the variables to be equal to the values stored in the relevant lists that have been modified
            void UpdateVariables()
            {
                PlayerHP = PlayerFloatList[0];
                IsPlayerDead = PlayerBoolList[0];

                EnemyHP = EnemyFloatList[0];
                IsEnemyDead = EnemyBoolList[0];
            }

            //Outputs the status of both the enemy and the player
            void StatusText()
            {
                Console.WriteLine("");
                Console.WriteLine("Player health   ============= " + PlayerHP + "/" + PlayerMaxHP);
                Console.WriteLine("Enemy health    ============= " + EnemyHP + "/" + EnemyMaxHP);
                Console.WriteLine("");
            }

            //A common check for whether the game should continue running
            void CheckForGameOver()
            {
                if (IsPlayerDead != false || IsEnemyDead != false)
                {
                    IsGameOver = true;
                }
            }

            //A common check for whether the player or enemy has died
            void CheckForDeath(float HealthPool, bool[] BoolList)
            {
                if (HealthPool <= 0)
                {
                    BoolList[0] = true;
                }
            }

            //PLAYER + ENEMY FUNCTIONS  =========================================================

            //Handles the events of the players turn
            void PlayerTurn()
            {
                StatusText();
                Console.WriteLine("It is the Players turn");
                Console.WriteLine("What would you like to do?");
                PlayerOptionsText();
                PlayerOptionsSelection();
                NextText_Placeholder();
            }

            //Handles the output of text for the players actions
            void PlayerOptionsText()
            {
                Console.WriteLine("Enter a number:");
                Console.WriteLine("1) Attack");
            }

            //Handles the selection of the players action
            void PlayerOptionsSelection()
            {
                TempInput = Console.ReadLine();
                
                if (TempInput == "1")
                {
                    DamageBeingDealt = PlayerAttack;
                    Attack_PlayerAction();
                }
            }

            //Handles the events of the enemies turn
            void EnemyTurn()
            {
                StatusText();
                Console.WriteLine("It is the Enemy's turn");
                NextText_Placeholder();
            }
           
            //ACTION FUNCTIONS          =========================================================

            //The player Attacks the enemy
            void Attack_PlayerAction()
            {
                EnemyFloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                CheckForDeath(EnemyHP, EnemyBoolList);
                UpdateVariables();
                Console.WriteLine("You attack the enemy for " + DamageBeingDealt + " damage!!!");
                StatusText();
            }
        }
    }
}
