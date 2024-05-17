
using System.IO;
using System.Text;

namespace Streams
{
    internal class Program
    {
        public static void CopyFile(string path, string writeFileName)
        {
            if (!Path.Exists(path))
            {
                Console.WriteLine("Could not to find file!");
                return;
            }
            using (StreamReader sr = new StreamReader(path)) 
            { 
                using (StreamWriter sw = new StreamWriter(writeFileName))
                {
                    sw.WriteLine(sr.ReadToEnd());
                }
            }
            Console.WriteLine($"File copied to {Path
                .Combine(Environment.CurrentDirectory, writeFileName)}");
        }
        public static void FindFile(string directory,string fileName)
        {
            var dirs = Directory.EnumerateDirectories(directory);
            var files = Directory.EnumerateFiles(directory,fileName);
            foreach (var file in files)
            {
                Console.WriteLine($"Founded file: {file}");
            }
            foreach (var dir in dirs) FindFile(dir, fileName);
        }
        public static void DisplayMatchLinesFromFile(string path, string line)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                bool hasMatches = false;
                Console.WriteLine("Matching lines:");
                while (!sr.EndOfStream)
                {
                    string? currentLine = sr.ReadLine();
                    if (currentLine == null) 
                    {
                        Console.WriteLine("File is empty.");
                        return;
                    }
                    if (currentLine.Contains(line))
                    {
                        Console.WriteLine(currentLine);
                        hasMatches = true;
                    }
                }
                if (!hasMatches) Console.WriteLine("None.");
            }
        }
        public static void AppendByBuffer(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    byte[] buffer = new byte[fs.Length];
                    bs.Read(buffer, 0, buffer.Length);
                    //used for no reason
                    //string bufferedString = Encoding.UTF8.GetString(buffer);
                    string newLine= "\nNew line to buffer";
                    byte[] toWrite = Encoding.UTF8.GetBytes(newLine);
                    bs.Write(toWrite, 0, toWrite.Length);
                }
            }
            Console.WriteLine("File changed");
        }
        public static void DisplayMatchLinesInMatchingFiles(string directory, string fileName, string line)
        {
            var dirs = Directory.EnumerateDirectories(directory);
            var files = Directory.EnumerateFiles(directory, fileName);
            foreach (var file in files)
            {
                Console.WriteLine($"Founded file: {file}");
                DisplayMatchLinesFromFile(file, line);
            }
            foreach (var dir in dirs) DisplayMatchLinesInMatchingFiles(dir, fileName, line);
        }
        static void Main(string[] args)
        {
            //CopyFile(@"E:\Projects\DotNet\Calculator\Calc.cs", "writen.txt");
            //File.Copy(@"E:\Projects\DotNet\Calculator\Calc.cs", "copied.txt");
            //AppendByBuffer(@"E:\Projects\DotNet\Streams\Buffer.txt");
            //if (args.Length == 2)
            //{
            //    Console.WriteLine("Can not find directory path and file name." +
            //        "Using default values:");
            //    args[0] = @"E:\Projects\DotNet";
            //    args[1] = "*.cs";
            //    Console.WriteLine(args[0] + " and " + args[1]);
            //}
            //FindFile(args[0], args[1]);
            if (args.Length == 3)
            {
                DisplayMatchLinesInMatchingFiles(args[0], args[1], args[2]);
                return;
            }
            Console.WriteLine("Wrong input count! Using default values:");
            Console.WriteLine(@"'E:\Projects\DotNet', '*.cs', 'Console'");
            DisplayMatchLinesInMatchingFiles(@"E:\Projects\DotNet", "*.cs",
                "namespace");
        }
    }
}
