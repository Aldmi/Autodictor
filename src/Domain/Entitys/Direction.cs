using System.Collections.Generic;

namespace ConsoleApplication_test.Entitys
{
    public class Direction : EntityBase
    {
        public string Name { get; set; }
        public List<Station> Stations { get; set; }
    }
}