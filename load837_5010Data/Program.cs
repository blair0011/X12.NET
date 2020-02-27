using System;
using System.Collections.Generic;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using X12.Parsing;
using X12.Shared.Models;

namespace load837_5010Data
{
    class Program
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option(Description = "Database connection string", ShortName = "db")]
        public string Conn { get; }

        [Option(Description = "Input 837 file path and name.", ShortName = "i")]
        public string FilePath { get; }

        private void OnExecute()
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Wrong path!", FilePath);

            string _837file = File.ReadAllText(FilePath);
            var parser = new X12Parser(false);

            IList<Interchange> interchanges = parser.ParseMultiple(_837file);

        }
    }
}
