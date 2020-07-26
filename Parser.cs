namespace VMParser {
    class Parser {
        public string Line { get; private set; }
        public string[] SplitLine { get; private set; }

        public Parser(string line) {
            Line = line;
            SplitLine = line.Split(" ");
        }

        public Command CommandType() {
            var command = Command.RETURN;
            switch(SplitLine[0]) {
                case "add":
                case "sub":
                case "neg":
                case "eq":
                case "gt":
                case "lt":
                case "and":
                case "or":
                case "not":
                    command = Command.ARITHMETIC;
                    break;
                case "push":
                    command = Command.PUSH;
                    break;
                case "pop":
                    command = Command.POP;
                    break;
                case "label":
                    command = Command.LABEL;
                    break;
                case "if-goto":
                    command = Command.IF;
                    break;
                case "goto":
                    command = Command.GOTO;
                    break;
                case "function":
                    command = Command.FUNCTION;
                    break;
                case "call":
                    command = Command.CALL;
                    break;
                default:
                    break;
            }
            return command;
        }

#nullable enable
        public string? Arg1() {
            if (SplitLine.Length >= 2)
            {
                if (CommandType() == Command.ARITHMETIC) {
                    return SplitLine[0];
                } else if (CommandType() == Command.RETURN) {
                    return null;
                }
                return SplitLine[1];
            }
            return null;
        }
        public string? Arg2() {
            var command = CommandType();
            if (
                command == Command.POP ||
                command == Command.PUSH ||
                command == Command.FUNCTION ||
                command == Command.CALL
            ) {
                return SplitLine[2];
            }
            return null;
        }
#nullable disable
    }
}