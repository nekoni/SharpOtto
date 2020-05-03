namespace SharpOtto.Core
{
    public interface IKeypad
    {
        bool[] Keys { get; }

        void KeyDown(KeypadKey key);

        void KeyUp(KeypadKey key);
    }
}