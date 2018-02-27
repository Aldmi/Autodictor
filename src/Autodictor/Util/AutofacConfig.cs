using System.Reflection;
using Autofac;
using Domain.Abstract;

namespace MainExample.Util
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            //// получаем экземпляр контейнера
            //var builder = new ContainerBuilder();

            //// регистрируем споставление типов
            //builder.RegisterType<BookRepository>().As<IRepository>();

            //// создаем новый контейнер с теми зависимостями, которые определены выше
            //var container = builder.Build();

            //// установка сопоставителя зависимостей
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}