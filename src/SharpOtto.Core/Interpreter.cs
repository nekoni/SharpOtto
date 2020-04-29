namespace SharpOtto.Core
{
    using System;
    using System.Diagnostics;
    using SharpOtto.Core.Input;
    using SharpOtto.Core.Screen;

    public class Interpreter
    {
        private byte[] memory = new byte[4096];

        private byte[] registers = new byte[16];

        private ushort index;

        private ushort pc;

        private byte[] screen = new byte[64 * 32];

        private byte delayTimer;

        private byte soundTimer;

        private ushort[] stack = new ushort[16];

        private ushort sp;

        private byte[] keypad = new byte[16];

        private byte[] fontset = new byte[] {
            0xF0, 0x90, 0x90, 0x90, 0xF0,
            0x20, 0x60, 0x20, 0x20, 0x70,
            0xF0, 0x10, 0xF0, 0x80, 0xF0,
            0xF0, 0x10, 0xF0, 0x10, 0xF0,
            0x90, 0x90, 0xF0, 0x10, 0x10,
            0xF0, 0x80, 0xF0, 0x10, 0xF0,
            0xF0, 0x80, 0xF0, 0x90, 0xF0,
            0xF0, 0x10, 0x20, 0x40, 0x40,
            0xF0, 0x90, 0xF0, 0x90, 0xF0,
            0xF0, 0x90, 0xF0, 0x10, 0xF0,
            0xF0, 0x90, 0xF0, 0x90, 0x90,
            0xE0, 0x90, 0xE0, 0x90, 0xE0,
            0xF0, 0x80, 0x80, 0x80, 0xF0,
            0xE0, 0x90, 0x90, 0x90, 0xE0,
            0xF0, 0x80, 0xF0, 0x80, 0xF0,
            0xF0, 0x80, 0xF0, 0x80, 0x80
        };

        private Stopwatch watch = new Stopwatch();

        private CyclesTimer opsTimer;

        private CyclesTimer delayAndSoundTimer;

        public Interpreter()
        {
            this.opsTimer = new CyclesTimer(this.watch, 500);
            this.delayAndSoundTimer = new CyclesTimer(this.watch, 60);
        }

        public Action<object, UpdateScreenEventArgs> OnUpdateScreen { get; set; }

        public void KeyDown(KeypadKey keyboardKey)
        {
            throw new NotImplementedException();
        }

        public void KeyUp(KeypadKey keyboardKey)
        {
            throw new NotImplementedException();
        }

        public void LoadGame(byte[] game)
        {
            watch.Reset();
            watch.Start();

            Array.Clear(this.memory, 0, this.memory.Length);
            Array.Copy(this.fontset, 0, this.memory, 0, this.fontset.Length);
            Array.Copy(game, 0, this.memory, 512, game.Length);

            this.pc = 512;
            while (true)
            {
                var opCode = (ushort)(memory[pc] << 8 | memory[pc + 1]);

                this.HandleOpCode(opCode);

                var delayAndSoundCycles = this.delayAndSoundTimer.GetCycles();
                if (this.delayTimer > 0)
                {
                    this.delayTimer -= (byte)delayAndSoundCycles;
                }

                if (this.soundTimer > 0)
                {
                    this.soundTimer -= (byte)delayAndSoundCycles;
                    if (this.soundTimer <= 0)
                    {
                        Console.WriteLine("Beep");
                    }
                }
            }
        }

        private void HandleOpCode(ushort opcode)
        {
            var handled = false;
            var op = opcode & 0xF000;
            if (op == 0xA000)
            {
                this.index = (ushort)(opcode & 0x0FFF);
                this.pc += 2;
            }

            if (!handled)
            {
                Console.WriteLine($"Unhandled opcode {opcode}");
            }
        }
    }
}