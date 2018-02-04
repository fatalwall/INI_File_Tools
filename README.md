# INI File Tools

## INI Library
This is a .Net 4.0 DLL file which can read in an entire INI file and allow easy access to the Section/Key/Value combinations. Values can also be updated and saved back to the source file or another file path.

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
        User=DoeJ
        Password=SomePassword
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
