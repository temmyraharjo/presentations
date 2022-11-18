using Lib.Core;
using Lib.Core.Rounding;
using Lib.Features;

namespace DemoPlugin
{
    public class PreOperationCalculation : PluginBase
    {
        public PreOperationCalculation() : base(typeof(PreOperationCalculation))
        {
        }

        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            new SetTotal(localPluginContext).Execute();
            // Rounding the Target
            localPluginContext.Rounding();
        }
    }
}