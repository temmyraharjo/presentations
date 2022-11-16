using System.Linq;
using Lib.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Extensions
{
    public static class OrganizationServiceExtensions
    {
        public static EnvironmentVariableDefinition GetEnvironmentVariable(this IOrganizationService service,
            string environmentVariableName)
        {
            var query = new QueryExpression(EnvironmentVariableDefinition.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    nameof(EnvironmentVariableDefinition.DefaultValue).ToLower())
            };
            query.Criteria.AddCondition(nameof(EnvironmentVariableDefinition.SchemaName).ToLower(), 
                ConditionOperator.Equal, environmentVariableName);
            var linkEnvironmentVariableValue = query.AddLink(EnvironmentVariableValue.EntityLogicalName,
                nameof(EnvironmentVariableDefinition.EnvironmentVariableDefinitionId).ToLower(),
                nameof(EnvironmentVariableValue.EnvironmentVariableDefinitionId).ToLower(),
                JoinOperator.LeftOuter);
            linkEnvironmentVariableValue.Columns = new ColumnSet(nameof(EnvironmentVariableValue.Value).ToLower());
            linkEnvironmentVariableValue.EntityAlias = "ev";

            var result = service.RetrieveMultiple(query);

            return result.Entities.FirstOrDefault()?.ToEntity<EnvironmentVariableDefinition>();
        }
    }
}