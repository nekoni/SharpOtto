namespace SharpOtto.Core.Input
{
    public sealed class KeypadKey
    {
        public static KeypadKey Pad0 = new KeypadKey("0");

        public static KeypadKey Pad1 = new KeypadKey("1");

        public static KeypadKey Pad2 = new KeypadKey("2");

        public static KeypadKey Pad3 = new KeypadKey("3");

        public static KeypadKey Pad4 = new KeypadKey("4");

        public static KeypadKey Pad5 = new KeypadKey("5");

        public static KeypadKey Pad6 = new KeypadKey("6");

        public static KeypadKey Pad7 = new KeypadKey("7");

        public static KeypadKey Pad8 = new KeypadKey("8");

        public static KeypadKey Pad9 = new KeypadKey("9");

        public static KeypadKey PadA = new KeypadKey("A");

        public static KeypadKey PadB = new KeypadKey("B");

        public static KeypadKey PadC = new KeypadKey("C");

        public static KeypadKey PadD = new KeypadKey("D");

        public static KeypadKey PadE = new KeypadKey("E");

        public static KeypadKey PadF  = new KeypadKey("F");

        public string Symbol { get; private set; }

        public KeypadKey(string symbol)
        {
            this.Symbol = symbol;
        }
    }
}