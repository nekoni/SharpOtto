namespace SharpOtto.Core.Opcodes
{
    using System;

    /// <summary>
    /// Cxkk - RND Vx, byte
    /// Set Vx = random byte AND kk.
    /// The interpreter generates a random number from 0 to 255, which is then ANDed with the value kk. The results are stored in Vx. See instruction 8xy2 for more information on AND.
    /// </summary>
    internal class RndVxByteOpcode : Opcode
    {
        private Random random = new Random(DateTime.Now.Millisecond);

        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xC000)
            {
                this.Interpreter.V[x] = (byte)(this.random.Next(0, 255) & k);
                return true;
            }

            return false;
        }
    }
}