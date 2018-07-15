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
        public INI(CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon) { this.CommentCharacter = CommentCharacter; }
        public INI(string FilePath, CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon)
        { this.FilePath = FilePath; this.CommentCharacter = CommentCharacter; this.Read(); }

        public enum CommentCharacterTypes
        {
            Semicolon = 59,
            NumberSign = 35
        }
        public CommentCharacterTypes CommentCharacter { get; private set; }

        private enum LineTypes
        {
            Invalid = -1,
            Comment = 0,
            Section = 1,
            KeyValue = 2,
        }
        private LineTypes GetLineType(string Content, out GroupCollection Groups)
        {
            Match match;
            //Comment
            match = Regex.Match(Content, string.Format(@"{0}(?'Comment'.*)", (char)(int)CommentCharacter));
            if (match.Success) { Groups = match.Groups; return LineTypes.Comment; }
            //Section
            match = Regex.Match(Content, @"\[(?'Section'.*)\]");
            //KeyValue
            if (match.Success) { Groups = match.Groups; return LineTypes.Section; }
            match = Regex.Match(Content, @"(?'Key'.*)=(?'Value'.*)");
            if (match.Success) { Groups = match.Groups; return LineTypes.KeyValue; }
            //Else
            Groups = null;
            return LineTypes.Invalid;
        }

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
                List<string> Comments = new List<string>();
                while ((readContents = streamReader.ReadLine()) != null)
                {
                    GroupCollection Groups;
                    switch (GetLineType(readContents, out Groups))
                    {
                        case LineTypes.Comment:
                            Comments.Add(Groups["Comment"].Value);
                            break;
                        case LineTypes.Section:
                            this.Add(new INI_Section(Groups["Section"].Value, Comments, CommentCharacter)); Comments.Clear();
                            CurSection = Groups["Section"].Value;
                            break;
                        case LineTypes.KeyValue:
                            this[CurSection].Add(new INI_KeyValue(Groups["Key"].Value, Groups["Value"].Value, Comments, CommentCharacter)); Comments.Clear();
                            break;
                        default: //LineType.Invalid
                            break;
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
                    combined += s;
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
                this.Add(new INI_Section(Name, this.CommentCharacter));
                return this[Name];
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
