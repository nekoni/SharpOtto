namespace SharpOtto.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using SharpOtto.Core.Opcodes;

    internal class OpcodeExecutor
    {
        private IInterpreter interpreter;

        private List<Opcode> opcodeHandlers = new List<Opcode>();

        public OpcodeExecutor(IInterpreter interpreter)
        {
            this.interpreter = interpreter;
            var types = Assembly.GetAssembly(typeof(Interpreter))
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Opcode)));
            foreach (var type in types)
            {
                var opcode = (Opcode)Activator.CreateInstance(type);
                opcode.Interpreter = interpreter;
                this.opcodeHandlers.Add(opcode);
            }
        }

        internal void ExecuteNext()
        {
            var opcode = (ushort)
                (this.interpreter.Memory[this.interpreter.ProgramCounter] << 8 |
                this.interpreter.Memory[this.interpreter.ProgramCounter + 1]);
            var op = (ushort)(opcode & 0xF000);
            var x = (byte)((opcode & 0x0F00) >> 8);
            var y = (byte)((opcode & 0x00F0) >> 4);
            var k = (byte)(opcode & 0x00FF);
            var o = (byte)(opcode & 0x000F);
            var n = (ushort)(opcode & 0x0FFF);
            Console.WriteLine($"0x{opcode.ToString("X")}");
            foreach (var opcodeHandler in this.opcodeHandlers)
            {
                if (opcodeHandler.Execute(opcode, op, x, y, k, o, n))
                {
                    if (!opcodeHandler.SkipIncrementProgramCounter)
                    {
                        this.interpreter.ProgramCounter += 2;
                    }

                    return;
                }
            }

            Console.WriteLine($"Unhandled opcode 0x{opcode.ToString("X")}");
            this.interpreter.ProgramCounter += 2;
        }
    }
}