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
        public INI_KeyValue(string Key, string Value, INI.CommentCharacterTypes CommentCharacter = INI.CommentCharacterTypes.Semicolon) { this.Key = Key; this.Value = Value; Comments = new List<string>(); this.CommentCharacter = CommentCharacter; }
        public INI_KeyValue(string Key, string Value, List<string> Comments, INI.CommentCharacterTypes CommentCharacter = INI.CommentCharacterTypes.Semicolon) { this.Key = Key; this.Value = Value; this.Comments = new List<string>(Comments); this.CommentCharacter = CommentCharacter; }

        public string Key { get; set; }
        public string Value { get; set; }
        public List<string> Comments { get; set; }
        public INI.CommentCharacterTypes CommentCharacter { get; private set; }
        public override string ToString() 
        {
            return string.Format("{0}={1}", this.Key, this.Value);
        }
        public static implicit operator string(INI_KeyValue KeyValue) 
        {
            StringBuilder output = new StringBuilder();
            foreach (string c in KeyValue.Comments) { output.AppendLine(string.Format("{0}{1}", (char)(int)KeyValue.CommentCharacter, c)); }
            output.AppendLine(string.Format("{0}={1}", KeyValue.Key, KeyValue.Value));
            return output.ToString();
        }
    }
}
