using System.Xml.Linq;

namespace AutodictorBL.Settings.XmlSettings
{
    public class XmlTrainTypeRuleSettings
    {
        #region prop

        public int Id { get; }              //Id типа.
        public string Type { get; }         //Принадлежность к типу (дальний/пригород)
        public string NameRu { get; }       //Имя и его Alias
        public string AliasRu { get; }
        public string NameEng { get; }
        public string AliasEng { get; }
        public string NameCh { get; }
        public string AliasCh { get; }

        public string ShowPathTimer { get; }//???
        public int WarningTimer { get; }  //окрашивать в главном окне в жёлтый за X минут до первого события.

        #endregion




        #region ctor


        #endregion




        #region Methode



        #endregion
    }
}