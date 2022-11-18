using Lib.Core;
using Lib.Entities;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;

namespace Lib.Features
{
    public class DemoApiBusiness : BusinessBase<Entity>
    {
        public DemoApiBusiness(ILocalPluginContext context) : base(context)
        {
        }

        public class Input
        {
            public string Name { get; set; }
            public decimal? Discount { get; set; }
            public int Qty { get; set; }
            public decimal PricePerUnit { get; set; }
        }

        public class Output
        {
            public Guid Id { get; set; }
            public string LogicalName { get; set; }
        }
        
        public override void HandleExecute()
        {
            Context.Trace("Hello world from Custom API!");

            var input = JsonConvert.DeserializeObject<Input>(Context.PluginExecutionContext
                .InputParameters[nameof(Input)].ToString());
            if (input == null) throw new ArgumentNullException(nameof(Input));

            var calculation = new dev_Calculation
            {
                dev_Number = input.Name,
                dev_PricePerUnit = new Money(input.PricePerUnit),
                dev_Discount = input.Discount.HasValue ? new Money(input.Discount.Value) : null,
                dev_Qty = input.Qty
            };

            var output = new Output
            {
                Id = Context.InitiatingUserService.Create(calculation.ToEntity<Entity>()),
                LogicalName = dev_Calculation.EntityLogicalName
            };

            Context.PluginExecutionContext.OutputParameters[nameof(Output)] =
                JsonConvert.SerializeObject(output);
        }
    }
}
