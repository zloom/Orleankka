namespace Orleankka
{
    public class TestSeparatedInterfaceActor : Actor, ITestSeparatedInterfaceActor
    {
        string text = "";

        void On(SetText cmd) => text = cmd.Text;
        string On(GetText q) => text;
    }
}
