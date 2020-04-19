using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            EMailSettings eMailSettings = new EMailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["EMail.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EMailOrderProcessor>().WithConstructorArgument("settings", eMailSettings);
        }
    }
}