using System;
using CommunicationDevices.Quartz.Jobs;
using CommunicationDevices.Verification;
using Quartz;
using Quartz.Impl;

namespace CommunicationDevices.Quartz.Shedules
{
    public class QuartzVerificationActivation
    {
        public static void Start(VerificationActivation verificationAct)
        {
            //Планировщик
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();


            //---0....30 дней 1 раз в день--------------------------------------------------------------------------------------------------------
            //Заполнение словаря пользовательских данных
            JobDataMap dataMap = new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_0To30)
            };

            //Создание объекта работы и установка данных для метода Execute
            IJobDetail job = JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job0To30", "group1")                 //идентификатор работы (по нему можно найти работу)
                .SetJobData(dataMap)
                .Build();

            //Создание первого условия сработки
            ITrigger trigger = TriggerBuilder.Create()             // создаем триггер
                .WithIdentity("trigger0To30", "group1")                 // идентифицируем триггер с именем и группой
                .StartAt(DateTimeOffset.Now.AddSeconds(5))             //старт тригера и первый вызов через 5 сек
                .WithSimpleSchedule(x => x                             // далее 5 вызовов с интервалом 5 сек
                    .WithIntervalInSeconds(10)
                    .RepeatForever())                             
                .ForJob(job)
                .Build(); // создаем триггер 

            //Связывание объекта работы с тригером внутри планировщика
            scheduler.ScheduleJob(job, trigger);


            //---30....60 дней 2 раза в день--------------------------------------------------------------------------------------------------------
            dataMap= new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_30To60)
            };

            //Создание объекта работы и установка данных для метода Execute
            job= JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job30To60", "group1")                
                .SetJobData(dataMap)
                .Build();

            trigger= TriggerBuilder.Create()           
                .WithIdentity("trigger30To60", "group1")      
                .StartAt(DateTimeOffset.Now.AddSeconds(5))      
                .WithSimpleSchedule(x => x                          
                    .WithIntervalInSeconds(20)
                    .RepeatForever())
                .ForJob(job)
                .Build(); // создаем триггер 

            scheduler.ScheduleJob(job, trigger);


            //---60....80 дней - раз в 3 часа--------------------------------------------------------------------------------------------------------
            dataMap = new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_60To80)
            };

            //Создание объекта работы и установка данных для метода Execute
            job = JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job60To80", "group1")
                .SetJobData(dataMap)
                .Build();

            trigger = TriggerBuilder.Create()
                .WithIdentity("trigger60To80", "group1")
                .StartAt(DateTimeOffset.Now.AddSeconds(5))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)//.WithIntervalInHours(3)
                    .RepeatForever())
                .ForJob(job)
                .Build(); // создаем триггер 

            scheduler.ScheduleJob(job, trigger);


            //---80....90 дней - раз в 1 час--------------------------------------------------------------------------------------------------------
            dataMap = new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_80To90)
            };

            //Создание объекта работы и установка данных для метода Execute
            job = JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job80To90", "group1")
                .SetJobData(dataMap)
                .Build();

            trigger = TriggerBuilder.Create()
                .WithIdentity("trigger80To90", "group1")
                .StartAt(DateTimeOffset.Now.AddSeconds(5))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(35)//.WithIntervalInHours(1)
                    .RepeatForever())
                .ForJob(job)
                .Build(); // создаем триггер 

            scheduler.ScheduleJob(job, trigger);

            //запуск планировщика
            scheduler.Start();
        }


        public static void Stop()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            JobKey job = new JobKey("Job0To30", "group1");
            scheduler.DeleteJob(job);

            job = new JobKey("Job30To60", "group1");
            scheduler.DeleteJob(job);
        }


        public static void Shutdown()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown(true);
        }
    }
}