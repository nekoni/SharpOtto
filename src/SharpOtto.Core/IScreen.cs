namespace SharpOtto.Core
{
    internal interface IScreen
    {
        byte Width { get; }

        byte Heigth { get; }

        bool DrawScreen { get; set; }

        bool[] Pixels { get; }

        void ClearScreen();
    }
}