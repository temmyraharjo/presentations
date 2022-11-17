using Lib.Core;
using Lib.Core.Rounding;
using Lib.Entities;
using Microsoft.Xrm.Sdk;

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

    public class SetTotal : BusinessBase<dev_Calculation>
    {
        public SetTotal(ILocalPluginContext context) : base(context)
        {
        }

        public override void HandleExecute()
        {
            var total = Wrapper.Latest.dev_Qty.GetValueOrDefault() * 
                        (Wrapper.Latest.dev_PricePerUnit?.Value ?? 0) - 
                        (Wrapper.Latest.dev_Discount?.Value ?? 0);
            Wrapper.Target.dev_Total = new Money(total);
        }
    }
}