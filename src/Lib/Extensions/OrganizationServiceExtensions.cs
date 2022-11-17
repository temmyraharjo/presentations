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
                ColumnSet = new ColumnSet(EnvironmentVariableDefinition.Fields.DefaultValue)
            };
            query.Criteria.AddCondition(EnvironmentVariableDefinition.Fields.SchemaName, 
                ConditionOperator.Equal, environmentVariableName);
            var linkEnvironmentVariableValue = query.AddLink(EnvironmentVariableValue.EntityLogicalName,
                EnvironmentVariableDefinition.Fields.EnvironmentVariableDefinitionId,
                EnvironmentVariableValue.Fields.EnvironmentVariableDefinitionId,
                JoinOperator.LeftOuter);
            linkEnvironmentVariableValue.Columns = new ColumnSet(EnvironmentVariableValue.Fields.Value);
            linkEnvironmentVariableValue.EntityAlias = "ev";

            var result = service.RetrieveMultiple(query);

            return result.Entities.FirstOrDefault()?.ToEntity<EnvironmentVariableDefinition>();
        }
    }
}