namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy3 - XOR Vx, Vy
    /// Set Vx = Vx XOR Vy.
    /// Performs a bitwise exclusive OR on the values of Vx and Vy, then stores the result in Vx. An exclusive OR compares the corrseponding bits from two values, and if the bits are not both the same, then the corresponding bit in the result is set to 1. Otherwise, it is 0. 
    /// </summary>
    internal class XorVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x03)
            {
                this.Interpreter.V[x] ^= this.Interpreter.V[y];
                return true;
            }

            return false;
        }
    }
}