using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using MainExample.Mappers;

namespace MainExample.Services.GetDataService
{
    class GetCisRegSh : GetSheduleAbstract
    {
        #region ctor

        public GetCisRegSh(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
            : base(baseGetDataBehavior, soundRecords)
        {

        }

        #endregion




        #region Methode

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        protected override void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data)
        {
            if (!Enable)
                return;

            var universalInputTypes = data as IList<UniversalInputType> ?? data.ToList();
            if (universalInputTypes.Any())
            {
                var tableRecords = new List<TrainTableRecord>();
                foreach (var uit in universalInputTypes)
                {
                   var trTable= Mapper.MapUniversalInputType2TrainTableRecord(uit);
                   tableRecords.Add(trTable);
                }

                TrainTableGrid.СохранитьСписокРегулярноеРасписаниеЦис(tableRecords);
            }
        }

        #endregion
    }
}
