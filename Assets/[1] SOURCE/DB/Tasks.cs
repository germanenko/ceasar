using UnityEngine;
using SimpleSQL;
using System.Collections.Generic;

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

        [MaxLength(9), Default(0), NotNull]
        public bool Draft { get; set; }
    }
    public class TaskSave
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Default(0), NotNull]
        public int TaskID { get; set; }

        [MaxLength(60), Default("'---'"), NotNull]
        public string Name { get; set; }

        [Default(0), NotNull]
        public int Type { get; set; }

        [MaxLength(9), Default("'ffffffff'"), NotNull]
        public string Color { get; set; }

    }
    public class Priority
    {

        [PrimaryKey, AutoIncrement]
        public int PriorityValue { get; set; }

        [Default(0), NotNull]
        public int TaskID { get; set; }
    }
}
