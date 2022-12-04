### SmallNetUtils
  A bunch of small utils and extensions which I often used in different projects on .NET. 

#### Data classes
* DateInterval: Class to store interval of two DateTime values. Allows to check intersection/including, equality and split interval by DateIntervalType.
* NumsRange: Class to store double range min-max. Contains <b>NumsRangeUtil</b> with few methods to create a list of NumsRange based on rules: by range size, by ranges amount.

#### Extensions
* StringExtension:
  1. <b>ToString</b>: Common 'ToString' method with possibility to change empty value to default text;
  2. <b>FirstCharToUpper</b>: Change first char of input text to Upper;
  3. <b>AddHtmlTag</b>: Wrap string with a HTML tag;
  4. <b>RemoveHtmlTags</b>: Remove all HTML tags from string;
  5. <b>CreateMultiLineByLength</b>: Convert input text to multiline length with lines of specific row length. Preserve words;
  6. <b>CreateMultiLineByDelimiters</b>: Convert input text to multiline separated by delimiters;
  7. <b>Append</b>: Allow to append new line to a StringBuilder with delimeter (like spaces);
  8. <b>ConvertFromBase64</b>: Convert string from Base64 format to an UTF8.
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
  1. <b>IsMidnight</b>: Check if date is midnight
  2. <b>IsMonthStart</b>: Check if date is month start
  3. <b>IsQuarterStart</b>: Check if date is quarter start
  4. <b>IsYearStart</b>: Check if date is year start
  5. <b>GetQuarter</b>: Get date quarter (number/arabic number)
  6. <b>IsStartOfIntervalType</b>: Check if date is start of DateInterval type (day/month/quarter/etc)
  7. <b>AddByType/SubtractByType</b>: Addition/Subtracting amount to a date by DateInterval type
  8. <b>Ceil/Floor</b>: Ceil/floor date by DateInterval
  9. <b>GetDateInterval</b>: Convert DateTime to DateInterval according to DateInterval type
* EnumExtension: Useful methods to work with enums:
  1. Allows to get caption/enum-value by DescriptionAttribute;
  2. Fill different DevExpress editors with enum values.
  ``` csharp
    public enum TestEnum
    {
        [Description("Show angles on the map")]
        Angles,

        [Description("Show axis on the graph")]
        Axis
    }
    
    var caption = TestEnum.Angles.GetCaption();
  ```
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
  5. <b>ValidateFileName</b>: Validate and return valid filename;
  6. <b>GetFileExtension</b>: Get file extension for a file, encoded in Base64 string.
* DateTimeUtil
  1. <b>Min/Max</b>: Get bigger/smaller date between two of them;
  2. <b>MonthsBetween</b>: Get difference between two dates in months;
  3. <b>Concat</b>: Concat two lists of DateTime;
  4. <b>ConvertDateIntervalsToDateTime</b>: Convert list of DateInterval to list of DateTime;
  5. <b>ConvertDateTimeToDateIntervals</b>: Convert list of DateTime to list of DateInterval;
  6. <b>Merge</b>: Merge consecutive dates into one DateInterval.
* FormsUtil
  1. <b>IsDesignMode</b>: Check if a form opened in Designer mode;