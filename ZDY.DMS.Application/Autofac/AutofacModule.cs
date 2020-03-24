using Autofac;
using ZDY.DMS.Application.Implementation;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Application.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //工作流模块
            //builder.RegisterType<WorkFlowRunService>().As<IWorkFlowRunService>();

            //builder.RegisterType<StockStatisticService>().As<IStockStatisticService>();
        }
    }
}
