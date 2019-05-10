# INI File Tools
Regularly working with legacy applications and routinely need to perform some form of automation around reading or updating configurations of these applications. Many use the legacy INI format. Solutions around this are often unable to handle simple issues such as a space in a section name, key, or value. This provides a simple tool that should work in all cases reliably.

## INI Library
This is a .Net 4.0 DLL file which can read in an entire INI file and allow easy access to the Section/Key/Value combinations. Values can also be updated and saved back to the source file or another file path.

### Example Usage
        using vshed.IO.INI;
   
        //Read in file via constructor and loop though Sections and KeyValuePairs
        INIFile file = new INIFile("C:\someFile.ini");
        foreach(Section s in file.Sections)
        { 
                console.WriteLine(s.Name);
                foreach(KeyValuePair e in s.Elements)
                {
                        console.WriteLine(s.Key + " = " + s.Value);
                }
        }

        //Read in file and loop though all Sections, KeyValuePairs, and comments
        file.Read("C:\someOtherFile.ini");        
        foreach(Section s in file.Sections)
        { 
                foreach(string c in file.Sections.Comments) {console.WriteLine(c);}
                console.WriteLine(s.Name);
                foreach(KeyValuePair e in s.Elements)
                {
                        foreach(string c in e.Comments) {console.WriteLine(c);}
                        console.WriteLine(s.Key + " = " + s.Value);
                }
        }

## INI CLI
        INI Command Line Interface
        Designed to read and update INI formatted files

        <COMMANDS>

        -F      -File           Relative or fully qualified path of the INI file you want to read
                                a value from or update a value in
        -S      -Section        The name of the section header. Value contained within [ ]
        -K      -Key            The key from the Key Value paring. Value contained before the =
        -V      -Value          (Optional)Adds or Updates the Value matching the Section Key Pair

        ?                       Displays this menu
        -H      -Help           Displays this menu


        <Example INI File>

        [Credentials]
        ;System login used for accessing the host device
        User=DoeJ
        Password=SomePassword
        
        ;Host details for the end point device
        [Host]
        Name=LPTDOEJ


        <USAGE>

        Return the value for user from section Credentials in a file named Settings.ini
                INI.EXE -F Settings.ini -S Credentials -K User

        Add or Update the value for user in section Credentials in a file named Settings.ini
                INI.EXE -F Settings.ini -S Credentials -K User -V SmithJ


        *************************************************************************
        * Copyright (C) 2018 Peter Varney - All Rights Reserved
        * You may use, distribute and modify this code under the
        * terms of the MIT license,
        *
        * You should have received a copy of the MIT license with
        * this file. If not, visit : https://github.com/fatalwall/INI_File_Tools
        *************************************************************************
