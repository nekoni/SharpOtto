namespace SharpOtto.Core.Screen
{
    using System;

    public class UpdateScreenEventArgs : EventArgs
    {
        public byte[] Buffer { get; private set; }

        public UpdateScreenEventArgs(byte[] buffer)
        {
            this.Buffer = buffer;
        }
    }
}