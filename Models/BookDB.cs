﻿using System.ComponentModel.DataAnnotations;

namespace ReadingDiaryApi.Models
{
    public class BookDB
    {
        public string UserID { get; set; }
        public string Title { get; set; }
        public List<string> Author { get; set; }
        public int Page { get; set; }
        public List<string> Genre { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PersonalDescription { get; set; }
        public List<string> PersonalTag { get; set; }
    }
}
