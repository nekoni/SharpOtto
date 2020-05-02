
namespace SharpOtto.Core.Opcodes
{
    using System;

    /// <summary>
    /// Fx65 - LD Vx, [I]
    /// Read registers V0 through Vx from memory starting at location I.
    /// The interpreter reads values from memory starting at location I into registers V0 through Vx.
    /// </summary>
    internal class LdVxIOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x65)
            {
                Array.Copy(this.Interpreter.Memory, this.Interpreter.I, this.Interpreter.V, 0, x+1);
                return true;
            }

            return false;
        }
    }
}