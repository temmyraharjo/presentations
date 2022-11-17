using Microsoft.Xrm.Sdk;

namespace Lib.Core.Rounding
{
    public static class LocalPluginContextExtensions
    {
        public static void Rounding(this ILocalPluginContext context)
        {
            if (!context.Feature.IsRounding) return;
            var target = context.GetTarget<Entity>();

            target.SanitizeMoney(context.Feature);
        }
    }
}