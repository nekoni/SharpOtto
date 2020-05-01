namespace SharpOtto.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;

    using SharpOtto.Core.Input;
    using SharpOtto.Core.Screen;

    public class Interpreter
    {
        private byte[] memory = new byte[4096];

        private byte[] v = new byte[16];

        private ushort index;

        private ushort pc;

        private bool[] screen = new bool[64 * 32];

        private bool drawScreen = false;

        private sbyte delayTimer;

        private sbyte soundTimer;

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
            this.opsTimer = new CyclesTimer(this.watch, 1000);
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
                var delayAndSoundCycles = this.delayAndSoundTimer.GetCycles();
                if (this.delayTimer > 0)
                {
                    this.delayTimer -= (sbyte)delayAndSoundCycles;
                }
                else
                {
                    this.delayTimer = 0;
                }

                if (this.soundTimer > 0)
                {
                    this.soundTimer -= (sbyte)delayAndSoundCycles;
                    if (this.soundTimer <= 0)
                    {
                        Console.WriteLine("Beep");
                    }
                }
                else
                {
                    this.soundTimer = 0;
                }

                var cycles = this.opsTimer.GetCycles();
                for (var cycle = 0; cycle < cycles; cycle++)
                {
                    var opcode = (ushort)(memory[this.pc] << 8 | memory[this.pc + 1]);
                    this.HandleOpCode(opcode);
                    this.pc += 2;
                }

                if (this.OnUpdateScreen != null && this.drawScreen)
                {
                    var bmp = new Bitmap(64, 32);
                    for (var y = 0; y < 32; y++)
                    {
                        for (var x = 0; x < 64; x++)
                        {
                            var color = this.screen[x + y * 64] ? Color.White : Color.Black;
                            bmp.SetPixel(x, y, color);
                        }
                    }

                    this.OnUpdateScreen(this, new UpdateScreenEventArgs(bmp));
                    this.drawScreen = false;
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
                this.drawScreen = true;
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
                        this.v[15] = (byte) (this.v[x] > this.v[y] ? 1 : 0);
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
                        this.v[15] = (byte) (this.v[y] > this.v[x] ? 1 : 0);
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

            if (op == 0xD000) // DRW Vx, Vy, nibble
            {
                var n = (byte)(opcode & 0x000F);
                var sprite = new byte[n];
                Array.Copy(this.memory, this.index, sprite, 0, n);
                var bsprite = new List<bool>();
                for (var z = 0; z < n; z++)
                {
                    var tmp = new BitArray(new byte[] { sprite[z] });
                    var tmp1 = new List<bool>();
                    foreach (var bit in tmp)
                    {
                        tmp1.Add((bool)bit);
                    }

                    tmp1.Reverse();
                    bsprite.AddRange(tmp1);
                }

                var erase = false;
                for (var j = 0; j < n; j++)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        var w = i + this.v[x] + (this.v[y] + j) * 64;
                        var u = i + j * 8;

                        if (w < this.screen.Length)
                        {
                            if (this.screen[w] && bsprite[u])
                            {
                                erase = true;
                            }

                            this.screen[w] ^= bsprite[u];
                        }
                    }
                }
                this.v[15] = (byte)(erase ? 1 : 0);
                this.drawScreen = true;
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
                        this.v[x] = (byte)this.delayTimer;
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
                        this.delayTimer = (sbyte)this.v[x];
                        return;
                    }

                    case 0x0018: // LD ST, Vx
                    {
                        this.soundTimer = (sbyte)this.v[x];
                        return;
                    }

                    case 0x001E: // ADD I, Vx
                    {
                        this.index += this.v[x];
                        return;
                    }

                    case 0x0029: // LD F, Vx
                    {
                        this.index = (ushort)(this.v[x] * 4);
                        return;
                    }

                    case 0x0033: // LD B, Vx
                    {
                        var val = this.v[x].ToString("000");
                        this.memory[this.index] = byte.Parse(val[0].ToString());
                        this.memory[this.index+1] = byte.Parse(val[1].ToString());
                        this.memory[this.index+2] = byte.Parse(val[2].ToString());
                        return;
                    }

                    case 0x0055: // LD [I], Vx
                    {
                        Array.Copy(this.v, 0, this.memory, this.index, x+1);
                        return;
                    }

                    case 0x0065: // LD Vx, [I]
                    {
                        Array.Copy(this.memory, this.index, this.v, 0, x+1);
                        return;
                    }
                }
            }

            Console.WriteLine($"Unhandled opcode {opcode}");
        }
    }
}