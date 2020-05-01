namespace SharpOtto.Core.Screen
{
    using System;
    using System.Drawing;

    public class UpdateScreenEventArgs : EventArgs
    {
        public Bitmap Bitmap { get; private set; }

        public UpdateScreenEventArgs(Bitmap bitmap)
        {
            this.Bitmap = bitmap;
        }
    }
}