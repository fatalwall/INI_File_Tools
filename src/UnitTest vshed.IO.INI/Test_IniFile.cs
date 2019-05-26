using System;
using System.IO;
using vshed.IO.INI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace UnitTest_vshed.IO.INI
{
    [TestClass]
    public class Test_IniFile
    {
        [TestMethod]
        public void INI_FILE_READ()
        {
            var file = new IniFile("Settings.ini");
            Assert.IsTrue(file.Sections.Count == 2);
        }

        [TestMethod]
        public void INI_STREAM_READ()
        {
            string txt = (new StreamReader("Settings.ini")).ReadToEnd();

            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(txt));
            var file = new IniFile(stream);
            Assert.IsTrue(file.Sections.Count == 2);
        }

        [TestMethod]
        public void INI_STREAM_READ_COMMENTPOUND()
        {
            string txt = (new StreamReader("SettingsPOUND.ini")).ReadToEnd();

            using (Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(txt)))
            {
                var file = new IniFile(stream, CommentCharacterTypes.NumberSign);
                Assert.IsTrue(file.Sections.Count == 2);
            }
        }

        [TestMethod]
        public void INI_STREAM_READ_NO_SECTIONS()
        {
            string txt = (new StreamReader("SettingsNoSections.ini")).ReadToEnd();

            using (Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(txt)))
            {
                var file = new IniFile(stream);
                
                Assert.IsTrue(!file.Content.Contains("[") || !file.Content.Contains("]"));
            }
        }
    }
}
