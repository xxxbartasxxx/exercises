
namespace WordCounter
{
    using CommandLine;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using static System.Console;

    class Program
    {
        private const string fileName = "database.file";

        private static void Main(string[] args)
        {
            try
            {
                string put = null;
                var res = Parser.Default.ParseArguments<Options>(args);

                if (res.Tag == ParserResultType.Parsed)
                {
                    var parsed = res as Parsed<Options>;
                    if (put != null)
                        parsed.Value.Put = put;
                    WriteLine(Parser.Default.FormatCommandLine(parsed.Value));
                    Run(parsed);
                }
                else
                {
                    WriteLine("Failed to parse command line args. Most likely value for -s or for -p is not provided.");
                }
            }
            catch (System.Exception excep)
            {
                WriteLine(excep);
            }
            WriteLine("Press any key to continue...");
            ReadKey(true);
        }

        private static bool DeletePrompt()
        {
            WriteLine("Do you want to clear db [y/n]?");
            var key = ReadKey();
            return char.ToUpperInvariant(key.KeyChar) == 'Y';
        }

        private static void DisplayVersion(Options opt)
        {
            if (!opt.Version)
                return;

            var callingAssembly = Assembly.GetCallingAssembly();
            var productName = callingAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            var copyright = callingAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            var infoVersion = callingAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            WriteLine("{0} {1}", productName, infoVersion);
            WriteLine(copyright);
        }

        private static void Run(Parsed<Options> parsed)
        {
            var opt = parsed.Value;

            Help(parsed);

            Delete(opt);

            DisplayVersion(opt);

            AppendText(opt);

            CountWords(opt);
        }

        private static void Help(Parsed<Options> parsed)
        {
            if (!parsed.Value.Help)
                return;
            WriteLine(parsed.Value.GetUsage(parsed));
        }

        private static void Delete(Options opt)
        {
            if (opt.Delete && DeletePrompt() || opt.Erase)
            {
                using (File.Create(fileName)) { }
            }
        }

        private static void CountWords(Options opt)
        {
            if (string.IsNullOrEmpty(opt.Select))
                throw new System.ArgumentException();
            var regex = new Regex(opt.Select);

            int counter = 0;
            using (var sr = File.OpenText(fileName))
            {
                string line = string.Empty;
                while (null != (line = sr.ReadLine()))
                {
                    var matches = regex.Matches(line);
                    counter += matches.Count;
                }
            }

            WriteLine("Count of {0} matches: {1}", opt.Select, counter);
        }

        private static void AppendText(Options opt)
        {
            if (string.IsNullOrEmpty(opt.Put))
                throw new System.ArgumentException();

            using (var sw = File.AppendText(fileName))
            {
                sw.WriteLine(opt.Put);
            }
        }
    }
}
