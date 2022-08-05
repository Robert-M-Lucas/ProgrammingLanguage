# ProgrammingLanguage

## General info

Fomatting:

``` [Symbol] | [arg1] | [arg2] | [...]; ```

Examples:

```
import | math.rlc;
print | 1;
tag | a;
goto | a;
```

- All whitespace and newlines are removed on processing as well as everythin being converted to lowercase
- 'Lines' are separated semicolons - an error on line 12 means an error before the 12th semicolon
- 'Meta' lines are 'import', 'let' and 'tag' symbols
- Import statements must come first in a file, followed by let statements and then everything else
- All variables persistent - going out and into scope won't change or reset their value

## Symbols

- ``` import | [path to import]; ``` - imports code from an external file
- ``` let | [object name] | [object value]; ``` - creates an object
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
