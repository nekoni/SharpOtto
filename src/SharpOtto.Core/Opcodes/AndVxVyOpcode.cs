namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// 8xy2 - AND Vx, Vy
    /// Set Vx = Vx AND Vy.
    /// Performs a bitwise AND on the values of Vx and Vy, then stores the result in Vx. A bitwise AND compares the corrseponding bits from two values, and if both bits are 1, then the same bit in the result is also 1. Otherwise, it is 0. 
    /// </summary>
    internal class AndVxVyOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0x8000 && o == 0x02)
            {
                this.Interpreter.V[x] &= this.Interpreter.V[y];
                return true;
            }

            return false;
        }
    }
}