namespace SharpOtto.Core
{
    using System;
    using System.Drawing;

    public partial class Interpreter : IScreen
    {
        private const byte ScreenWidth = 64;

        private const byte ScreenHeigth = 32;

        private bool[] pixels = new bool[ScreenWidth * ScreenHeigth];

        private bool drawScreen;

        public byte Width { get => ScreenWidth; }

        public byte Heigth { get => ScreenHeigth; }

        public bool[] Pixels { get => this.pixels; }

        public bool DrawScreen { get => this.drawScreen; set => this.drawScreen = value; }

        public Action<object, UpdateScreenEventArgs> OnUpdateScreen { get; set; }

        public void ClearScreen()
        {
            Array.Clear(this.pixels, 0, this.pixels.Length);
            this.drawScreen = true;
        }

        private void UpdateScreen()
        {
            if (!this.drawScreen)
            {
                return;
            }

            var bmp = new Bitmap(ScreenWidth, ScreenHeigth);
            for (var y = 0; y < ScreenHeigth; y++)
            {
                for (var x = 0; x < ScreenWidth; x++)
                {
                    var color = this.pixels[x + y * ScreenWidth] ? Color.White : Color.Black;
                    bmp.SetPixel(x, y, color);
                }
            }

            if (this.OnUpdateScreen != null)
            {
                this.OnUpdateScreen(this, new UpdateScreenEventArgs(bmp));
            }

            this.drawScreen = false;
        }
    }
}