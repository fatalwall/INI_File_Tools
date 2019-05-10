/* 
 *Copyright (C) 2018-2019 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/INI_File_Tools
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace vshed.IO.INI
{
    public class IniFile
    {
        public IniFile(CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon) { this.CommentCharacter = CommentCharacter; }
        public IniFile(string FilePath, bool IgnoreFileNotFound = false, CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon)
        { this.CommentCharacter = CommentCharacter; this.Read(FilePath); }
        public IniFile(Stream stream, CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon)
        { this.CommentCharacter = CommentCharacter; this.Read(stream); }

        public CommentCharacterTypes CommentCharacter { get; private set; }
        public bool IgnoreFileNotFound { get; private set; }

        public enum LineTypes
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

        public void Read(Stream stream, int streamPosition = 0)
        {
            string readContents;
            stream.Position = streamPosition;
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream))
            {
                string CurSection = "";
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
        public void Read(string FilePath)
        {
            this.FilePath = FilePath;

            if (Sections == null) { Sections = new List<Section>(); }
            else { Sections.Clear(); }

            if (!System.IO.File.Exists(this.FilePath)) { if (IgnoreFileNotFound) { return; } else throw new FileNotFoundException("The INI configuraiton file you are trying to load could not be found.", this.FilePath); }
            Read((new StreamReader(this.FilePath)).BaseStream);
        }


        public void Write(string FilePath)
        {
            this.FilePath = FilePath;
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(this.FilePath))
            {
                //string combined = "";
                //foreach (Section s in this.Sections)
                //{
                //    combined += s;
                //    combined += Environment.NewLine;
                //    combined += Environment.NewLine;
                //}
                //writer.Write(combined.TrimEnd());
                writer.Write(this.Content);
                writer.Close();
            }
        }
        public Stream Write()
        {

            //string combined = "";
            //foreach (Section s in this.Sections)
            //{
            //    combined += s;
            //    combined += Environment.NewLine;
            //    combined += Environment.NewLine;
            //}
            return (Stream)(new MemoryStream(Encoding.ASCII.GetBytes(this.Content)));
            //using (System.IO.StreamWriter writer = new System.IO.StreamWriter(this.FilePath))
            //{
            //    writer.Write(this.ToString());
            //    writer.Close();
            //}
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

        public override string ToString() { return string.Format("[{0} Sections] {1}", this.Sections.Count, this.FilePath ?? ""); }

        public string Content => (string)this;
        public static implicit operator string(IniFile iniFile)
        {
            string output = "";
            foreach (Section s in iniFile.Sections)
            {
                output += (String)s;
                output += Environment.NewLine;

            }
            return output.TrimEnd();
        }
    }
}
