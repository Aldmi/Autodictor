using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Domain.Abstract;
using Domain.Entitys;

namespace Domain.Concrete
{
    public class RepositoryXmlDirection : IRepository<Direction>
    {
        private readonly XElement _xElement;
        private IEnumerable<Direction> Directions { get; set; }



        public RepositoryXmlDirection(XElement xElement)
        {
            _xElement = xElement;
        }



        public Direction GetById(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Direction> List()
        {
            return Directions ?? (Directions = ParseXmlFile());
        }


        private IEnumerable<Direction> ParseXmlFile()
        {
            var directions = new List<Direction>();
            try
            {
                foreach (var directXml in _xElement.Elements("Direction"))
                {
                    var direct = new Direction
                    {
                        Id = int.Parse((string)directXml.Attribute("Id")),
                        Name = (string)directXml.Attribute("Name"),
                        Stations = new List<Station>()
                    };
                    try
                    {
                        var stations = directXml.Elements("Station").ToList();
                        if (stations.Any())
                        {
                            foreach (var stXml in stations)
                            {
                                direct.Stations.Add(new Station
                                {
                                    Id = int.Parse((string)stXml.Attribute("Id")),
                                    NameRu = (string)stXml.Attribute("NameRu"),
                                    NameEng = (string)stXml.Attribute("NameEng"),
                                    NameCh = (stXml.Attribute("NameCh") != null) ? (string)stXml.Attribute("NameCh") : string.Empty,
                                    CodeEsr = !string.IsNullOrEmpty((string)stXml.Attribute("CodeEsr")) ? int.Parse((string)stXml.Attribute("CodeEsr")) : 0,
                                    CodeExpress = !string.IsNullOrEmpty((string)stXml.Attribute("CodeExpress")) ? int.Parse((string)stXml.Attribute("CodeExpress")) : 0
                                });
                            }
                        }
                    }
                    finally
                    {
                        directions.Add(direct);
                    }
                }
            }
            catch (Exception ex) { }
            return directions;
        }


        public IEnumerable<Direction> List(Expression<Func<Direction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Direction entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Direction entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Direction entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Direction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Direction> entity)
        {
            throw new NotImplementedException();
        }
    }
}