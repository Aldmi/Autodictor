﻿namespace Domain.Entitys
{
    public class Station : EntityBase
    {
        public string NameRu { get; set; }
        public string NameEng { get; set; }
        public string NameCh { get; set; }
        public int CodeEsr { get; set; }
        public int CodeExpress { get; set; }
    }
}