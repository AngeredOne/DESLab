using System;
using CommandLine;

namespace DesLFSR.CommandLine
{
    public class CommandLineOptions
    {
        [Option('t', "type", Required = true)]
        public InputType Type { get; set; }
        [Option('i', "input", Required = true)]
        public string Input { get; set; }
        [Option('r', "result", Required = true)]
        public string Result { get; set; }
        
    }
}