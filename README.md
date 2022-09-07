### SmallNetUtils
  A bunch of small utils and extensions which I often used in different projects on .NET. 

#### Data classes
DateInterval, NumsRange

#### Extensions
* StringExtension:
  1. <b>ToString</b>: Common 'ToString' method with possibility to change empty value to default text;
  2. <b>FirstCharToUpper</b>: Change first char of input text to Upper;
  3. <b>AddHtmlTag</b>: Wrap string with a HTML tag;
  4. <b>RemoveHtmlTags</b>: Remove all HTML tags from string;
  5. <b>CreateMultiLineByLength</b>: Convert input text to multiline length with lines of specific row length. Preserve words;
  6. <b>CreateMultiLineByDelimiters</b>: Convert input text to multiline separated by delimiters.
* ColorExtension:
  1. <b>GetRandomColor</b>: Get nice random color. Use LRU cache to create unique colors;
  2. <b>Lightend, Darken</b>: Tints the color by the given percent;
  3. <b>Invert</b>: Color inversion;
  4. <b>GenerateColorFromString</b>: Generate color based on a string hash;
* DoubleExtension:
  1. <b>Validate</b>: Validate number if it has value (not nan or infinity). If empty - can return string/number default value;
  2. <b>ToString</b>: Get string representation with number validation. If empty - can return string/number default value;
  3. <b>EqualsEpsilon, EqualsEpsilonNaN, EqualsEpsilonNanInf</b>: equal two numbers with epsilon to fix floating-point comparison issue. Supports Nan and Infinitives to compare; 
  6. <b>RoundToSignificant</b>: Round number up to significant digits;
* DateTimeExtension:
* EnumExtension:
#### Utils
* ConvertUtil. Some useful methods to extend a System.Convert library. All convert metods Process Unicode BOM symbols, DBull values.
  1. <b>ToString</b>: Convert an object to string. If object empty - can return string default value;
  2. <b>ToBool</b>: Convert an object/string to boolean. Supports different representation of 'true' value like 1, YES, Y;
  3. <b>ToDateTime</b>: Convert an object/string to DateTime. Supports both: default string representation of DateTime and OLE automation date (Excel);
  4. <b>ToDouble</b>: Convert object/string value to double with CurrentFormatInfo. Supports different delimiters, default value;
  5. <b>ToInt</b>: Convert object/string value to int. Supports different delimiters, default value.
* FileSystemUtil
  1. <b>GetFileEncoding</b>: Determines a text file's encoding by analyzing its byte order mark (BOM);
  2. <b>LaunchFile</b>: Launch file in an assigned application or show it in the Explorer;
  3. <b>OpenFolder</b>: Open a folder in the Explorer;
  4. <b>ShowFileInExplorer</b>: Open the Explorer and focus on a file;
  5. <b>ValidateFileName</b>: Validate and return valid filename.
* DateTimeUtil
  1. <b>Min/Max</b>: Get bigger/smaller date between two of them;
  2. <b>MonthsBetween</b>: Get difference between two dates in months;
  3. <b>Concat</b>: Concat two lists of DateTime;
  4. <b>ConvertDateIntervalsToDateTime</b>: Convert list of DateInterval to list of DateTime;
  5. <b>ConvertDateTimeToDateIntervals</b>: Convert list of DateTime to list of DateInterval;
  6. <b>Merge</b>: Merge consecutive dates into one DateInterval.
* NumsRangeUtil - contains few methods to create list of NumsRange based on rules: by range size, by ranges amount.

