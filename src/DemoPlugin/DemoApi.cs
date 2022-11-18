using Lib.Core;
using Lib.Features;

namespace DemoPlugin
{
    public class DemoApi : PluginBase
    {
       
        public DemoApi() : base(typeof(DemoApi))
        {
        }

        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
           new DemoApiBusiness(localPluginContext).Execute();
        }
    }
}
