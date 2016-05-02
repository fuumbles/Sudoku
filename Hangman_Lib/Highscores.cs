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
    public class Highscores
    {
        [ColumnAttribute(AutoSync = AutoSync.OnInsert,
        DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id;

        [Column]
        public string Time;

        [Column]
        public string Username;


        //private EntityRef<Score> _score = new EntityRef<Score>();
        //[Column]
        //private int PuzzleId;
        //[Association(IsForeignKey = true, Storage = "_score", ThisKey = "ScoreId")]
        //public Score score
        //{
        //    get { return _score.Entity; }
        //    set { _score.Entity = value; }
        //}
    }
}
