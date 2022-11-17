using System;
using Lib.Core.FeatureFlags;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Core.Rounding
{
    public class RoundingService : IOrganizationService
    {
        private readonly IOrganizationService _service;
        private readonly Feature _feature;

        public RoundingService(IOrganizationService service, Feature feature)
        {
            _service = service;
            _feature = feature;
        }

        public Guid Create(Entity entity)
        {
            entity.SanitizeMoney(_feature);
            return _service.Create(entity.ToEntity<Entity>());
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            var result = _service.Retrieve(entityName, id, columnSet);
            result.SanitizeMoney(_feature);
            return result;
        }

        public void Update(Entity entity)
        {
           entity.SanitizeMoney(_feature);
           _service.Update(entity.ToEntity<Entity>());
        }

        public void Delete(string entityName, Guid id)
        {
            _service.Delete(entityName, id);
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            return _service.Execute(request);
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            _service.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship,
            EntityReferenceCollection relatedEntities)
        {
            _service.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            var result = _service.RetrieveMultiple(query);
            foreach (var resultEntity in result.Entities)
            {
                resultEntity.SanitizeMoney(_feature);
            }

            return result;
        }
    }
}
