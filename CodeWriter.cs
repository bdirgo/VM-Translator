using VMParser;
using System;

namespace VMCodeWriter
{
    class CodeWriter
    {
        private enum MemoryMap
        {
            SP,
            LCL,
            ARG,
            THIS,
            THAT,
            TEMP,
            MAXTEMP = 12,
            R13,
            R14,
            R15,
            STATIC,
            MAXSTATIC = 255,
            STACK,
            MAXSTACK = 2047
        }
        public string Code { get; private set; }
        private Parser Line;
        public CodeWriter(Parser parsedLine) {
            Line = parsedLine;
            var currentCommand = parsedLine.CommandType();
            if (currentCommand == Command.ARITHMETIC) {
                WriteArithmetic(parsedLine.SplitLine[0]);
            }
            if (
                currentCommand == Command.PUSH ||
                currentCommand == Command.POP
            ) {
                WritePushPop(currentCommand, parsedLine.Arg1(), int.Parse(parsedLine.Arg2()));
            }
        }

        private void WriteArithmetic(string command) {
            Code = "MATH";
        }
        private void WritePushPop(Command command, string segment, int index) {
            if (command == Command.PUSH)
            {
                if (segment == "local") {
                    Code =
                    $"{printVMCodeAsComment()}" +
                    $"@LCL // ADDR = LCL + {index}" + Environment.NewLine +
                    $"D=M" + Environment.NewLine +
                    $"@{index}" + Environment.NewLine +
                    $"D=D+A" + Environment.NewLine +
                    $"@ADDR" + Environment.NewLine +
                    $"M=D" + Environment.NewLine +
                    $"{SPMM()}" +
                    $"@SP // *ADDR = *SP" + Environment.NewLine +
                    $"A=M" + Environment.NewLine +
                    $"D=M" + Environment.NewLine +
                    $"@ADDR" + Environment.NewLine +
                    $"A=M" + Environment.NewLine +
                    $"M=D"
                    ;
                }
                if (segment == "constant")
                {
                    Code =
                    $"{printVMCodeAsComment()}" +
                    $"@{index} // D={index}" + Environment.NewLine +
                    $"D=A" + Environment.NewLine +
                    $"@SP // *SP=D" + Environment.NewLine +
                    $"A=M" + Environment.NewLine +
                    $"M=D" + Environment.NewLine +
                    $"{SPPP()}"
                    ;
                } 
            } else if (command == Command.POP) {
                Code = "POP";
            } else {
                Code = "PUSHPOP";
            }
        }

        private string printVMCodeAsComment() {
            return $"//{Line.Line}" + Environment.NewLine;
        }

        private string SPPP()
        {
            return
                    $"@SP // SP++" + Environment.NewLine +
                    $"M=M+1" + Environment.NewLine;
        }

        private string SPMM()
        {
            return
                    $"@SP // SP--" + Environment.NewLine +
                    $"M=M-1" + Environment.NewLine;
        }

    }
}