namespace SharpOtto.Core
{
    using System;

    public interface IScreen
    {
        byte Width { get; }

        byte Heigth { get; }

        bool DrawScreen { get; set; }

        bool[] Pixels { get; }

        void ClearScreen();

        Action<object, UpdateScreenEventArgs> OnUpdateScreen { get; set; }
    }
}