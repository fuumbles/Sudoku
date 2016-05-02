using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hangman_Lib
{
    public class DBAccess
    {

        private class DataHelper : DataContext
        {
            // NOTE TO SELF: change the db file location drive letter as needed.
            public DataHelper() :
                base(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\2nd Semester\PROG 2500 C#\Assignments\PROG2500_Hangman\Hangman_Lib\DB\HangmanDB_EN.mdf;Integrated Security=True")
            { }
            public Table<User> users;
            public Table<Sudoku> puzzles;
            public Table<Score> scores;
            public Table<Highscores> hscores;
        }
        private DataHelper data;

        public void DataManager()
        {
            data = new DataHelper();
        }

        public bool InsertUser(User newUser)
        {
            DataManager();
            using (data)
            {
                try
                {
                    data.users.InsertOnSubmit(newUser);
                    data.SubmitChanges();
                }
                catch (Exception e) 
                {
                    Console.WriteLine("Exception: " + e);
                    return false;
                }
                return true;
                
            }
        }
        public bool InsertScore(Score newScore)
        {
            DataManager();
            using (data)
            {
                data.scores.InsertOnSubmit(newScore);
                data.SubmitChanges();
                return true;
            }
        }

        public void InsertHighScore(Highscores newHScore)
        {
            DataManager();
            using (data)
            {
                data.hscores.InsertOnSubmit(newHScore);
                data.SubmitChanges();
                
            }
        }

        public void RemoveHighScore(Highscores oldHScore)
        {
            DataManager();
            using (data)
            {
                data.hscores.Attach(oldHScore);
                data.hscores.DeleteOnSubmit(oldHScore);
                data.SubmitChanges();
            }
        
        }

        //no real use atm - no player submitted sudokus 
        public void InsertSudoku(Sudoku newSudoku)
        {
            DataManager();
            using (data)
            {
                data.puzzles.InsertOnSubmit(newSudoku);
                data.SubmitChanges();
            }
        }




        /// <summary>
        /// Returns an array containing a true/false string, and username
        /// if the requested username & password is found in the database
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string[,] GetUserByUsernameAndPassword(string Username, string Password)
        {
            string[,] UserLoginArr = new string[1,2];
            DataManager();
            using(data)
            {
                var UserChk = (from user in data.users
                              where user.Name == Username && user.Password == Password
                              select user.Name).SingleOrDefault();

                UserLoginArr[0,0] = UserChk;
                if(UserLoginArr[0,0] != null)
                {
                    UserLoginArr[0, 1] = "True";
                }
                else
                {
                    UserLoginArr[0, 1] = "False";
                }

            }
            return UserLoginArr;
        }

        public bool CreateNewUser(string Username, string Password) 
        {
            string UserNameChk;
            DataManager();
            using (data)
            {
                var UserChk = (from user in data.users
                               where user.Name == Username
                               select user.Name).SingleOrDefault();
                UserNameChk = UserChk;
                if (UserNameChk != null)
                {
                    return false; 
                }
                else if (UserNameChk == null)
                {
                    User newUser = new User
                    {
                        Id = -55, // dummy data
                        Name = Username,
                        Password = Password
                    };
                    if (InsertUser(newUser))
                        return true;
                    else
                        return false;
                }
            }
            
            return false; 
        }

        public int GetUserIDByUsername(string Username)
        {
            int UserId = 0;
            DataManager();
            using (data) 
            { 
                var uid= (from i in data.users
                          where i.Name == Username
                          select i.Id).SingleOrDefault();
                if (uid != 0)
                {
                    UserId = uid;
                }
                else
                {
                    return -1;
                }
            }
            return UserId;

        
        }

        public List<int> GetPuzzleIDsByDifficulty(string Difficulty)
        {
            List<int> PuzzleIDs = new List<int>();
            DataManager();
            using (data)
            { 
                var ids = from id in data.puzzles
                          where id.Difficulty == Difficulty
                          select id.Id;
                foreach (var id in ids)
                {
                    PuzzleIDs.Add(id);
                }
            }
            return PuzzleIDs;
        }

        public string[,] GetPuzzleByID(int ID)
        {
            string[,] PuzzleStr = new string[1, 3];
            DataManager();
            using (data) 
            {
                var puzC = (from p in data.puzzles
                            where p.Id == ID
                            select p.CompleteString).SingleOrDefault();
                PuzzleStr[0, 1] = puzC;
                var puzI = (from i in data.puzzles
                            where i.Id == ID
                            select i.IncompleteString).SingleOrDefault();
                PuzzleStr[0, 0] = puzI;
            }
            return PuzzleStr;

        }

        public void RemoveUserFromHighscoresByName(string Name)
        {
            Highscores Removable;
            DataManager();
            using (data)
            {
                var uname = (from u in data.hscores
                            where u.Username == Name
                            select u).SingleOrDefault();
                Removable = (Highscores)uname;
            }
            RemoveHighScore(Removable);
        }

        public int InsertPlayerScore(int UserID, int PuzzleID, string Time, string Username)
        {
            DataManager();
            int returner = 0;//the value for returning depending on if the score was submitted or not, or was a high score
            //1 - score submitted, 2 - highscore, 0 - error
            using (data) 
            {

                List<Highscores> AllHighScores = new List<Highscores>();

                
                var HScores = from h in data.hscores
                              select h;
                foreach (var score in HScores)
                {
                    AllHighScores.Add(score);
                }

                List<string> AllTimes = AllHighScores.Select(o => o.Time).ToList();
                List<string> AllNames = AllHighScores.Select(o => o.Username).ToList();


                //empty db and limiting to 5 top scores
                if (AllHighScores.Capacity < 5)
                {
                    Highscores nHighScore = new Highscores
                    {
                        Id = -55, //dummy data
                        Time = Time,
                        Username = Username
                    };
                    InsertHighScore(nHighScore);
                    returner = 2;
                }
                else 
                {
                    //else check all the 5 highscores
                    if (Convert.ToDateTime(Time) < Convert.ToDateTime(AllTimes[0]))
                    {

                        RemoveUserFromHighscoresByName(AllNames[4]);
                        Highscores nHighScore = new Highscores 
                        {
                            Id = -55, //dummy data
                            Time = Time,
                            Username = Username
                        };
                        InsertHighScore(nHighScore);
                        returner = 2;
                    }
                    else if (Convert.ToDateTime(Time) < Convert.ToDateTime(AllTimes[1]))
                    {
                        RemoveUserFromHighscoresByName(AllNames[4]);
                        Highscores nHighScore = new Highscores
                        {
                            Id = -55, //dummy data
                            Time = Time,
                            Username = Username
                        };
                        InsertHighScore(nHighScore);
                        returner = 2;
                    }
                    else if (Convert.ToDateTime(Time) < Convert.ToDateTime(AllTimes[2]))
                    {
                        RemoveUserFromHighscoresByName(AllNames[4]);
                        Highscores nHighScore = new Highscores
                        {
                            Id = -55, //dummy data
                            Time = Time,
                            Username = Username
                        };
                        InsertHighScore(nHighScore);
                        returner = 2;
                    }
                    else if (Convert.ToDateTime(Time) < Convert.ToDateTime(AllTimes[3]))
                    {
                        RemoveUserFromHighscoresByName(AllNames[4]);
                        Highscores nHighScore = new Highscores
                        {
                            Id = -55, //dummy data
                            Time = Time,
                            Username = Username
                        };
                        InsertHighScore(nHighScore);
                        returner = 2;
                    }
                    else if (Convert.ToDateTime(Time) < Convert.ToDateTime(AllTimes[4]))
                    {
                        RemoveUserFromHighscoresByName(AllNames[4]);                        
                        Highscores nHighScore = new Highscores
                        {
                            Id = -55, //dummy data
                            Time = Time,
                            Username = Username
                        };
                        InsertHighScore(nHighScore);
                        returner = 2;
                    }

                }
                Score newScore = new Score
                {
                    Id = -55,//dummy data
                    Time = Time,
                    UserId = UserID,
                    PuzzleId = PuzzleID
                };
                if (InsertScore(newScore))
                {
                    returner = 1;//score submitted
                }
            }
            return returner;//no score submitted
        }

        public List<Highscores> GetAllHighScores()
        {
            List<Highscores> Highscores = new List<Highscores>();
            DataManager();
            using (data) 
            {
                var scores = from s in data.hscores
                             select s;
                foreach (var score in scores)
                {
                    Highscores.Add(score);
                }
            }

            return Highscores;
        }
    }

}
