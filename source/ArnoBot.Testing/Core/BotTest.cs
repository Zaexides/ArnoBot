using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ArnoBot.Core;

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
    }
}
