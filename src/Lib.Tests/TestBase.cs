using System.Reflection;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using Lib.Entities;
using Microsoft.Xrm.Sdk;

namespace Lib.Tests
{
    public class TestBase
    {
        public IXrmFakedContext InitializeFakedContext(params Entity[] entities)
        {
            var context = MiddlewareBuilder
                .New()
                .AddCrud()
                .UseCrud()
                .SetLicense(FakeXrmEasyLicense.NonCommercial)
                .Build();
            context.EnableProxyTypes(Assembly.GetAssembly(typeof(Contact)));
            context.Initialize(entities);

            return context;
        }
    }
}