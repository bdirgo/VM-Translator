using System;
using VMParser;
using VMCodeWriter;
using System.IO;
using System.Collections.Generic;

enum Command
{
    ARITHMETIC,
    PUSH,
    POP,
    LABEL,
    GOTO,
    IF,
    FUNCTION,
    CALL,
    RETURN,
}

namespace VMTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "--file" || args[0] == "-f") {
                var filename = args[1];
                var outputFilename = Path.ChangeExtension(filename, ".asm");
// TODO: get filename for static variables
                Console.WriteLine($"{outputFilename}");
                var lines = ReadFrom(filename);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var isComment = (line[0] == '/') && (line[1] == '/');
                        if (!isComment) {
                            var parsedLine = new Parser(line);
                            var writer = new CodeWriter(parsedLine);
                            Console.WriteLine(writer.Code);
                        }
                    }
                }
            }
        }

        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using (var reader = File.OpenText(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
