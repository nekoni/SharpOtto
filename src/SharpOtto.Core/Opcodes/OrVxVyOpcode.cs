namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy1 - OR Vx, Vy
    /// Set Vx = Vx OR Vy.
    /// Performs a bitwise OR on the values of Vx and Vy, then stores the result in Vx. A bitwise OR compares the corrseponding bits from two values, and if either bit is 1, then the same bit in the result is also 1. Otherwise, it is 0.
    /// </summary>
    internal class OrVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x01)
            {
                this.Interpreter.V[x] |= this.Interpreter.V[y];
                return true;
            }

            return false;
        }
    }
}