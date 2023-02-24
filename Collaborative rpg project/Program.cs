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
            float BurnDamageModifier = 0.7f;
            float BurnDamage = 0f;
            int BurnMaxTurns = 3;
            int BurnTurns = 0;
            int FreezeMaxTurns = 4;
            int FreezeTurns = 0;
            bool IsFrozen = false;
            int AcidicMaxTurns = 3;
            int AcidicTurns = 0;
            bool IsAcidic = false;
            float AcidicModifier = 0.7f;
            float AcidDamage = 0;
            int BleedMaxTurns = 3;
            int BleedTurns = 0;
            int BleedCritModifier = 38;
            bool IsBleed = false;

            //Player Variables
            float PlayerHP = 100;
            float PlayerMaxHP = 100;
            float PlayerAttack = 10;
            bool IsPlayerDead = false;
            string PlayerCritText = "You got a critical hit! ";
            float[] PlayerFloatList = new float[3] { PlayerHP, PlayerMaxHP, PlayerAttack };
            bool[] PlayerBoolList = new bool[1] { IsPlayerDead };

            //Enemy Variables
            float EnemyHP = 150;
            float EnemyMaxHP = 150;
            float EnemyAttack = 10;
            bool IsEnemyDead = false;
            string EnemyCritText = "The Enemy Attacked you and got a critical hit! ";
            string EnemyStatus = "";
            float[] EnemyFloatList = new float[3] { EnemyHP, EnemyMaxHP, EnemyAttack };
            bool[] EnemyBoolList = new bool[1] { IsEnemyDead };

            //RUN LOOP                  =========================================================
            TitleScreen();
            while (IsGameRunning == true)
            {
                //Playing the Players turn if the game should not be finishing
                CheckForGameOver();
                if(IsGameRunning == true)
                {
                    PlayerTurn();
                }

                //Playing the Enemies turn if the game should not be finishing
                CheckForGameOver();
                if (IsGameRunning == true)
                {
                    EnemyTurn();
                }
            }
            //Handleing the end of the game
            NextText_Placeholder();
            Console.WriteLine("GAME OVER");

            //SYSTEM FUNCTIONS           =========================================================

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
                //Getting the random value
                Random rnd = new Random();
                CriticalHit = rnd.Next(1, 100);

                //Increasing the crit rate if the Player is critting and the enemy is bleeding
                if (IsBleed == true && CritText == PlayerCritText)
                {
                    CriticalHit -= BleedCritModifier;
                }
                
                //If it is the first turn of an enemy bleeding, guarantee its crit
                if (BleedTurns == (BleedMaxTurns - 1) && CritText == EnemyCritText)
                {
                    CriticalHit = 0;
                }

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
                //Checking that the status wants to be changed
                if (NewStatus != PriorStatus)
                {
                    //Resetting status related variables
                    IsAcidic = false;
                    IsFrozen = false;
                    IsBleed = false;

                    //Not allowing frozen to overwrite chilled
                    if (NewStatus == "Frozen" && PriorStatus == "Chilled")
                    {
                        return PriorStatus;
                    }

                    //Overwriting status and declaring that a status interaction has occured
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
                else if (TargetStatus == "Frozen" || TargetStatus == "Chilled")
                {
                    TargetStatus = FreezeStatusEffect(TargetFloatList, TargetStatus);
                }
                else if (TargetStatus == "Dripping with Acid")
                {
                    TargetStatus = AcidicStatusEffect(TargetFloatList, TargetStatus);
                }
                else if (TargetStatus == "Bleeding")
                {
                    TargetStatus = BleedStatusEffect(TargetFloatList, TargetStatus);
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

            //Handling Damage being dealt
            void DealDamage(float[] FloatList, float HP, bool[] BoolList)
            {
                FloatList[0] -= DamageBeingDealt;
                UpdateVariables();
                CheckForDeath(HP, BoolList);
                UpdateVariables();
            }

            //PLAYER + ENEMY FUNCTIONS  =========================================================

            //Handles the events of the players turn
            void PlayerTurn()
            {
                Console.WriteLine("It is the Players turn");

                //Checking for whether the player should suffer from Acid recoil
                if (IsAcidic == true)
                {
                    //Setting acid recoil damage
                    Random rnd = new Random();
                    AcidDamage = rnd.Next(1, 3);
                    DamageBeingDealt = AcidDamage;
                    DealDamage(PlayerFloatList, PlayerHP, PlayerBoolList);

                    Console.WriteLine("A small amount of acid drips off of the enemy and deals " + AcidDamage + " Damage");
                }
                StatusText();

                //Providing Player Options and Handling the selection of said options
                Console.WriteLine("What would you like to do?");
                PlayerOptionsText();
                PlayerOptionsSelection();
                NextText_Placeholder();
            }

            //Handles the output of text for the players actions
            void PlayerOptionsText()
            {
                Console.WriteLine("Enter a number for which weapon you would like to attack with:");
                Console.WriteLine("1) Sword               - High physical damage");
                Console.WriteLine("2) Moloktov Cocktail   - Sets the enemy ablaze");
                Console.WriteLine("3) Ice-Pick            - Freezes the enemy for the next turn");
                Console.WriteLine("4) Bottle of Acid      - Coats the enemy in acid which errodes their weapon");
                Console.WriteLine("5) Dagger              - Leave a gaping wound in the target");
            }

            //Handles the selection of the players action
            void PlayerOptionsSelection()
            {
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
                Console.WriteLine("It is the Enemy's turn");

                //Handling the effect that a status would do
                EnemyStatus = StatusEffectsMaster(EnemyFloatList, EnemyStatus);
                StatusText();

                //Handling a frozen turn
                if (IsFrozen == true)
                { 
                    Console.WriteLine("However the Enenmy cant act because it is Frozen still");
                }
                else
                {
                    //Handling a non-frozen turns assigned attack and damage
                    DamageBeingDealt = EnemyAttack;
                    
                    //Handling whether the Damage is reduced by acid or not
                    if (IsAcidic == true)
                    {
                        DamageBeingDealt *= AcidicModifier;
                        Console.WriteLine("The Enemies weapon has melted due to the acid making it less efficient");
                    }

                    CriticalCheck(EnemyCritText);

                    DealDamage(PlayerFloatList, PlayerHP, PlayerBoolList);

                    //output text to inform user of what happened
                    Console.WriteLine("The Enemy has attacked the Player!!!");
                    Console.WriteLine("The Enemy did " + DamageBeingDealt + " damage!!!");
                }
                Console.WriteLine("");

                NextText_Placeholder();
            }
           
            //ACTION FUNCTIONS          =========================================================

            //The player Attacks the enemy for neutral damage
            void Sword_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack * DamageMultiplier;
                CriticalCheck(PlayerCritText);

                DealDamage(EnemyFloatList, EnemyHP, EnemyBoolList);

                //Outputting text
                Console.WriteLine("You slash the enemy with a sword for " + DamageBeingDealt + " damage!!!");
                StatusText();
                
            }

            //The player Attacks the enemy for fire damage                          
            void Moloktov_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);

                DealDamage(EnemyFloatList, EnemyHP, EnemyBoolList);

                //Burn status stuff
                EnemyStatus = OverwriteStatus(EnemyStatus, "Burnt");
                BurnTurns = BurnMaxTurns;
                BurnDamage = DamageBeingDealt * BurnDamageModifier;

                //Outputting text
                Console.WriteLine("You throw a moloktov cocktail at the enemy for " + DamageBeingDealt + " damage, setting the enemy ablaze with the combusting liquor!!!");
                StatusText();
            }

            //The player Attacks the enemy for frost damage                         
            void IcePick_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);

                DealDamage(EnemyFloatList, EnemyHP, EnemyBoolList);

                //Ice status stuff
                EnemyStatus = OverwriteStatus(EnemyStatus, "Frozen");
                if (EnemyStatus == "Frozen")
                {
                    FreezeTurns = FreezeMaxTurns;
                }

                //Outputting text
                Console.WriteLine("You shatter the enemy with a ice-pick (pick made of ice) for " + DamageBeingDealt + " damage, freezing them for the next turn!!!");
                StatusText();
            }

            //The player Attacks the enemy for poison damage                        
            void BlowDart_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);

                DealDamage(EnemyFloatList, EnemyHP, EnemyBoolList);

                //Acidic status stuff
                EnemyStatus = OverwriteStatus(EnemyStatus, "Dripping with Acid");
                AcidicTurns = AcidicMaxTurns;

                //Outputting text
                Console.WriteLine("You smashed a bottle of acid on the enemy for " + DamageBeingDealt + " damage, while also melting their weapon partially!!!");
                StatusText();
            }

            //The player Attacks the enemy for bleed damage                         
            void Dagger_PlayerAction()
            {
                DamageBeingDealt = PlayerAttack;
                CriticalCheck(PlayerCritText);

                DealDamage(EnemyFloatList, EnemyHP, EnemyBoolList);

                //Bleed status stuff
                EnemyStatus = OverwriteStatus(EnemyStatus, "Bleeding");
                BleedTurns = BleedMaxTurns;

                //Outputting text
                Console.WriteLine("You stabbed the enemy with a dagger for " + DamageBeingDealt + " damage, leaving a deep open wound!!!");
                StatusText();
            }

            //The status effect for burn
            string BurnStatusEffect(float[] TargetFloatList, string TargetStatus)
            {
                BurnTurns -= 1;
                
                if (BurnTurns > 0)
                {
                    //Dealing burn damage
                    DamageBeingDealt = BurnDamage;
                    DealDamage(EnemyFloatList, EnemyHP, EnemyBoolList);
                    Console.WriteLine("They take " + BurnDamage + " damge from burn!!!");
                }
                else
                {
                    //naturally removing burn
                    TargetStatus = "";
                    Console.WriteLine("They are cured of burn");
                }
                UpdateVariables();
                return TargetStatus;
            }

            //The status effect for freeze
            string FreezeStatusEffect(float[] TargetFloatList, string TargetStatus)
            {
                FreezeTurns -= 1;

                if (FreezeTurns == (FreezeMaxTurns - 1))
                {
                    //Freezing enemy
                    IsFrozen = true;
                }
                else if (FreezeTurns > 0)
                {
                    //Chilling enemy
                    IsFrozen = false;
                    TargetStatus = "Chilled";
                    Console.WriteLine("They remain Chilled but are no longer Frozen still");
                }
                else
                {
                    //naturally removing chill
                    TargetStatus = "";
                    Console.WriteLine("They are cured of Chilled");
                }
                UpdateVariables();
                return TargetStatus;
            }

            //The status effect for acidic
            string AcidicStatusEffect(float[] TargetFloatList, string TargetStatus)
            {
                AcidicTurns -= 1;

                if (AcidicTurns > 0)
                {
                    //Acidifying Enemy
                    IsAcidic = true;
                }
                else
                {
                    //Naturally removing acid
                    TargetStatus = "";
                    IsAcidic = false;
                    Console.WriteLine("They are cured of Acidic");
                }
                UpdateVariables();
                return TargetStatus;
            }

            //The status effect for bleed
            string BleedStatusEffect(float[] TargetFloatList, string TargetStatus)
            {
                BleedTurns -= 1;

                if (BleedTurns == (BleedMaxTurns - 1))
                {
                    //Alerting Player of enemies guaranteed crit
                    Console.WriteLine("The enemy enters a rage and is guaranteed to hit critically");
                }

                if (BleedTurns > 0)
                {
                    //Bleeding the enemy
                    IsBleed = true;
                }
                else
                {
                    //naturally removing bleed
                    TargetStatus = "";
                    Console.WriteLine("They are cured of Bleed");
                }
                UpdateVariables();
                return TargetStatus;
            }
        }
    }
}
