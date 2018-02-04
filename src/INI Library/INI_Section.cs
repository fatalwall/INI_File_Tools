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

namespace INI_LIB
{
    public class INI_Section
    {
        public INI_Section(string Name) { this.Name = Name; }
        public string Name { get; private set; }
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
            string output = string.Format("[{0}]", this.Name);
            foreach (INI_KeyValue e in Elements)
            {
                output += Environment.NewLine;
                output += e.ToString();
            }
            return output; 
        }

    }
}
