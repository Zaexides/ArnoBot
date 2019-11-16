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

        public Bot Setup()
        {
            return Bot.CreateOrGet();
        }

        [Test]
        public void CreateOrGet_ReturnNotNull()
        {
            Bot bot = Bot.CreateOrGet();
            Assert.NotNull(bot);
        }

    }
}
