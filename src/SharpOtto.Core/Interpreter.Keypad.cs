namespace SharpOtto.Core
{
    public partial class Interpreter : IKeypad
    {
        public bool[] Keys { get; } = new bool[16];

        public void KeyDown(KeypadKey key)
        {
            this.Keys[key.Value] = true;
        }

        public void KeyUp(KeypadKey keyboardKey)
        {
            this.Keys[keyboardKey.Value] = false;
        }
    }
}