namespace SharpOtto.Core.Opcodes
{
    /// <summary>
    /// This is the base class for all the opcodes handlers.
    /// </summary>
    internal abstract class Opcode
    {
        /// <summary>
        /// Gets or sets the <see cref="IInterpreter" /> instance.
        /// </summary>
        public IInterpreter Interpreter { get; set; }

        /// <summary>
        /// Gets or sets a boolean value telling whether or not skip
        /// the program counter increment after executing the instruction.
        /// </summary>
        public bool SkipIncrementProgramCounter { get; set; } = false;

        /// <summary>
        /// Execute the opcode.
        /// </summary>
        /// <param name="value">The opcode.</param>
        /// <param name="value">The op component (opcode mask 0xF000).</param>
        /// <param name="value">The x component (opcode mask 0x0F00 >> 8).</param>
        /// <param name="value">The y component (opcode mask 0x00F0 >> 4).</param>
        /// <param name="value">The k component (opcode mask 0x00FF).</param>
        /// <param name="value">The o component (opcode mask 0x000F).</param>
        /// <param name="value">The n component (opcode mask 0x0FFF).</param>
        /// <returns>True the value was executed otherwise false.</returns>
        public abstract bool Execute(ushort opcode, ushort op, byte x, byte y, byte k, byte o, ushort n);
    }
}