/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/INI_File_Tools
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace INI_LIB
{
    public class INI
    {
        public INI() { }
        public INI(string FilePath) { this.FilePath = FilePath; this.Read(); }
        public void Read(string FilePath) { this.FilePath = FilePath; this.Read(); }
        private void Read() 
        {
            if (Sections == null) { Sections = new List<INI_Section>(); }
            else { Sections.Clear(); }

            string readContents;
            if (!File.Exists(this.FilePath)) { throw new FileNotFoundException("The INI configuraiton file you are trying to load could not be found.", this.FilePath); }
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(this.FilePath))
            {
                string CurSection="";
                while ((readContents = streamReader.ReadLine()) != null)
                {
                    //Check if its a new section and create an object for it
                    Match TestSection = Regex.Match(readContents, @"\[(?'Section'.*)\]");
                    if (TestSection.Success)
                    {
                        CurSection= TestSection.Groups["Section"].Value;
                        this.Add(new INI_Section(CurSection));
                    }else{
                        //Check if its a Key Value Pair and add it to the last Section object
                        Match TestKeyValue = Regex.Match(readContents, @"(?'Key'.*)=(?'Value'.*)");
                        if (TestKeyValue.Success)
                        {
                            this[CurSection].Add(new INI_KeyValue(TestKeyValue.Groups["Key"].Value, TestKeyValue.Groups["Value"].Value));
                        }
                    }
                }
                streamReader.Close();
            }
        }
        public void Write(string FilePath) { this.FilePath = FilePath; this.Write(); }
        public void Write()
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(this.FilePath))
            {
                string combined = "";
                foreach (INI_Section s in this.Sections)
                {
                    combined += s.ToString();
                    combined += Environment.NewLine;
                    combined += Environment.NewLine;
                }
                writer.Write(combined.TrimEnd());
                writer.Close();
            }
        }

        public string FilePath { get; private set; }
        public List<INI_Section> Sections { get; set; }

        public INI_Section this[string Name]
        {
            get
            {
                foreach (INI_Section e in Sections)
                {
                    if (e.Name == Name) { return e; }
                }
                return null;
            }
        }

        public INI_Section this[int index]
        {
            get { return Sections[index]; }
        }

        public int IndexOf(string Name)
        {
            return Sections.IndexOf(this[Name]);
        }
        public int IndexOf(INI_Section Section)
        {
            return Sections.IndexOf(Section);
        }

        public void Add(INI_Section d) { Sections.Add(d); }

        public void Remove(INI_Section d) { if (this.IndexOf(d) >= 0) Sections.Remove(d); }

        public void RemoveAt(int index) { Sections.RemoveAt(index); }

        public void Remove(string Name) { Remove(this[Name]); }

        public void Clear() { Sections.Clear(); }

        public override string ToString()
        {
            string output = "";
            foreach (INI_Section s in Sections)
            {
                output += s.ToString();
                output += Environment.NewLine;

            }
            return output.TrimEnd(); 
        }

    }
}
