namespace SharpOtto.Tests
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using SharpOtto.Core;

    using Xunit;

    public class InterpreterTests
    {
        /// <summary>
        /// Test the conditional jumps, the mathematical and logical operations of Chip 8
        /// Each error is accompanied by a number that identifies the opcode in question. If all tests are positive, the rom will display on screen  "BON" meaning "GOOD".
        /// CORRESPONDENCE
        /// E 01: 3XNN verify that the jump condition is fair
        /// E 02: 5XY0 verify that the jump condition is fair
        /// E 03: 4XNN verify that the jump condition is fair
        /// E 04: 7XNN check the result of the addition
        /// E 05: 8XY5 verify that VF is set to 0 when there is a borrow
        /// E 06: 8XY5 verify that VF is set to 1 when there is no borrow
        /// E 07: 8XY7 verify that VF is set to 0 when there is a borrow
        /// E 08: 8XY7 verify that VF is set to 1 when there is no borrow
        /// E 09: 8XY1 check the result of the OR operation
        /// E 10: 8XY2 check the result of AND operation
        /// E 11: 8XY3 check the result of the XOR operation
        /// E 12: 8XYE verify that VF is set to the MSB (most significant bit or most left) before the shift and  VF does not take value 0 every time
        /// E 13: 8XYE verify that VF is set to the MSB (most significant bit or most left) before the shift and  VF does not take value 1 every time 
        /// E 14: 8XY6 verify that VF is set to the LSB (least significant bit or most right ) before the shift and  VF does not take value 0 every time
        /// E 15: 8XY6 verify that VF is the LSB (least significant bit or most right) before the shift and  VF does not take value 1 every time 
        /// E 16: FX55 and FX65 verify that these two opcodes are implemented. The error may come from one or the other or both are defects.
        /// E 17: FX33 calculating the binary representation is mistaken or the result is poorly stored into memory or poorly poped (FX65 or FX1E).
        /// 
        /// If everything goes smoothly the program reaches the opcode 0x130E with 197 executed opcodes and produces the expected bitmap.
        /// </summary>
        [Fact]
        public void RunTestRom()
        {
            var interpreter = new Interpreter() { EnableRecording = true, ExitOnOpcode = 4878 };
            var lastBitmap = default(Bitmap);
            var rom = GetResourceBytes("SharpOtto.Tests.test.ch8");
            var bmp = GetResourceBytes("SharpOtto.Tests.test.png");

            interpreter.OnUpdateScreen += (sender, ev) => { lastBitmap = ev.Bitmap; };
            interpreter.Run(rom);

            Assert.Equal(197, interpreter.ExecutedOpcodes.Count);

            using (var memoryStream = new MemoryStream())
            {
                lastBitmap.Save(memoryStream, ImageFormat.Png);
                Assert.False(true, DumpBinaryFile(memoryStream.ToArray()));
                // Assert.Equal(bmp, memoryStream.ToArray());
            }
        }

        private static string DumpBinaryFile(byte[] bytes)
        {
            var output = string.Empty;
            foreach (var b in bytes)
            {
                output += $"{b} ";
            }

            return output;
        }

        private static byte[] GetResourceBytes(string resourceName)
        {
            using (var memoryStream = new MemoryStream())
            {
                var assembly = typeof(SharpOtto.Tests.InterpreterTests).Assembly;
                var bmp = assembly.GetManifestResourceStream(resourceName);
                bmp.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
