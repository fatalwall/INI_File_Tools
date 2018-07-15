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

namespace INI_LIB
{
    public class INI_Section
    {
        public INI_Section(string Name, INI.CommentCharacterTypes CommentCharacter = INI.CommentCharacterTypes.Semicolon) { this.Name = Name; this.Comments = new List<string>(); this.CommentCharacter = CommentCharacter; }
        public INI_Section(string Name, List<string> Comments, INI.CommentCharacterTypes CommentCharacter = INI.CommentCharacterTypes.Semicolon) { this.Name = Name; this.Comments = new List<string>(Comments); this.CommentCharacter = CommentCharacter; }
        public string Name { get; private set; }
        public List<string> Comments { get; set; }
        public INI.CommentCharacterTypes CommentCharacter { get; private set; }
        public List<INI_KeyValue> Elements = new List<INI_KeyValue>();
        public int Count { get { return Elements.Count; } }

        public INI_KeyValue this[string Key]
        {
            get
            {
                foreach (INI_KeyValue e in Elements)
                {
                    if (e.Key == Key) { return e; }
                }
                return null;
            }
        }

        public INI_KeyValue this[int index]
        {
            get { return Elements[index]; }
        }

        public int IndexOf(string Key)
        {
            return Elements.IndexOf(this[Key]);
        }
        public int IndexOf(INI_KeyValue KeyValue)
        {
            return Elements.IndexOf(KeyValue);
        }

        public void Add(INI_KeyValue d) { Elements.Add(d); }

        public void Remove(INI_KeyValue d) { if (this.IndexOf(d) >= 0) Elements.Remove(d); }

        public void RemoveAt(int index) { Elements.RemoveAt(index); }

        public void Remove(string Key) { Remove(this[Key]); }

        public void Clear() { Elements.Clear(); }

        public override string ToString()
        {
            return string.Format("[{0}]", this.Name);
        }
        public static implicit operator string(INI_Section Section) 
        {
            StringBuilder output = new StringBuilder();
            foreach (string c in Section.Comments) { output.AppendLine(string.Format("{0}{1}", (char)(int)Section.CommentCharacter, c)); }
            output.AppendLine(string.Format("[{0}]", Section.Name));
            foreach (INI_KeyValue e in Section.Elements) { output.AppendLine(e.ToString()); }
            return output.ToString();
        }

    }
}
