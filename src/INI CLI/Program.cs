/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/INI_File_Tools
 */

using System;

namespace INI_CLI
{
    class Program
    {
        static string FilePath = null;
        static string Section = null;
        static string Key = null;
        static string Value = null;

        static void Main(string[] args)
        {
            if (args.Length == 0) { Console.Write(CommandHelp()); Environment.Exit(0); }
            for (int i = 0; i < args.Length ; i++)
            {

                string curArg = args[i];
                switch (curArg.ToUpper())
                {
                    case "-FILE":
                    case "-F":
                        FilePath = args[i + 1];
                        i++;
                        break;
                    case "-SECTION":
                    case "-S":
                        Section = args[i + 1];
                        i++;
                        break;
                    case "-KEY":
                    case "-K":
                        Key = args[i + 1];
                        i++;
                        break;
                    case "-Value":
                    case "-V":
                        Value = args[i + 1];
                        i++;
                        break;
                    case "-HELP":
                    case "/HELP":
                    case "-H":
                    case "/H":
                    case "?":
                        Console.Write(CommandHelp());
                        Environment.Exit(0);
                        break;
                    default:
                        //Throw error and end program
                        Console.WriteLine("Invalid Parameter Input.");
                        Console.Write(CommandHelp());
                        Environment.Exit(-1);
                        break;
                }
            }
            if (FilePath == null || Section == null || Key == null) 
            {
                Console.WriteLine("Invalid Parameter Input.");
                Console.Write(CommandHelp());
                Environment.Exit(-1);
            }

            vshed.IO.INI.IniFile config = new vshed.IO.INI.IniFile(FilePath,true);
            config.ToString();
            if (Value == null)
            {
                //Read
                try
                {
                    Value = config[Section][Key].Value;
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine(string.Format("Invalid Section or Key in {0}", FilePath));
                    Environment.Exit(-1);
                }
            }else {
                //Write     
                if (config.IndexOf(Section) < 0) { config.Add(new vshed.IO.INI.Section(Section)); }
                if (config[Section].IndexOf(Key) < 0)
                { config[Section].Add(new vshed.IO.INI.KeyValuePair(Key, Value)); }
                else { config[Section][Key].Value = Value; config.Write(); }
            }
                       
            //Output results
            Console.Write(Value);
            Environment.Exit(0);
        }

        static string CommandHelp()
        {
            string output = "";

            output += Environment.NewLine + string.Format("\tINI Command Line Interface");
            output += Environment.NewLine + string.Format("\tDesigned to read and update INI formatted files");

            //Commands
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t<COMMANDS>");
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t-{1}\t-{0}\t\t{2}", "File", "F", "Relative or fully qualified path of the INI file you want to read");
            output += Environment.NewLine + string.Format("\t\t\t\t{0}", "a value from or update a value in");
            output += Environment.NewLine + string.Format("\t-{1}\t-{0}\t{2}", "Section", "S", "The name of the section header. Value contained within [ ]");
            output += Environment.NewLine + string.Format("\t-{1}\t-{0}\t\t{2}", "Key", "K", "The key from the Key Value paring. Value contained before the =");
            output += Environment.NewLine + string.Format("\t-{1}\t-{0}\t\t{2}", "Value", "V", "(Optional)Adds or Updates the Value matching the Section Key Pair");

            //Information Commands
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t{0}\t\t\t{1}", "?", "Displays this menu");
            output += Environment.NewLine + string.Format("\t-{1}\t-{0}\t\t{2}", "Help", "H", "Displays this menu");

            //Example file

            output += Environment.NewLine;
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t<Example INI File>");
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t[Credentials]");
            output += Environment.NewLine + string.Format("\tUser=DoeJ");
            output += Environment.NewLine + string.Format("\tPassword=SomePassword");
            output += Environment.NewLine + string.Format("\t[Host]");
            output += Environment.NewLine + string.Format("\tName=LPTDOEJ");

            //Usage
            output += Environment.NewLine;
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t<USAGE>");
            output += Environment.NewLine;

            output += Environment.NewLine + string.Format("\tReturn the value for user from section Credentials in a file named Settings.ini");
            output += Environment.NewLine + string.Format("\t\tINI.EXE -F Settings.ini -S Credentials -K User");
            output += Environment.NewLine;

            output += Environment.NewLine + string.Format("\tAdd or Update the value for user in section Credentials in a file named Settings.ini");
            output += Environment.NewLine + string.Format("\t\tINI.EXE -F Settings.ini -S Credentials -K User -V SmithJ");
            output += Environment.NewLine;

            //Copywrite details
            output += Environment.NewLine;
            output += Environment.NewLine + string.Format("\t*************************************************************************");
            output += Environment.NewLine + string.Format("\t* Copyright (C) 2018 Peter Varney - All Rights Reserved");
            output += Environment.NewLine + string.Format("\t* You may use, distribute and modify this code under the");
            output += Environment.NewLine + string.Format("\t* terms of the MIT license,");
            output += Environment.NewLine + string.Format("\t*");
            output += Environment.NewLine + string.Format("\t* You should have received a copy of the MIT license with");
            output += Environment.NewLine + string.Format("\t* this file. If not, visit : https://github.com/fatalwall/INI_File_Tools");
            output += Environment.NewLine + string.Format("\t*************************************************************************");

            return output;
        }
    }
}
