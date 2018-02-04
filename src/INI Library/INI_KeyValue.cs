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

namespace INI_LIB
{
    public class INI_KeyValue
    {
        public INI_KeyValue(string Key, string Value) { this.Key = Key; this.Value = Value; }

        public string Key { get; set; }
        public string Value { get; set; }
        public override string ToString() { return string.Format("{0}={1}", this.Key, this.Value); }
    }
}
