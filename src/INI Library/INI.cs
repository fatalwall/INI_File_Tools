/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/INI_File_Tools
 */

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.IO.INI
{
    public class File
    {
        public File(CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon) { this.CommentCharacter = CommentCharacter; }
        public File(string FilePath,bool IgnoreFileNotFound = false, CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon)
        { this.FilePath = FilePath; this.CommentCharacter = CommentCharacter; this.Read(); }

        public CommentCharacterTypes CommentCharacter { get; private set; }
        public bool IgnoreFileNotFound { get; private set; }

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
            if (Sections == null) { Sections = new List<Section>(); }
            else { Sections.Clear(); }

            string readContents;
            if (!IO.File.Exists(this.FilePath)) { if (IgnoreFileNotFound) { return; } else throw new FileNotFoundException("The INI configuraiton file you are trying to load could not be found.", this.FilePath); }
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
                            this.Add(new Section(Groups["Section"].Value, Comments, CommentCharacter)); Comments.Clear();
                            CurSection = Groups["Section"].Value;
                            break;
                        case LineTypes.KeyValue:
                            this[CurSection].Add(new KeyValuePair(Groups["Key"].Value, Groups["Value"].Value, Comments, CommentCharacter)); Comments.Clear();
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
                foreach (Section s in this.Sections)
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
        public List<Section> Sections { get; set; }

        public Section this[string Name]
        {
            get
            {
                foreach (Section e in Sections)
                {
                    if (e.Name == Name) { return e; }
                }
                this.Add(new Section(Name, this.CommentCharacter));
                return this[Name];
            }
        }

        public Section this[int index]
        {
            get { return Sections[index]; }
        }

        public int IndexOf(string Name)
        {
            return Sections.IndexOf(this[Name]); 
        }
        public int IndexOf(Section Section)
        {
            return Sections.IndexOf(Section);
        }

        public void Add(Section d) { Sections.Add(d); }

        public void Remove(Section d) { if (this.IndexOf(d) >= 0) Sections.Remove(d); }

        public void RemoveAt(int index) { Sections.RemoveAt(index); }

        public void Remove(string Name) { Remove(this[Name]); }

        public void Clear() { Sections.Clear(); }

        public override string ToString()
        {
            string output = "";
            foreach (Section s in Sections)
            {
                output += s.ToString();
                output += Environment.NewLine;

            }
            return output.TrimEnd(); 
        }

    }
}
