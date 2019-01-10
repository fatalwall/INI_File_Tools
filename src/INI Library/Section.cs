/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/INI_File_Tools
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Text;

namespace System.IO.INI
{
    public class Section
    {
        public Section(string Name, CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon) { this.Name = Name; this.Comments = new List<string>(); this.CommentCharacter = CommentCharacter; }
        public Section(string Name, List<string> Comments, CommentCharacterTypes CommentCharacter = CommentCharacterTypes.Semicolon) { this.Name = Name; this.Comments = new List<string>(Comments); this.CommentCharacter = CommentCharacter; }
        public string Name { get; private set; }
        public List<string> Comments { get; set; }
        public INI.CommentCharacterTypes CommentCharacter { get; private set; }
        public List<KeyValuePair> Elements = new List<KeyValuePair>();
        public int Count { get { return Elements.Count; } }

        public KeyValuePair this[string Key]
        {
            get
            {
                foreach (KeyValuePair e in Elements)
                {
                    if (e.Key == Key) { return e; }
                }
                return null;
            }
        }

        public KeyValuePair this[int index]
        {
            get { return Elements[index]; }
        }

        public int IndexOf(string Key)
        {
            return Elements.IndexOf(this[Key]);
        }
        public int IndexOf(KeyValuePair KeyValue)
        {
            return Elements.IndexOf(KeyValue);
        }

        public void Add(KeyValuePair d) { Elements.Add(d); }

        public void Remove(KeyValuePair d) { if (this.IndexOf(d) >= 0) Elements.Remove(d); }

        public void RemoveAt(int index) { Elements.RemoveAt(index); }

        public void Remove(string Key) { Remove(this[Key]); }

        public void Clear() { Elements.Clear(); }

        public override string ToString()
        {
            return string.Format("[{0}]", this.Name);
        }
        public static implicit operator string(Section Section) 
        {
            StringBuilder output = new StringBuilder();
            foreach (string c in Section.Comments) { output.AppendLine(string.Format("{0}{1}", (char)(int)Section.CommentCharacter, c)); }
            output.AppendLine(string.Format("[{0}]", Section.Name));
            foreach (KeyValuePair e in Section.Elements) { output.AppendLine(e.ToString()); }
            return output.ToString();
        }

    }
}
