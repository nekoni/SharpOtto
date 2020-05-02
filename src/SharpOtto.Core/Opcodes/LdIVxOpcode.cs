
namespace SharpOtto.Core.Opcodes
{
    using System;

    /// <summary>
    /// Fx55 - LD [I], Vx
    /// Store registers V0 through Vx in memory starting at location I.
    /// The interpreter copies the values of registers V0 through Vx into memory, starting at the address in I.
    /// </summary>
    internal class LdIVxOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x55)
            {
                Array.Copy(this.Interpreter.V, 0, this.Interpreter.Memory, this.Interpreter.I, x+1);
                return true;
            }

            return false;
        }
    }
}