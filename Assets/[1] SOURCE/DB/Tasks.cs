using UnityEngine;
using SimpleSQL;
using System.Collections.Generic;
using System;
using System.Data.SqlTypes;

namespace Germanenko.Source
{

    public enum TypeOfTasks
    {
        Meeting = 0,
        Task = 1,
        Reminder = 2,
        Information = 3
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

        [Default(0), NotNull]
        public DateTime StartTime { get; set; }

        [Default(0), NotNull]
        public DateTime EndTime { get; set; }
    }



    public class SavesAndDrafts
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Default(0), NotNull]
        public int TaskID { get; set; }

        [MaxLength(9), Default(0), NotNull]
        public bool Draft { get; set; }

        [Default(0), NotNull]
        public int Reference { get; set; }

        [Default(0), NotNull]
        public DateTime Date { get; set; }
    }



    public class DeletedTasks
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Default(0), NotNull]
        public int TaskID { get; set; }

        [Default(0), NotNull]
        public DateTime Date { get; set; }
    }



    public class Priority
    {

        [PrimaryKey, AutoIncrement]
        public int PriorityValue { get; set; }

        [Default(0), NotNull]
        public int TaskID { get; set; }
    }



    public class Chats
    {
        [PrimaryKey, MaxLength(300)]
        public string Id { get; set; }

        [Default("name"), MaxLength(300), NotNull]
        public string Name { get; set; }

        [Default("Personal"), MaxLength(300), NotNull]
        public string Type { get; set; }

        [Default("link"), MaxLength(300)]
        public string Image { get; set; }
    }



    public class ChatMessages
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }

        [Default(0), NotNull]
        public string ChatId { get; set; }

        [Default(0), NotNull]
        public string Content { get; set; }

        [Default(0), NotNull]
        public string SenderId { get; set; }

        [Default(0), NotNull]
        public string SentAt { get; set; }

        [Default(0), NotNull]
        public string Type { get; set; }
    }
}
