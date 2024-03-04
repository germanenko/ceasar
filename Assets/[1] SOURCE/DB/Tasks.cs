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


    public class Boards
    {
        [PrimaryKey]
        public string Id { get; set; }

        [Default(0), NotNull]
        public string Name { get; set; }
    }



    public class Tasks
    {
        [PrimaryKey]
        public string Id { get; set; }

        [Default("'qwerty'"), NotNull]
        public string BoardId { get; set; }

        [Default(0), NotNull]
        public DateTime CreatedAtDate { get; set; }

        [Default("'qwerty'"), NotNull] 
        public string CreatorId { get; set; }

        [Default("'qwerty'"), NotNull]
        public string Description { get; set; }

        [Default("'qwerty'"), NotNull]
        public string DraftOfTaskId { get; set; }

        [Default(0), NotNull]
        public DateTime EndDate { get; set; }

        [MaxLength(9), Default("'ffffffff'"), NotNull]
        public string HexColor { get; set; }

        [Default(0), NotNull]
        public int IsDraft { get; set; }

        [MaxLength(60), Default("'---'"), NotNull]
        public int PriorityOrder { get; set; }

        [Default(0), NotNull]
        public DateTime StartDate { get; set; }

        [MaxLength(60), Default("'---'"), NotNull]
        public string Status { get; set; }

        [Default("'qwerty'"), NotNull]
        public string Title { get; set; }

        [Default("'qwerty'"), NotNull]
        public int Type { get; set; }
    }



    public class SavesAndDrafts
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Default(0), NotNull]
        public string TaskID { get; set; }

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

        [Default("Personal"), MaxLength(300), NotNull]
        public int UnreadMessagesCount { get; set; }

        [Default("link"), MaxLength(300)]
        public string Image { get; set; }
    }


    [Serializable]
    public class ChatMessages
    {
        [PrimaryKey]
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
