using AutodictorBL.Entites;
using AutodictorBL.Rules.TrainRecordRules;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public class TrainRecordBuilderManual : TrainRecordBuilderBase
    {
        private string DaysFollowingFormat { get; }
        private ITrainRecordRule Rule { get; }




        #region ctor

        public TrainRecordBuilderManual(TrainTableRecord trainTableBase, string daysFollowingFormat, ITrainRecordRule rule)
        {
            TrainTableRecord = trainTableBase;
            DaysFollowingFormat = daysFollowingFormat;
            Rule = rule;
        }

        #endregion





        #region Methode

        public override void BuildBase()
        {
        }



        public override void BuildDaysFollowing()
        {
            if(string.IsNullOrEmpty(DaysFollowingFormat))
                return;

            //преобразовать из строки в масив байт.
        }



        public override void BuildSoundTemplateByRules()
        {
            //DEBUG
            var templateStr = @"Начало посадки на пассажирский поезд:10:1:Начало посадки на скорый поезд:15:0";
            TrainTableRecord.SoundTemplates = templateStr;
        }

        #endregion
    }
}