namespace Hospital.Console.Configuration
{
    public sealed class CmdOptions
    {
        // -s [amount]
        public int Seed { get; set; } = 0;

        // -u api url
        public string Url { get; set; }

        // -c
        public bool Clear { get; set; }
        // -x
        public bool Exit { get; set; }

        public static CmdOptions Parse(string input)
        {
            var options = new CmdOptions();

            var parts = string.Join(" ", input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).Split(' ');

            for (var i = 0; i < parts.Length; i++)
            {
                var p = parts[i];

                if (string.IsNullOrEmpty(p))
                    return options;

                if (p == "-s")
                {
                    if (i + 1 < parts.Length)
                    {
                        if (int.TryParse(parts[i + 1], out int result))
                            options.Seed = result;
                    }
                    else
                    {
                        options.Seed = 100;
                    }
                }
                if (p == "-u")
                {
                    if (i + 1 < parts.Length)
                    {
                        if(!string.IsNullOrEmpty(parts[i + 1]))
                        {
                            options.Url = parts[i + 1];
                            if (options.Seed == 0)
                            {
                                options.Seed = 100;
                            }
                        }
                    }
                }
                if (p == "-c")
                    options.Clear = true;

                if (p == "-x")
                    options.Exit = true;
            }

            return options;
        }
    }
}
