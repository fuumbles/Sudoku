using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangman_Lib;
namespace SudokuBiz
{
    public class Biz
    {
        DBAccess db = new DBAccess();

       
        //Most of these methods are named in a way to show what they're doing
        
        
        //Login + checking for user
        public string[,] GetUserByUsernameAndPassword(string UserName, string Password)
        {
            string[,] UserLogin = new string[1,2];
            UserLogin = db.GetUserByUsernameAndPassword(UserName, Password);
            return UserLogin;
        }

        //Get Puzzle Complete + Puzzle Incomplete strs
        public string[,] GetPuzzle(int DifficultySelection)
        {
            List<int> PuzzleIds = new List<int>();
            string[,] PuzzleStr = new string[1,3];//for housing solution & puzzle
            string Difficulty = "";
            if(DifficultySelection == 1)
            {
                Difficulty = "Medium";
            }
            else if(DifficultySelection == 0)
            {
                Difficulty = "Easy";
            }
            PuzzleIds = db.GetPuzzleIDsByDifficulty(Difficulty);
            PuzzleIds.Shuffle();
            PuzzleStr = db.GetPuzzleByID(PuzzleIds[1]);//gets the second ID from the shuffled list for a random sudoku
            PuzzleStr[0, 2] = PuzzleIds[1].ToString();
            return PuzzleStr;
        }

        public int SubmitUserScore(string Username, string Time, int PuzzleID)
        {
            int UserID = db.GetUserIDByUsername(Username);

            return InsertNewScoreAndCheckHighscores(UserID, PuzzleID, Time, Username);

        }

        //new user
        public bool CreateUser(string UserName, string Password)
        { 
            if(db.CreateNewUser(UserName, Password))
            {
                return true;
            }
            return false;
        }

        public int InsertNewScoreAndCheckHighscores(int UserID, int PuzzleID, string Time, string Username)
        {            
            return db.InsertPlayerScore(UserID, PuzzleID, Time, Username);              
        }

        public List<Highscores> GetAllHighScores()
        {
            List<Highscores> HighScores = db.GetAllHighScores();
            return HighScores;
        }
        
    }


}
