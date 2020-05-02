namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// Fx0A - LD Vx, K
    /// Wait for a key press, store the value of the key in Vx.
    /// All execution stops until a key is pressed, then the value of that key is stored in Vx.
    /// </summary>
    internal class LdVxKOpcode : Opcode
    {
        /// <inheritdoc/>
        public override bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n)
        {
            if (op == 0xF000 && k == 0x0A)
            {
                var keyPressed = false;
                for (var i = 0; i < this.Interpreter.Keys.Length; i++)
                {
                    if (this.Interpreter.Keys[i])
                    {
                        this.Interpreter.V[x] = (byte)i;
                        keyPressed = true;
                        break;
                    }
                }
                
                this.SkipIncrementProgramCounter = !keyPressed;
                return true;
            }

            return false;
        }
    }
}