﻿namespace MainExample.Infrastructure
{
    public static class StructCompare
    {
        public static bool SoundRecordComparer (ref SoundRecord sr1, ref SoundRecord sr2)
        {
            return (sr1.ВремяОтправления == sr2.ВремяОтправления) &&
                   (sr1.ВремяПрибытия == sr2.ВремяПрибытия) &&
                   (sr1.ВремяСтоянки == sr2.ВремяСтоянки) &&
                   (sr1.ДниСледования == sr2.ДниСледования) &&
                   (sr1.СтанцияНазначения == sr2.СтанцияНазначения) &&
                   (sr1.СтанцияОтправления == sr2.СтанцияОтправления) &&
                   (sr1.НазваниеПоезда == sr2.НазваниеПоезда) &&
                   (sr1.НомерПоезда == sr2.НомерПоезда) &&
                   (sr1.Примечание == sr2.Примечание);
        }
    }
}