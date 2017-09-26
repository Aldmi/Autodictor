using System;
using System.Threading.Tasks;

namespace Library.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    bool res = task.Wait(duration);
                    if (res)
                        return task.Result;

                    return default(T);
                }
                catch (Exception ex)
                {
                   // throw ex;
                    return default(T);  //TODO: ???
                }
            });
        }
    }
}