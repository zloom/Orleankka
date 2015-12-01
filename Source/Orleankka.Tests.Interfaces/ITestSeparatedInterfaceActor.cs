using System;

namespace Orleankka
{
    using Meta;

    [Serializable]
    public class SetText : Command
    {
        public string Text;
    }

    [Serializable]
    public class GetText : Query<string>
    {}

    public interface ITestSeparatedInterfaceActor : IActor
    {}
}
