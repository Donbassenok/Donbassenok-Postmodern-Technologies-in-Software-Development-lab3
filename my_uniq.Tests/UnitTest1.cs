using NUnit.Framework;
using System.IO;
using my_uniq;

namespace my_uniq.Tests
{
    public class Tests
    {
        private string CreateTempFile(string content)
        {
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, content);
            return tempFilePath;
        }

        [Test]
        public void Test_FileNotFound_ReturnsExitCode1()
        {
            var output = new StringWriter();
            var error = new StringWriter();
            
            int exitCode = App.Run(new string[] { "nonexistent_file.txt" }, new StringReader(""), output, error);
            
            Assert.That(exitCode, Is.EqualTo(1));
            Assert.That(error.ToString(), Does.Contain("not found").IgnoreCase);
        }

        [Test]
        public void Test_BasicUniq_Stdin()
        {
            var input = new StringReader("a\na\nb\n");
            var output = new StringWriter();
            int exitCode = App.Run(new string[] { }, input, output, new StringWriter());
            
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("a\nb\n"));
        }

        [Test]
        public void Test_BasicUniq_File()
        {
            string tempFile = CreateTempFile("a\na\nb\n");
            try
            {
                var output = new StringWriter();
                int exitCode = App.Run(new string[] { tempFile }, new StringReader(""), output, new StringWriter());
                
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("a\nb\n"));
            }
            finally { File.Delete(tempFile); }
        }

        [Test]
        public void Test_CountOption_Stdin()
        {
            var input = new StringReader("a\na\nb\n");
            var output = new StringWriter();
            int exitCode = App.Run(new string[] { "-c" }, input, output, new StringWriter());
            
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("   2 a\n   1 b\n"));
        }

        [Test]
        public void Test_CountOption_File()
        {
            string tempFile = CreateTempFile("a\na\nb\n");
            try
            {
                var output = new StringWriter();
                int exitCode = App.Run(new string[] { "-c", tempFile }, new StringReader(""), output, new StringWriter());
                
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("   2 a\n   1 b\n"));
            }
            finally { File.Delete(tempFile); }
        }

        [Test]
        public void Test_IgnoreCaseOption_Stdin()
        {
            var input = new StringReader("a\nA\nb\n");
            var output = new StringWriter();
            int exitCode = App.Run(new string[] { "-i" }, input, output, new StringWriter());
            
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("a\nb\n"));
        }

        [Test]
        public void Test_IgnoreCaseOption_File()
        {
            string tempFile = CreateTempFile("a\nA\nb\n");
            try
            {
                var output = new StringWriter();
                int exitCode = App.Run(new string[] { "-i", tempFile }, new StringReader(""), output, new StringWriter());
                
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("a\nb\n"));
            }
            finally { File.Delete(tempFile); }
        }

        [Test]
        public void Test_CombinedOptions_Stdin()
        {
            var input = new StringReader("a\nA\nb\n");
            var output = new StringWriter();
            int exitCode = App.Run(new string[] { "-c", "-i" }, input, output, new StringWriter());
            
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("   2 a\n   1 b\n"));
        }

        [Test]
        public void Test_FileTakesPriorityOverStdin()
        {
            string tempFile = CreateTempFile("data_from_file\n");
            try
            {
                var stdinInput = new StringReader("data_from_stdin\n");
                var output = new StringWriter();
                
                int exitCode = App.Run(new string[] { tempFile }, stdinInput, output, new StringWriter());
                
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("data_from_file\n"));
            }
            finally { File.Delete(tempFile); }
        }
    }
}
