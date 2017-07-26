using System;

namespace MainExample.Entites
{
    public class SoundRecordChanges
    {
        public DateTime TimeStamp { get; set; }       //Время фиксации изменений
        public SoundRecord Rec { get; set; }         //До 
        public SoundRecord NewRec { get; set; }      //После  
    }
}