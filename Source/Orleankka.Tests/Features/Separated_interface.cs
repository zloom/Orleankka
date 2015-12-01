using NUnit.Framework;

namespace Orleankka.Features
{
    namespace Separated_interface
    {
        using Meta;
        using Testing;

        [TestFixture]
        [RequiresSilo]
        public class Tests
        {
            IActorSystem system;

            [SetUp]
            public void SetUp()
            {
                system = TestActorSystem.Instance;
            }

            [Test]
            public async void Can_refer_to_actor_by_virtue_of_interface()
            {
                var actor = system.FreshActorOf<ITestSeparatedInterfaceActor>();

                await actor.Tell(new SetText {Text = "c-a"});
                Assert.AreEqual("c-a", await actor.Ask(new GetText()));
            }
        }
    }
}