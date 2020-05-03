namespace SharpOtto.Core.Opcodes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Dxyn - DRW Vx, Vy, nibble
    /// Display n-byte sprite starting at memory location I at (Vx, Vy), set VF = collision.
    /// The interpreter reads n bytes from memory, starting at the address stored in I.
    /// These bytes are then displayed as sprites on screen at coordinates (Vx, Vy).
    /// Sprites are XORed onto the existing screen. If this causes any pixels to be erased, VF is set to 1, otherwise it is set to 0.
    /// If the sprite is positioned so part of it is outside the coordinates of the display,
    /// it wraps around to the opposite side of the screen. See instruction 8xy3 for more information on XOR, and section 2.4,
    /// Display, for more information on the Chip-8 screen and sprites.
    /// </summary>
    internal class DrwVxVyNibbleOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xD000)
            {
                var bsprite = this.GetSprite(o);
                var erase = false;
                for (var j = 0; j < o; j++)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        var w = i + this.Interpreter.V[x] + (this.Interpreter.V[y] + j) * this.Interpreter.Width;
                        var u = i + j * 8;
                        if (w < this.Interpreter.Pixels.Length)
                        {
                            if (this.Interpreter.Pixels[w] && bsprite[u])
                            {
                                erase = true;
                            }

                            this.Interpreter.Pixels[w] ^= bsprite[u];
                        }
                    }
                }

                this.Interpreter.V[15] = (byte)(erase ? 1 : 0);
                this.Interpreter.DrawScreen = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a sprite from the memory and convers the bits to big endian.
        /// </summary>
        /// <param name="n">The size of the sprite in bytes.</param>
        /// <returns>A list of booleans representing the sprite.</returns>
        private List<bool> GetSprite(byte n)
        {
            var sprite = new byte[n];
            Array.Copy(this.Interpreter.Memory, this.Interpreter.I, sprite, 0, n);
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

            return bsprite;
        }
    }
}