using UnityEngine;
using SimpleSQL;



namespace Germanenko.Source
{

    public enum TypeOfTasks
    {
        Clicker = 0,
        Timer = 1,
        Task = 2
    }



    public class Tasks
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }


        [MaxLength(60), Default("'---'"), NotNull]
        public string Name { get; set; }

        [Default(0), NotNull]
        public int Type { get; set; }

        [MaxLength(9), Default("'ffffffff'"), NotNull]
        public string Color { get; set; }

    }
    public class TaskBase
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }


        [MaxLength(60), Default("'---'"), NotNull]
        public string Name { get; set; }

        [Default(0), NotNull]
        public int Type { get; set; }

        [MaxLength(9), Default("'ffffffff'"), NotNull]
        public string Color { get; set; }

    }

}
