using Microsoft.Xrm.Sdk;
using System;
using Lib.Core;
using Lib.Entities;
using Newtonsoft.Json;

namespace DemoPlugin
{
    public class DemoApi : PluginBase
    {
        public class Input
        {
            public string Name { get; set; }
            public decimal? Discount { get; set; }
            public int Qty { get; set; }
            public decimal PricePerUnit { get; set; }
        }

        public class Output
        {
            public Guid? Id { get; set; }
            public string LogicalName { get; set; }
            public string Error { get; set; }
        }
        public DemoApi() : base(typeof(DemoApi))
        {
        }

        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            var output = new Output();
            try
            {
                localPluginContext.Trace("Hello world from Custom API!");

                var input = JsonConvert.DeserializeObject<Input>(localPluginContext.PluginExecutionContext
                    .InputParameters[nameof(Input)].ToString());
                if (input == null) throw new ArgumentNullException(nameof(Input));

                var calculation = new dev_Calculation
                {
                    dev_Number = input.Name,
                    dev_PricePerUnit = new Money(input.PricePerUnit),
                    dev_Discount = input.Discount.HasValue ? new Money(input.Discount.Value) : null,
                    dev_Qty = input.Qty
                };

                output.Id = localPluginContext.InitiatingUserService.Create(calculation.ToEntity<Entity>());
                output.LogicalName = dev_Calculation.EntityLogicalName;
            }
            catch (Exception ex)
            {
                output.Error = ex.Message;
            }
            finally
            {
                localPluginContext.PluginExecutionContext.OutputParameters[nameof(Output)] = 
                    JsonConvert.SerializeObject(output);
            }
        }
    }
}
