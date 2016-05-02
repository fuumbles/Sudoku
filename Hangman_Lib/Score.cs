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
    [Table]
    public class Score
    {
        [ColumnAttribute(AutoSync = AutoSync.OnInsert,
        DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id;

        [Column]
        public string Time;


        private EntityRef<User> _user = new EntityRef<User>();
        [Column]
        public int UserId;
        [Association(IsForeignKey = true, Storage = "_user", ThisKey = "UserId")]
        public User user
        {
            get { return _user.Entity; }
            set { _user.Entity = value; }
        }

        private EntityRef<Sudoku> _sudoku = new EntityRef<Sudoku>();
        [Column]
        public int PuzzleId;
        [Association(IsForeignKey = true, Storage = "_sudoku", ThisKey = "PuzzleId")]
        public Sudoku sudoku
        {
            get { return _sudoku.Entity; }
            set { _sudoku.Entity = value; }
        }

        //private EntitySet<Highscore> _hscores = new EntitySet<Highscore>();

        //[Association(Storage = "_hscores", OtherKey = "ScoreID", ThisKey = "Id")]
        //public ICollection<Highscore> scores
        //{
        //    get { return _hscores; }
        //    set { _hscores.Assign(value); }
        //}

    }
}
