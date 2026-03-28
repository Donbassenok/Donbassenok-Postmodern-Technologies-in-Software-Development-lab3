using NUnit.Framework;
using System.IO;
using my_uniq;

namespace my_uniq.Tests
{
    public class Tests
    {
        [Test]
        public void Test_Stdin_Stdout_Success()
        {
            var input = new StringReader("hello\nhello\nworld\n");
            var output = new StringWriter();
            var error = new StringWriter(); 
            
            int exitCode = App.Run(new string[] { }, input, output, error);
            
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString().Replace("\r\n", "\n"), Is.EqualTo("hello\nworld\n"));
            Assert.That(error.ToString(), Is.Empty);
        }

        [Test]
        public void Test_UnknownOption_ReturnsError()
        {
            var input = new StringReader("");
            var output = new StringWriter(); 
            var error = new StringWriter(); 
            
            int exitCode = App.Run(new string[] { "--unknown" }, input, output, error);
            
            Assert.That(exitCode, Is.EqualTo(2)); 
            Assert.That(output.ToString(), Is.Empty); 
            Assert.That(error.ToString(), Does.Contain("unknown").IgnoreCase);
        }
    }
}
