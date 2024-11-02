using Hospital.Console.Configuration;

namespace Hospital.Tests.Console
{
    [TestFixture]
    public class CmdOptionsTests
    {
        [Test]
        public void Parse_Seed_Correct()
        {
            string input = "-s 10";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(10));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_SeedCustom_Correct()
        {
            string input = "-s 100";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(100));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_Url_Correct()
        {
            string input = "-u https://localhost/patients";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(100));
                Assert.That(result.Url, Is.EqualTo("https://localhost/patients"));
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_SeedUrl_Correct()
        {
            string input = "-s 10 -u https://localhost/patients";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(10));
                Assert.That(result.Url, Is.EqualTo("https://localhost/patients"));
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_SeedNoAmount_Returns100()
        {
            string input = "-s";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(100));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_Clear_True()
        {
            string input = "-c";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(0));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.True);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_Exit_True()
        {
            string input = "-x";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(0));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Exit, Is.True);
                Assert.That(result.Clear, Is.False);
            });
        }

        [Test]
        public void Parse_Options_Correct()
        {
            string input = "-s 100 -c -x";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(100));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.True);
                Assert.That(result.Exit, Is.True);
            });
        }

        [Test]
        public void Parse_Empty_ReturnsZero()
        {
            string input = "";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(0));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }

        [Test]
        public void Parse_IncorrectSeed_ReturnsZero()
        {
            string input = "-s qwerty";

            var result = CmdOptions.Parse(input);

            Assert.Multiple(() =>
            {
                Assert.That(result.Seed, Is.EqualTo(0));
                Assert.That(result.Url, Is.Null);
                Assert.That(result.Clear, Is.False);
                Assert.That(result.Exit, Is.False);
            });
        }
    }
}
