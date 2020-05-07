namespace SharpOtto.Core
{
    using System;
    using System.Drawing;

    public partial class Interpreter : IScreen
    {
        private const byte ScreenWidth = 64;

        private const byte ScreenHeigth = 32;

        public byte Width => ScreenWidth;

        public byte Heigth => ScreenHeigth;

        public bool[] Pixels { get; } = new bool[ScreenWidth * ScreenHeigth];

        public bool DrawScreen { get ; set; } = false;

        public Action<object, UpdateScreenEventArgs> OnUpdateScreen { get; set; }

        public void ClearScreen()
        {
            Array.Clear(this.Pixels, 0, this.Pixels.Length);
            this.DrawScreen = true;
        }

        private void UpdateScreen()
        {
            if (!this.DrawScreen)
            {
                return;
            }

            var bmp = new Bitmap(ScreenWidth, ScreenHeigth);
            for (var y = 0; y < ScreenHeigth; y++)
            {
                for (var x = 0; x < ScreenWidth; x++)
                {
                    var color = this.Pixels[x + y * ScreenWidth] ? Color.White : Color.Black;
                    bmp.SetPixel(x, y, color);
                }
            }

            if (this.OnUpdateScreen != null)
            {
                this.OnUpdateScreen(this, new UpdateScreenEventArgs(bmp));
            }

            this.DrawScreen = false;
        }
    }
}