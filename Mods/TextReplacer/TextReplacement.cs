using System;

public class TextReplacement
{
    public string sourceString;
    public string targetString;

    public static TextReplacement FromCSV(string line)
    {
        string[] values = line.Split(',');
        TextReplacement replacement = new TextReplacement();
        replacement.sourceString = values[0].Trim();
        replacement.targetString = values[1].Trim();

        return replacement;
    }
}
