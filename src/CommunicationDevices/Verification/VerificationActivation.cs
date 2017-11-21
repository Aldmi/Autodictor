using System;
using System.Reactive.Subjects;


namespace CommunicationDevices.Verification
{
    public interface IVerificationActivation
    {
        int GetDeltaDay();
        void ResetActivation(string confirmationPassword);
    }




    public class VerificationActivation : IVerificationActivation
    {

    #region prop

    public DateTime LastActivationDate { get; set; }
    public string Password { get; set; }

    #endregion




    #region ctor

    public VerificationActivation()
    {
        Load();
    }

    #endregion




    #region RxEvent

    public Subject<IVerificationActivation> WarningInvokeRx { get; } = new Subject<IVerificationActivation>(); // предупреждение, передаем дату последней активации
    public Subject<IVerificationActivation> BlockingInvokeRx { get; } = new Subject<IVerificationActivation>(); // блокировка, передаем дату последней активации

    #endregion





    #region Methode

    public void Load()
    {
        //загрузить дату последней активации
        LastActivationDate = new DateTime(2017, 09, 12);
        Password = "123456";
    }


    private void Save()
    {
        //сохранить дату активации
    }


    public int GetDeltaDay()
    {
        return (DateTime.Now - LastActivationDate).Days;
    }


    public void ResetActivation(string confirmationPassword)
    {
        if (Password == confirmationPassword)
        {
            LastActivationDate = DateTime.Now;
            Save();
        }
    }


    /// <summary>
    /// 0...30 дней
    /// Вызвать раз в день
    /// </summary>
    public void CheckActivation_0To30()
    {
        var deltaDay = GetDeltaDay();
        if (deltaDay < 30)
        {
            WarningInvokeRx.OnNext(this);
        }
    }


    /// <summary>
    /// 30...60 дней
    /// раз в 12 часов
    /// </summary>
    public void CheckActivation_30To60()
    {
        var deltaDay = GetDeltaDay();
        if (deltaDay > 30 && deltaDay < 60)
        {
            WarningInvokeRx.OnNext(this);
        }
    }


    /// <summary>
    /// 60...80 дней
    /// раз в 3 часа
    /// </summary>
    public void CheckActivation_60To80()
    {
        var deltaDay = GetDeltaDay();
        if (deltaDay > 60 && deltaDay < 80)
        {
            WarningInvokeRx.OnNext(this);
        }
    }


    /// <summary>
    /// 80...90 дней
    /// раз в час
    /// более 90 дней
    /// блокировка
    /// </summary>
    public void CheckActivation_80To90()
    {
        var deltaDay = GetDeltaDay();
        if (deltaDay > 80 && deltaDay < 90)
        {
            WarningInvokeRx.OnNext(this);
        }
        else if (deltaDay > 90)
        {
            BlockingInvokeRx.OnNext(this);
        }
    }

    #endregion

    }
}