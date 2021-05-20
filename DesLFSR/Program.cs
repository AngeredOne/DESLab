using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using DesLFSR.CommandLine;
using DesLFSR.DES;

namespace DesLFSR
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser(with =>
            {
                with.CaseInsensitiveEnumValues = true;
                with.AutoHelp = true;
                with.HelpWriter = Parser.Default.Settings.HelpWriter;
            });

            var result = parser.ParseArguments<CommandLineOptions>(args).WithParsed(Start);
            
        }

        private static void Start(CommandLineOptions opts)
        {
            switch (opts.Type)
            {
                case InputType.Text:
                    DemoString(opts.Input);
                    break;
                case InputType.File:
                    DemoFile(opts.Input, opts.Result);
                    break;
            }
        }

        private static void DemoString(string inputText)
        {
            Console.WriteLine("Исходный текст --->>>");
            Console.WriteLine(inputText);

            var encryptor = new Shit(128, 16);

            var encrypted = encryptor.Encrypt(inputText);
            Console.WriteLine("Зашифрованный текст --->>>");
            Console.WriteLine(encrypted);

            var decrypted = encryptor.Decode(encrypted);
            Console.WriteLine("Расшифрованный текст --->>>");
            Console.WriteLine(decrypted);
            
        }

        private static void DemoFile(string inputPath, string resultPath)
        {
            FileInfo fileInfo = new FileInfo(inputPath);
            var ext = fileInfo.Extension;
            
            var data = new byte[fileInfo.Length];
            
            using (FileStream fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
            }
            
            string fileStr = "";

            foreach (var bt in data)
            {
                fileStr += (char)bt;
            }

            var encryptor = new Shit();
            
            var encrypted = encryptor.Encrypt(fileStr);

            var decrypted = encryptor.Decode(encrypted);

            var dataN = new List<byte>();

            foreach (var btc in decrypted)
            {
                dataN.Add((byte)btc);
            }
            
            using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(resultPath, $"result{ext}"), FileMode.Create)))
            {
                writer.Write(dataN.ToArray());
            }
            
        }
    }
}
