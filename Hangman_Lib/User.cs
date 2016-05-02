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
    public class User
    {
        [ColumnAttribute(AutoSync = AutoSync.OnInsert,
        DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id;

        [Column]
        public string Name;

        [Column]
        public string Password;


        private EntitySet<Score> _scores = new EntitySet<Score>();

        [Association(Storage = "_scores", OtherKey = "UserId", ThisKey = "Id")]
        public ICollection<Score> scores
        {
            get { return _scores; }
            set { _scores.Assign(value); }
        }
    }
}
