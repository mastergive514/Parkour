// A Parkour MiniGame Plugin made by NassemRB
// Remake of Upsurge server but without RoundsGame
// Before you compile this Plugin!
// You have to add the maps in line 40
// and please change variable "maxmaps" to how much maps do you have
// also Replace secretcode with your code in line 52
// How to use it?
// 
// 1. Add A Message Block with /Parkour checkpoint 1 (replace 1 with number of checkpoint you did)
// 2. Keep doing that until you finish all checkpoints in map 
// 3. Add A Message Block with /Parkour secretcode (Replace secretcode with your code)
// 4. Add Killer Blocks if Possible
// 5. Start Game By Doing /Parkour start
// You can stop game by doing /Parkour stop Or End game by doing /Parkour end
//
// LIST: (X: what i did | O: what i didnt did)
// |X| a working checkpoint
// |X| Commands
// |X| Add Picker
// |X| A working random map picker
// |O| Adding support for more maps
// |O| fixing bugs

// Thank you for using my plugin! your free to change or fix the code, but do not tell peoples that you made this
// Creator of this plugin is NassemRB

using System;
using System.Threading;

using MCGalaxy;

namespace MCGalaxy
{
	public class Parkour : Plugin
	{
		// The plugin's name (i.e what shows in /Plugins)
		public override string name { get { return "Parkour"; } }

		// The oldest version of MCGalaxy this plugin is compatible with
		public override string MCGalaxy_Version { get { return "1.9.5.1"; } }

		// Message displayed in server logs when this plugin is loaded
		public override string welcome { get { return "Loaded Message!"; } }

		// Who created/authored this plugin
		public override string creator { get { return "NassemRB"; } }
                
                // Very Static Family
                public static string GameName { get { return "Parkour"; } }
                public static bool GameStarted;
                public static bool CSGame;
                public static string GameLevel;
                public static string c1, c2, c3, c4, c5;
                public static string ar;
                public static string winner;
                public static int timeleft;
                public static string[] maps = {"pkr_cloudy", "pkr_classic", "pkr_bedrocky", "pkr_leaves"}; // REPLACE OR ADD MAPS HERE
                public static int maxmaps = 5; // CHANGE THIS TO HOW MUCH MAPS YOU HAVE
                public static string secretcode = "secretcode"; // REPLACE SECRETCODE WITH YOUR CODE, DO NOT SHARE WITH ANYONE
		// Called when this plugin is being loaded (e.g. on server startup)
		public override void Load(bool startup)
		{
                        Command.Register(new CmdParkour());
		}

		// Called when this plugin is being unloaded (e.g. on server shutdown)
		public override void Unload(bool shutdown)
		{
			GameStarted = false;
                        Command.Unregister(Command.Find("Parkour"));
		}
                
                public static void SpawnPlayers() {
                 
                 foreach(Player pl in PlayerInfo.Online.Items) {
                  PlayerActions.Respawn(pl);
                 }
                 }


                public static void CountStart() {
                
                int a = 10;
                while (a >= 1 ) { foreach(Player pl in PlayerInfo.Online.Items) { pl.SendCpeMessage(CpeMessageType.Announcement, "Starting in " + a + "s"); } a--; Thread.Sleep(1000); }
                
                CSGame = true;

}

                public static void StartGame(Player p) {
                  GameLevel = "pkr_classic";
                  if (GameStarted) { p.Message(GameName + " Game is already running."); }
                  CountStart();
                  if (!CSGame) { return; }
                  SpawnPlayers();
                  GameStarted = true;
                  foreach(Player pl in PlayerInfo.Online.Items) { pl.SendCpeMessage(CpeMessageType.Status1, "&b= Parkour ="); pl.SendCpeMessage(CpeMessageType.Status2, "Game Started");}
                  int t = 60;
                  Position pos = p.Pos;
                  while (t >=1) { t--; Thread.Sleep(1000); } // DEBUG moment
                  GameLevel = p.level.name;
                  StopGame(p);
                }
                
                public static void StopGame(Player p) {
		 
                 if (!GameStarted) { p.Message(GameName + " Game is not running."); }
                 GameStarted = false;
		 winner = null;
                 GameLevel = null;
                 
                 foreach(Player pl in PlayerInfo.Online.Items) { if (winner == null) { pl.Message("Nobody wins this round!"); } pl.Message(GameName + " Game Ends!"); }
                 

                }

                public static void WinGame(Player p) {
                 if (!GameStarted) { return; }
                 int reward = 10;
                 winner = p.name;
                 p.money += 10;
                 
                 foreach (Player pl in PlayerInfo.Online.Items) {
                  pl.Message("================================");
                  pl.Message("&aCongratulations for winner!&f " + p.color + p.name + " &aJust Won This Round!");
                  pl.Message("================================");

                }
                p.Message("&fYou received &e" + reward + "&f for winning this round!");
                StopGame(p);
                int a = 5;
                while (a >= 1) { foreach(Player pl in PlayerInfo.Online.Items) { pl.SendCpeMessage(CpeMessageType.Status2, "A break! " + a + "s Left"); } a--; Thread.Sleep(1000); }
                
                // TODO: Make a better WinGame 
                }

                public static void EndGame(Player p) {
                 if (!GameStarted) { p.Message(GameName + " Game is not running."); }
                 GameStarted = false;
                 
                 foreach(Player pl in PlayerInfo.Online.Items) { if (winner == null) { pl.Message("Nobody wins this round!"); } pl.Message(GameName + " Game Ends!"); }
                 PickGame();
                 StartGame(p);
                  
                }

                public static void SetupCheckpoint(Player p) {

                 if (!GameStarted) { return; }
                 if (ar == null) { return; }
 
                 if (ar == "1") { c1 = p.name;  SpawnTHEM(p); p.Message("You Reach Checkpoint 1"); }
                 else if (ar == "2") { c2 = p.name; SpawnTHEM(p); p.Message("You Reach Checkpoint 2");}
                 else if (ar == "3") { c3 = p.name; SpawnTHEM(p); p.Message("You Reach Checkpoint 3");}
                 else if (ar == "4") { c4 = p.name; SpawnTHEM(p); p.Message("You Reach Checkpoint 4");}
                 else if (ar == "5") { c5 = p.name; SpawnTHEM(p); p.Message("You Reach Checkpoint 5");}
                 
                 else if (ar == "end") { winner = p.name; WinGame(p); EndGame(p); }

                 else { return; }
                 

                }


                
 
                public static void PickGame() {
                Random r = new Random();
                int i = r.Next(0, 4);

                foreach(Player pl in PlayerInfo.Online.Items) {
                 pl.Message("%aType 1 or 2 or 3 to vote!");
                 timeleft = 10;
                 while (timeleft >= 1) {
                 pl.SendCpeMessage(CpeMessageType.BottomRight3, "&bChoosing in... &a" + timeleft + "s");
                 pl.SendCpeMessage(CpeMessageType.BottomRight1, "Server is choosing a new map...");
                 timeleft--;
                 Thread.Sleep(1000);
 
                 }

                 
                foreach(Player d in PlayerInfo.Online.Items) { if (maps[i] == d.level.name) i += 1;  }          
                 
                 pl.Message("Next map is %b" + maps[i]);
                 PlayerActions.ChangeMap(pl, maps[i]);

                 // TODO: fix the broken Vote Game

               
                 

 
                            
                 
                }

              


                } 

                public static void SpawnTHEM(Player p) {
                 Thread.Sleep(100); // delay to not spawn in wrong block
                 p.level.spawnx = (ushort)p.Pos.BlockX;
                 p.level.spawny = (ushort)p.Pos.BlockY;
                 p.level.spawnz = (ushort)p.Pos.BlockZ;
                 p.level.rotx = p.Rot.RotY; p.level.roty = p.Rot.HeadX;


                }
    public class CmdParkour : Command2
    {
        public override string name { get { return "Parkour"; } }
        public override string type { get { return CommandTypes.Economy; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        
        public override void Use(Player p, string message, CommandData data)
        {

          string[] args = message.SplitSpaces();
          if (args[0] == "start") { StartGame(p); }
          else if (args[0] == "stop") { StopGame(p); }
          else if (args[0] == "end") { EndGame(p); }
          else if (args[0] == "checkpoint") { 
           ar = args[1];
           SetupCheckpoint(p);
          }
          
          else if (args[0] == secretcode) { p.lastCMD = "secret"; WinGame(p); }
          else { Help(p); }
 
          
        }

        public override void Help(Player p)
        {
            p.Message("%T/Parkour %e- Parkour MiniGame");
            p.Message("%a/Parkour Start %e- Starts " + GameName + " MiniGame.");
            p.Message("%a/Parkour End %e- Ends Round of " + GameName + " MiniGame.");
            p.Message("%a/Parkour Stop %e- Stops " + GameName + " MiniGame.");
        }
    }
		// Displays help for or information about this plugin
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
	}
}
