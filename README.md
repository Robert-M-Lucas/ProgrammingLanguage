# ProgrammingLanguage

## VSCode extension

An extension with instructions on how to install it can be found at <https://github.com/Robert-M-Lucas/RLC-VSCode-Extension>

## General info

Running:

To run a program run the exe with the first argument being the main file. All files must end in `.rlc`. Currently to ensure stability make sure the code files are in the same folder as the excecutable

Fomatting:

``` [Symbol] | [arg1] | [arg2] | [...]; ```

Examples:

```rlc
import | math;
let | i | 0;

tag | loop;
    goto | math.count;
    add | i | 1;
    compare | i | 100 | end;
    goto | loop;


tag | end;
    exit;
```

- All whitespace and newlines are removed on processing as well as everythin being converted to lowercase
- 'Lines' are separated semicolons - an error on line 12 means an error before the 12th semicolon
- 'Meta' lines are 'import', 'let' and 'tag' symbols
- Import statements must come first in a file, followed by let statements and then everything else
- All variables persistent - going out and into scope won't change or reset their value
- Use `goto | [filename].[tagname];` to go to a tag in a different file (all files have a default `start` tag that will take you to the first line)
- Use `[arrayname].[index]` to access an item in an array and treat it like any other object

## Symbols

- ``` import | [path to import]; ``` - imports code from an external file
- ``` let | [object name] | [object value]; ``` - creates an object
- ``` arr | [array name] | [array length]; ``` - creates an array with all items 0
- ``` arr | [array name] | [array value (string)]; ``` - creates an array with set values
- ``` tag | [tag name]; ``` - creates a tag
- ``` goto | [tag name]; ``` - goes to tag
- ``` goto | [file_name.tag_name]; ``` - goes to tag in external file (code will continue - running in this file after other file finishes excecuting)
- ``` print | [object or constant] | [1 = no new line]; ``` - outputs a value
- ``` printc | [object or constant] | [1 = no new line]; ``` - outputs an ascii encoded value
- ``` if | [object] | [tag_name]; ``` - goes to tag if object = 1
- ``` compare | [object] | [object or constant] | [tag_name]; ``` - goes to tag if the object is equal to the second argument
- ``` exit; ``` - immediately ends the program from anywhere
- ``` add | [object] | [amount to add]; ``` - adds a value to an object
- ``` invert | [object]; ``` - if object = 0 sets object to 1, else sets object to 0
- ``` skip; ``` - does nothing
- ``` set | [object] | [value]; ``` - sets an object to a value
- ``` setarr | [array name] | [array value]; ``` - changes the contents of an array
