using CommunicationDevices.DataProviders;
using MainExample.Entites;

namespace MainExample.Builder.TrainRecordBuilder
{
    public abstract class TrainRecordBuilderBase
    {
        protected TrainTableRecord TrainTableRecord;
        public TrainTableRecord GetTrainRec => TrainTableRecord;




        public abstract void BuildBase();
        public abstract void BuildDaysFollowing();
        public abstract void BuildSoundTemplateByRules();
    }
}