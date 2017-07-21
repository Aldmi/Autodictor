using System;

namespace Domain.Entitys
{
    public class SoundRecordChangesDb : EntityBase
    {
        public DateTime TimeStamp { get; set; }        //Время фиксации изменений

        public SoundRecordDb Rec { get; set; }         //До 
        public SoundRecordDb NewRec { get; set; }      //После
    }
}