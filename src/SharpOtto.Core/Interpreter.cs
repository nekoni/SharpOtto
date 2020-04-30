namespace SharpOtto.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SharpOtto.Core.Input;
    using SharpOtto.Core.Screen;

    public class Interpreter
    {
        private byte[] memory = new byte[4096];

        private byte[] v = new byte[16];

        private ushort index;

        private ushort pc;

        private byte[] screen = new byte[64 * 32];

        private byte delayTimer;

        private byte soundTimer;

        private Stack<ushort> stack = new Stack<ushort>();

        private bool[] keypad = new bool[16];

        private Random random = new Random(DateTime.Now.Millisecond);

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

        public Action<object, EventArgs> OnCyclesCompleted { get; set; }

        public void KeyDown(KeypadKey keyboardKey)
        {
            this.keypad[keyboardKey.Value] = true;
        }

        public void KeyUp(KeypadKey keyboardKey)
        {
            this.keypad[keyboardKey.Value] = false;
        }

        public void LoadGame(byte[] game)
        {
            watch.Reset();
            watch.Start();

            Array.Clear(this.memory, 0, this.memory.Length);
            Array.Copy(this.fontset, 0, this.memory, 0, this.fontset.Length);
            Array.Clear(this.screen, 0, this.screen.Length);
            Array.Copy(game, 0, this.memory, 512, game.Length);

            this.pc = 512;
            while (true)
            {
                var cycles = this.opsTimer.GetCycles();
                for (var cycle = 0; cycle < cycles; cycle++)
                {
                    var opcode = (ushort)(memory[this.pc] << 8 | memory[this.pc + 1]);

                    this.HandleOpCode(opcode);

                    this.pc += 2;
                }

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

                if(this.OnCyclesCompleted != null)
                {
                    this.OnCyclesCompleted(this, new EventArgs());
                }
            }
        }

        private void HandleOpCode(ushort opcode)
        {
            var x = (byte)((opcode & 0x0F00) >> 8);
            var y = (byte)((opcode & 0x00F0) >> 4);
            var k = (byte)(opcode & 0x00FF);

            if (opcode == 0x00E0) // CLS
            {
                Array.Clear(this.screen, 0, this.screen.Length);
                return;
            }

            if (opcode == 0x00EE) // RET
            {
                this.pc = this.stack.Pop();
                return;
            }

            var op = opcode & 0xF000;
            if (op == 0x1000) // JP addr
            {
                this.pc = (ushort)(opcode & 0x0FFF);
                this.pc -= 2;
                return;
            }

            if (op == 0x2000) // CALL addr
            {
                this.stack.Push(this.pc);
                this.pc = (ushort)(opcode & 0x0FFF);
                this.pc -= 2;
                return;
            }

            if (op == 0x3000) // SE Vx, byte
            {
                if (this.v[x] == k)
                {
                    this.pc += 2;
                }

                return;
            }

            if (op == 0x4000) // SNE Vx, byte
            {
                if (this.v[x] != k)
                {
                    this.pc += 2;
                }

                return;
            }

            if (op == 0x5000) // SE Vx, Vy
            {
                if (this.v[x] == this.v[y])
                {
                    this.pc += 2;
                }

                return;
            }

            if (op == 0x6000) // LD Vx, byte
            {
                this.v[x] = k;
                return;
            }

            if (op == 0x7000) // ADD Vx, byte
            {
                this.v[x] += k;
                return;
            }

            if (op == 0x8000)
            {
                var o = opcode & 0x000F;
                switch (o)
                {
                    case 0: // LD Vx, Vy
                    {
                        this.v[x] = this.v[y];
                        return;
                    }

                    case 1: // OR Vx, Vy
                    {
                        this.v[x] |= this.v[y];
                        return;
                    }

                    case 2: // AND Vx, Vy
                    {
                        this.v[x] &= this.v[y];
                        return;
                    }

                    case 3: // XOR Vx, Vy
                    {
                        this.v[x] ^= this.v[y];
                        return;
                    }

                    case 4: // ADD Vx, Vy
                    {
                        var res = this.v[x] + this.v[y];
                        this.v[15] = (byte) (res > 255 ? 1 : 0);
                        this.v[x] = (byte) res;
                        return;
                    }

                    case 5: // SUB Vx, Vy
                    {
                        this.v[15] = (byte) (this.v[y] > this.v[y] ? 1 : 0);
                        this.v[x] -= this.v[y];
                        return;
                    }

                    case 6: // SHR Vx {, Vy}
                    {
                        this.v[15] = (byte) (this.v[x] & 1);
                        this.v[x] /= 2;
                        return;
                    }

                    case 7: // SUBN Vx, Vy
                    {
                        this.v[15] = (byte) (this.v[y] > this.v[y] ? 1 : 0);
                        this.v[x] = (byte) (this.v[y] - this.v[x]);
                        return;
                    }

                    case 0xE: // SHL Vx {, Vy}
                    {
                        this.v[15] = (byte) ((this.v[x] >> 7) & 1);
                        this.v[x] *= 2;
                        return;
                    }
                }
            }

            if (op == 0x9000) // SNE Vx, Vy
            {
                if (this.v[x] != this.v[y])
                {
                    this.pc += 2;
                }

                return;
            }

            if (op == 0xA000) // LD I, addr
            {
                this.index = (ushort)(opcode & 0x0FFF);
                this.pc += 2;
                return;
            }

            if (op == 0xB000) // JP V0, addr
            {
                this.pc = (ushort)((opcode & 0x0FFF) + this.v[0]);
                return;
            }

            if (op == 0xC000) // RND Vx, byte
            {
                this.v[x] = (byte)(this.random.Next(0, 255) & k);
                return;
            }

            if (op == 0xD000) // RND Vx, byte
            {
                var n = (byte)(opcode & 0x000F);
                return;
            }

            if (op == 0xE000)
            {
                var o = opcode & 0x00FF;
                switch (o)
                {
                    case 0x009E: // SKP Vx
                    {
                        if (this.keypad[x])
                        {
                            this.pc += 2;
                        }
                        return;
                    }

                    case 0x00A1: // SKNP Vx
                    {
                        if (!this.keypad[x])
                        {
                            this.pc += 2;
                        }
                        return;
                    }
                }
            }

            if (op == 0xF000)
            {
                var o = opcode & 0x00FF;
                switch (o)
                {
                    case 0x0007: // LD Vx, DT
                    {
                        this.v[x] = this.delayTimer;
                        return;
                    }

                    case 0x000A: // LD Vx, K
                    {
                        var keyPressed = false;
                        for (var i = 0; i < this.keypad.Length; i++)
                        {
                            if (this.keypad[i])
                            {
                                this.v[x] = (byte)i;
                                keyPressed = true;
                                return;
                            }
                        }
                        
                        if (!keyPressed)
                        {
                            this.pc -= 2;
                        }
                        return;
                    }

                    case 0x0015: // LD DT, Vx
                    {
                        this.delayTimer = this.v[x];
                        return;
                    }

                    case 0x0018: // LD ST, Vx
                    {
                        this.soundTimer = this.v[x];
                        return;
                    }

                    case 0x001E: // ADD I, Vx
                    {
                        this.index += this.v[x];
                        return;
                    }

                    case 0x0029: // LD F, Vx
                    {
                        
                        return;
                    }

                    case 0x0033: // LD B, Vx
                    {
                        
                        return;
                    }

                    case 0x0055: // LD [I], Vx
                    {
                        Array.Copy(this.v, 0, this.memory, this.index, x);
                        return;
                    }

                    case 0x0065: // LD Vx, [I]
                    {
                        Array.Copy(this.memory, this.index, this.v, 0, x);
                        return;
                    }
                }
            }

            Console.WriteLine($"Unhandled opcode {opcode}");
        }
    }
}