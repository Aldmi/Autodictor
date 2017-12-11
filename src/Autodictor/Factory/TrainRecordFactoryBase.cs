﻿using AutodictorBL.Builder.TrainRecordBuilder;
using AutodictorBL.Entites;
using MainExample.Entites;

namespace MainExample.Factory
{
    public abstract class TrainRecordFactoryBase
    {
        protected readonly TrainRecordBuilderBase Builder;



        protected TrainRecordFactoryBase(TrainRecordBuilderBase builder)
        {
            Builder = builder;
        }


        public abstract TrainTableRecord Construct();
    }
}