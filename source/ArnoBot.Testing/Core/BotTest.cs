using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ArnoBot.Core;
using ArnoBot.Core.Responses;
using ArnoBot.Interface;

namespace ArnoBot.Testing.Core
{
    [TestFixture]
    public class BotTest
    {
        Bot bot;

        [SetUp]
        public void SetUp()
        {
            bot = Bot.CreateOrGet();
            bot.ModuleRegistry.RegisterModule(new MockModule());
        }

        [TearDown]
        public void TearDown()
        {
            bot.Dispose();
        }

        [Test]
        public void CreateOrGet_ReturnNotNull()
        {
            Bot bot = Bot.CreateOrGet();
            Assert.NotNull(bot);
        }

        [Test]
        public void CreateOrGet_ReturnSameObject()
        {
            Bot botRef1 = Bot.CreateOrGet();
            Bot botRef2 = Bot.CreateOrGet();
            Assert.AreSame(botRef1, botRef2);
        }

        [Test]
        public void GetModuleRegistry_NotNull()
        {
            Assert.NotNull(bot.ModuleRegistry);
        }

        [Test]
        public void CreateOrGet_ReturnNewObject_AfterDispose()
        {
            Bot bot1 = Bot.CreateOrGet();
            bot1.Dispose();
            Bot bot2 = Bot.CreateOrGet();

            Assert.AreNotSame(bot1, bot2);
        }

        [Test]
        public void Query_MockCommand_Input1Parameter_ReturnsSameParametersInResponseBody()
        {
            string input = "mock test";
            string responseBody = bot.Query(input).TextBody;

            Assert.AreEqual(input, responseBody);
        }

        [Test]
        public void Query_MockCommand_Input4Parameters_ReturnsSameParametersInResponseBody()
        {
            string input = "mock this test should succeed!";
            string responseBody = bot.Query(input).TextBody;

            Assert.AreEqual(input, responseBody);
        }

        class MockModule : IModule
        {
            public string Name => "__Mock";

            private Dictionary<string, ICommand> commandRegistry = new Dictionary<string, ICommand>();

            public IReadOnlyDictionary<string, ICommand> CommandRegistry => commandRegistry;

            public MockModule()
            {
                commandRegistry.Add("mock", new MockCommand());
            }

            class MockCommand : ICommand
            {
                public Response Execute(CommandContext context)
                {
                    StringBuilder responseBody = new StringBuilder(context.CommandName + " ");
                    responseBody.AppendJoin(' ', context.Parameters);

                    return new TextResponse(Response.Type.Executed, responseBody.ToString());
                }
            }
        }
    }
}
