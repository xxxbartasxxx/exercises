
namespace WordCounter
{
    using CommandLine;
    using CommandLine.Text;

    class Options
    {
        [Option('d', "delete", HelpText = "Wyświetla monit")]
        public bool Delete { get; set; }

        [Option('e', "erase", HelpText = "Czyści całkowicie bazę aplikacji")]
        public bool Erase { get; set; }

        [Option('h', "help", HelpText = "Wyświetla tekst pomocy aplikacji")]
        public bool Help { get; set; }

        [Option('p', "put", HelpText = "Wstawia do bazy podaną wartość")]
        public string Put { get; set; }

        [Option('s', "select", HelpText = "Wyświetla ilość powtórzeń danej wartości w bazie danych")]
        public string Select { get; set; }

        [Option('v', "version", HelpText = "Wyświetla numer wersji oraz buildu aplikacji")]
        public bool Version { get; set; }

        public string GetUsage(Parsed<Options> parsed)
        {
            return HelpText.AutoBuild(parsed, null, null);
        }
    }
}
