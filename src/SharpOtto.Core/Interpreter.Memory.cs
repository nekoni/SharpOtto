namespace SharpOtto.Core
{
    using System;
    using System.Collections.Generic;

    public partial class Interpreter : IMemory
    {
        private Stack<ushort> stack = new Stack<ushort>();

        private readonly byte[] fontset = new byte[] {
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

        private byte[] memory = new byte[4096];

        public byte[] Memory => this.memory;

        public Stack<ushort> Stack => this.stack;

        private void Load(byte[] data)
        {
            Array.Clear(this.memory, 0, this.memory.Length);
            Array.Copy(this.fontset, 0, this.memory, 0, this.fontset.Length);
            Array.Copy(data, 0, this.memory, 512, data.Length);
            this.stack.Clear();
        }
    }
}