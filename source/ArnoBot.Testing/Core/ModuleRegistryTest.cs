using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ArnoBot.Core;
using ArnoBot.Interface;

namespace ArnoBot.Testing.Core
{
    [TestFixture]
    public class ModuleRegistryTest
    {
        const string MOCK_MODULE_NAME = "__Mock";

        MockModule mockModule;
        ModuleRegistry moduleRegistry;

        [SetUp]
        public void SetUp()
        {
            mockModule = new MockModule();
            moduleRegistry = Bot.CreateOrGet().ModuleRegistry;
        }

        [TearDown]
        public void TearDown()
        {
            Bot.CreateOrGet().Dispose();
        }

        [Test]
        public void RegisterModule_AddsModuleToRegistry()
        {
            moduleRegistry.RegisterModule(mockModule);
            Assert.AreSame(mockModule, moduleRegistry[MOCK_MODULE_NAME]);
        }

        [Test]
        public void RegisterModule_ThrowsDuplicateRegistryException_OnSameNameRegister()
        {
            MockModule secondMockModule = new MockModule();
            moduleRegistry.RegisterModule(mockModule);
            Assert.Throws<DuplicateRegistryException>(() => moduleRegistry.RegisterModule(secondMockModule));
        }

        public class MockModule : IModule
        {
            public string Name { get => MOCK_MODULE_NAME; }

            public IReadOnlyCommandRegistry CommandRegistry => throw new NotImplementedException();
        }
    }
}
