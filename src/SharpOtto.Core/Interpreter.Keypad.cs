namespace SharpOtto.Core
{
    public partial class Interpreter : IKeypad
    {
        private bool[] keys = new bool[16];

        public bool[] Keys => this.keys;

        public void KeyDown(KeypadKey key)
        {
            this.keys[key.Value] = true;
        }

        public void KeyUp(KeypadKey keyboardKey)
        {
            this.keys[keyboardKey.Value] = false;
        }
    }
}