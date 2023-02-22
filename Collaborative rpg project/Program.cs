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
            float DamageBeingDealt = 0;
            float DamageMultiplier = 1.5f;
            bool IsGameOver = false;
            int CriticalChance = 12;
            int CriticalHit = 0;
            float CriticalHitMultipler = 2;

            //Status Variables
            float BurnDamageModifier = 0.5f;
            float BurnDamage = 0f;
            int BurnMaxTurns = 3;
            int BurnTurns = 0;

            //Player Variables
            float PlayerHP = 100;
            float PlayerMaxHP = 100;
            float PlayerAttack = 5;
            bool IsPlayerDead = false;
            string PlayerCritText = "You got a critical hit! ";
            float[] PlayerFloatList = new float[3] { PlayerHP, PlayerMaxHP, PlayerAttack };
            bool[] PlayerBoolList = new bool[1] { IsPlayerDead };

            //Enemy Variables
            float EnemyHP = 100;
            float EnemyMaxHP = 100;
            float EnemyAttack = 5;
            bool IsEnemyDead = false;
            string EnemyCritText = "The Enemy Attacked you and got a critical hit! ";
            string EnemyStatus = "";
            float[] EnemyFloatList = new float[3] { EnemyHP, EnemyMaxHP, EnemyAttack };
            bool[] EnemyBoolList = new bool[1] { IsEnemyDead };

            //RUN LOOP                  =========================================================
            TitleScreen();
            while (IsGameRunning == true)
            {
                CheckForGameOver();
                if(IsGameRunning == true)
                {
                    PlayerTurn();
                }
                CheckForGameOver();
                if (IsGameRunning == true)
                {
                    EnemyTurn();
                }
            }
            NextText_Placeholder();
            Console.WriteLine("GAME OVER");

            //SYSTEM FUNCTION           =========================================================

            //A function that handes the start of the game
            void TitleScreen()
            {
                Console.WriteLine("This is Version 0.3");
                Console.WriteLine("added a critical hit system");
                Console.WriteLine("Hello and welcome to the RPG combat system press enter to begin...");
                Console.ReadLine();
            }

            //Runs the check to see whether an effect is critical or not
            void CriticalCheck(string CritText)
            {
                Random rnd = new Random();
                CriticalHit = rnd.Next(1, 100);

                //checks if the critical hit is within range of 12

                if (CriticalHit < CriticalChance)
                {
                    //operation for critical hit damage
                    DamageBeingDealt *= CriticalHitMultipler;

                    //resets criticalhit variable
                    CriticalHit = 0;

                    //updates the player on getting a crit hit
                    Console.WriteLine(CritText);
                }
            }

            //Handles the assigning of a status effect to a target
            string OverwriteStatus(string PriorStatus, string NewStatus)
            {
                if (NewStatus != PriorStatus)
                {
                    Console.WriteLine("A status interaction has occured: " + PriorStatus + " ---------> " + NewStatus);
                    PriorStatus = NewStatus;
                    Console.WriteLine("");
                }
                return PriorStatus;
            }

            //Hanldes the selection of a statuses effect function
            string StatusEffectsMaster(float[] TargetFloatList, string TargetStatus)
            {
                //Set of conditionals to run StatusEffects functions that are held in Action functions
                if (TargetStatus == "Burnt")
                {
                    TargetStatus = BurnStatusEffect(TargetFloatList, TargetStatus);
                }
                else if (TargetStatus == "Frozen")
                {
                    FreezeStatusEffect(TargetFloatList);
                }
                else if (TargetStatus == "Acidic")
                {
                    AcidicStatusEffect(TargetFloatList);
                }
                else if (TargetStatus == "Bleeding")
                {
                    BleedStatusEffect(TargetFloatList);
                }
                return TargetStatus;
            }

            //COMMON FUNCTIONS          =========================================================

            //A placeholder for detecting Enter input to clear the screen
            void NextText_Placeholder()
            {
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
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
                Console.WriteLine("Enemy health    ============= " + EnemyHP + "/" + EnemyMaxHP + " ---------- " + EnemyStatus);
                Console.WriteLine("");
            }

            //A common check for whether the game should continue running
            void CheckForGameOver()
            {
                if (IsPlayerDead != false || IsEnemyDead != false)
                {
                    IsGameRunning = false;
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
                Console.WriteLine("Enter a number for which weapon you would like to attack with:");
                Console.WriteLine("1) Sword");
                Console.WriteLine("2) Moloktov Cocktail");
                Console.WriteLine("3) Ice-Pick");
                Console.WriteLine("4) Blow Dart");
                Console.WriteLine("5) Dagger");
            }

            //Handles the selection of the players action
            void PlayerOptionsSelection()
            {
                //TempInput = Console.ReadLine();
                //assigns a variable for the inputed key
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);


                if (consoleKeyInfo.KeyChar == '1')
                {
                    Console.WriteLine("");
                    Sword_PlayerAction();
                }
                else if (consoleKeyInfo.KeyChar == '2')
                {
                    Console.WriteLine("");
                    Moloktov_PlayerAction();
                }
                else if (consoleKeyInfo.KeyChar == '3')
                {
                    Console.WriteLine("");
                    IcePick_PlayerAction();
                }
                else if (consoleKeyInfo.KeyChar == '4')
                {
                    Console.WriteLine("");
                    BlowDart_PlayerAction();
                }
                else if (consoleKeyInfo.KeyChar == '5')
                {
                    Console.WriteLine("");
                    Dagger_PlayerAction();
                }
            }

            //Handles the events of the enemies turn
            void EnemyTurn()
            {
                StatusText();
                Console.WriteLine("It is the Enemy's turn");

                EnemyStatus = StatusEffectsMaster(EnemyFloatList, EnemyStatus);

                //damaging the target
                DamageBeingDealt = EnemyAttack;
                CriticalCheck(EnemyCritText);
                PlayerFloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                CheckForDeath(PlayerHP, PlayerBoolList);
                UpdateVariables();

                //output text to inform user of what happened
                Console.WriteLine("The Enemy has attacked the Player!!!");
                Console.WriteLine("The Enemy did " + DamageBeingDealt + " damage!!!");
                Console.WriteLine("");

                NextText_Placeholder();
            }
           
            //ACTION FUNCTIONS          =========================================================

            //The player Attacks the enemy for neutral damage
            void Sword_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack * DamageMultiplier;
                CriticalCheck(PlayerCritText);
                EnemyFloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                CheckForDeath(EnemyHP, EnemyBoolList);
                UpdateVariables();
                Console.WriteLine("You attack the enemy with a sword for " + DamageBeingDealt + " damage!!!");
                StatusText();
                
            }

            //The player Attacks the enemy for fire damage                          //PLACEHOLDER
            void Moloktov_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);

                //Regular attack stuff
                EnemyFloatList[0] -= DamageBeingDealt;
                UpdateVariables();

                //Burn status stuff
                EnemyStatus = OverwriteStatus(EnemyStatus, "Burnt");
                BurnTurns = BurnMaxTurns;
                BurnDamage = DamageBeingDealt * BurnDamageModifier;

                CheckForDeath(EnemyHP, EnemyBoolList);
                UpdateVariables();

                Console.WriteLine("You throw a moloktov cocktail  for " + DamageBeingDealt + " damage!!!");
                StatusText();
            }

            //The player Attacks the enemy for frost damage                         //PLACEHOLDER
            void IcePick_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);
                EnemyFloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                EnemyStatus = OverwriteStatus(EnemyStatus, "Frozen");
                CheckForDeath(EnemyHP, EnemyBoolList);
                UpdateVariables();
                Console.WriteLine("You shatter the enemy with a ice-pick for " + DamageBeingDealt + " damage!!!");
                StatusText();
            }

            //The player Attacks the enemy for poison damage                        //PLACEHOLDER
            void BlowDart_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);
                EnemyFloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                EnemyStatus = OverwriteStatus(EnemyStatus, "Acidic");
                CheckForDeath(EnemyHP, EnemyBoolList);
                UpdateVariables();
                Console.WriteLine("You spat a poiosnous dart at the enemy for " + DamageBeingDealt + " damage!!!");
                StatusText();
            }

            //The player Attacks the enemy for bleed damage                         //PLACEHOLDER
            void Dagger_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);
                EnemyFloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                EnemyStatus = OverwriteStatus(EnemyStatus, "Bleeding");
                CheckForDeath(EnemyHP, EnemyBoolList);
                UpdateVariables();
                Console.WriteLine("You stabbed the enemy with a dagger for " + DamageBeingDealt + " damage!!!");
                StatusText();
            }

            string BurnStatusEffect(float[] TargetFloatList, string TargetStatus)
            {
                //Insert Burn Status Effect here
                BurnTurns -= 1;
                
                if (BurnTurns > 0)
                {
                    TargetFloatList[0] -= BurnDamage;
                    Console.WriteLine("They take " + BurnDamage + " damge from burn!!!");
                }
                else
                {
                    TargetStatus = "";
                    Console.WriteLine("They are cured of burn");
                }
                UpdateVariables();
                return TargetStatus;
            }

            void FreezeStatusEffect(float[] TargetFloatList)
            {
                //Insert Freeze Status Effect here
            }

            void AcidicStatusEffect(float[] TargetFloatList)
            {
                //Insert Acidic Status Effect here
            }

            void BleedStatusEffect(float[] TargetFloatList)
            {
                //Insert Bleed Status Effect here
            }
        }
    }
}
