﻿namespace SharpOtto.Wpf
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using SharpOtto.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IInterpreter interpreter = new Interpreter();

        private Dictionary<Key, KeypadKey> keysMap = new Dictionary<Key, KeypadKey>()
        {
            { Key.NumPad0, KeypadKey.Pad0 },
            { Key.NumPad1, KeypadKey.Pad1 },
            { Key.NumPad2, KeypadKey.Pad2 },
            { Key.NumPad3, KeypadKey.Pad3 },
            { Key.NumPad4, KeypadKey.Pad4 },
            { Key.NumPad5, KeypadKey.Pad5 },
            { Key.NumPad6, KeypadKey.Pad6 },
            { Key.NumPad7, KeypadKey.Pad7 },
            { Key.NumPad8, KeypadKey.Pad8 },
            { Key.NumPad9, KeypadKey.Pad9 },
            { Key.Insert, KeypadKey.PadA },
            { Key.Home, KeypadKey.PadB },
            { Key.PageUp, KeypadKey.PadC },
            { Key.Delete, KeypadKey.PadD },
            { Key.End, KeypadKey.PadE },
            { Key.PageDown, KeypadKey.PadF },
        };

        public MainWindow()
        {
            this.InitializeComponent();
            
            this.RegisterKeyboardEvents();
            this.RegisterScreenUpdateEvent();
            this.Loaded += (s, e) => Task.Run(() => this.Run());
            this.Closing += (s, e) => this.interpreter.Exit = true;
        }

        private void Run()
        {
            var assembly = typeof(SharpOtto.Wpf.App).Assembly;
            var resource = assembly.GetManifestResourceStream("SharpOtto.Wpf.roms.tetris.ch8");
            using (var memoryStream = new MemoryStream())
            {
                resource.CopyTo(memoryStream);
                interpreter.Run(memoryStream.ToArray());
            }
        }

        private void RegisterScreenUpdateEvent()
        {
            this.interpreter.OnUpdateScreen += (sender, args) =>
            {
                if (this.interpreter.Exit)
                {
                    return;
                }

                this.Dispatcher.Invoke(() =>
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        args.Bitmap.Save(memoryStream, ImageFormat.Bmp);
                        memoryStream.Position = 0;
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = memoryStream;
                        image.EndInit();
                        this.Screen.Source = image;
                    }
                });
            };
        }

        private void RegisterKeyboardEvents()
        {
            this.KeyDown += (sender, args) =>
            {
                if (keysMap.ContainsKey(args.Key))
                {
                    interpreter.KeyDown(this.keysMap[args.Key]);
                }
            };

            this.KeyUp += (sender, args) => 
            {
                if (keysMap.ContainsKey(args.Key))
                {
                    interpreter.KeyUp(this.keysMap[args.Key]);
                }
            };
        }
    }
}