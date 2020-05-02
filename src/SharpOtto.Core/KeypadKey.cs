namespace SharpOtto.Core
{
    public sealed class KeypadKey
    {
        public static KeypadKey Pad0 = new KeypadKey(0x00);

        public static KeypadKey Pad1 = new KeypadKey(0x01);

        public static KeypadKey Pad2 = new KeypadKey(0x02);

        public static KeypadKey Pad3 = new KeypadKey(0x03);

        public static KeypadKey Pad4 = new KeypadKey(0x04);

        public static KeypadKey Pad5 = new KeypadKey(0x05);

        public static KeypadKey Pad6 = new KeypadKey(0x06);

        public static KeypadKey Pad7 = new KeypadKey(0x07);

        public static KeypadKey Pad8 = new KeypadKey(0x08);

        public static KeypadKey Pad9 = new KeypadKey(0x09);

        public static KeypadKey PadA = new KeypadKey(0x0A);

        public static KeypadKey PadB = new KeypadKey(0x0B);

        public static KeypadKey PadC = new KeypadKey(0x0C);

        public static KeypadKey PadD = new KeypadKey(0x0D);

        public static KeypadKey PadE = new KeypadKey(0x0E);

        public static KeypadKey PadF  = new KeypadKey(0x0F);

        public byte Value { get; private set; }

        public KeypadKey(byte value)
        {
            this.Value = value;
        }
    }
}