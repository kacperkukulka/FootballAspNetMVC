﻿using AWWW_Lab1.Interfaces;

namespace AWWW_Lab1.Models
{
    public class Comment: IModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Article Article { get; set; }
    }
}
