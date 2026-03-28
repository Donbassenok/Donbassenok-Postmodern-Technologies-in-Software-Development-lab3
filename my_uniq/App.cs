using System;
using System.IO;
using System.Collections.Generic;

namespace my_uniq
{
    public class App
    {
        public static int Run(string[] args, TextReader inStream, TextWriter outStream, TextWriter errorStream)
        {
            bool countDuplicates = false;
            bool ignoreCase = false;
            string filePath = null;

            foreach (var arg in args)
            {
                if (arg == "-c") countDuplicates = true;
                else if (arg == "-i") ignoreCase = true;
                else if (arg.StartsWith("-"))
                {
                    errorStream.WriteLine($"Unknown option: {arg}");
                    return 2; //невірний аргумент
                }
                else filePath = arg;
            }

            TextReader reader = inStream;
            if (filePath != null)
            {
                if (!File.Exists(filePath))
                {
                    errorStream.WriteLine($"File not found: {filePath}");
                    return 1; //часткова помилка 
                }
                reader = new StreamReader(filePath);
            }

            try
            {
                ProcessUniq(reader, outStream, countDuplicates, ignoreCase);
            }
            finally
            {
                if (filePath != null) reader.Dispose();
            }

            return 0; // успішне виконання 
        }

        private static void ProcessUniq(TextReader reader, TextWriter outStream, bool count, bool ignoreCase)
        {
            string previousLine = null;
            int duplicateCount = 0;
            string currentLine;

            StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            while ((currentLine = reader.ReadLine()) != null)
            {
                if (previousLine == null)
                {
                    previousLine = currentLine;
                    duplicateCount = 1;
                    continue;
                }

                if (string.Equals(previousLine, currentLine, comparison))
                {
                    duplicateCount++;
                }
                else
                {
                    PrintLine(outStream, previousLine, duplicateCount, count);
                    previousLine = currentLine;
                    duplicateCount = 1;
                }
            }

            if (previousLine != null)
            {
                PrintLine(outStream, previousLine, duplicateCount, count);
            }
        }

        private static void PrintLine(TextWriter outStream, string line, int count, bool showCount)
        {
            if (showCount) outStream.WriteLine($"{count,4} {line}");
            else outStream.WriteLine(line);
        }
    }
}