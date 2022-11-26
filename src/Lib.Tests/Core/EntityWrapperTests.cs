using System;
using Lib.Core;
using Lib.Entities;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace Lib.Tests.Core
{
    public class EntityWrapperTests 
    {
        [Fact]
        public void Check_early_bound()
        {
            var contact = new Contact
            {
                FirstName = "FirstName",
                LastName = "LastName",
                JobTitle = "Programmer"
            };

            var parentContact = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());
            var target = new Contact
            {
                Address1_City = "Address City",
                ParentCustomerId = parentContact
            };

            var wrapper = new EntityWrapper<Contact>(contact, target);

            Assert.Equal(contact.FirstName, wrapper.Latest.FirstName);
            Assert.Equal(contact.LastName, wrapper.Latest.LastName);
            Assert.Equal(contact.JobTitle, wrapper.Latest.JobTitle);
            Assert.Equal(target.Address1_City, wrapper.Latest.Address1_City);
            Assert.Equal(target.ParentCustomerId.Id, wrapper.Latest.ParentCustomerId.Id);

            wrapper.Target.FirstName = "New";
            Assert.Equal("New", wrapper.Latest.FirstName);
        }

        [Fact]
        public void Check_late_bound()
        {
            var contact = new Contact
            {
                FirstName = "FirstName",
                LastName = "LastName",
                JobTitle = "Programmer"
            };

            var parentContact = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());
            var target = new Contact
            {
                Address1_City = "Address City",
                ParentCustomerId = parentContact
            };

            var wrapper = new EntityWrapper(contact, target);

            var latest = wrapper.Latest.ToEntity<Contact>();
            Assert.Equal(contact.FirstName, latest.FirstName);
            Assert.Equal(contact.LastName, latest.LastName);
            Assert.Equal(contact.JobTitle, latest.JobTitle);
            Assert.Equal(target.Address1_City, latest.Address1_City);
            Assert.Equal(target.ParentCustomerId.Id, latest.ParentCustomerId.Id);


            wrapper.Target["firstname"] = "New";
            Assert.Equal("New", wrapper.Latest["firstname"]);
        }
    }
}
