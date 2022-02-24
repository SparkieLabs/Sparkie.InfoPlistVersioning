namespace Sparkie.InfoPlistVersioning;

static class InfoPlistUpdater
{
    /*
        <key>CFBundleShortVersionString</key>
        <string>1.0</string>
        <key>CFBundleVersion</key>
        <string>1.0</string>
    */

    enum State
    {
        Default,
        CFBundleShortVersionString,
        CFBundleVersion
    }

    public static void Update(string plistPath, string bundleVersion, string bundleShortVersionString)
    {
        var sb = new StringBuilder();
        State state = State.Default;
        foreach (var line in File.ReadAllLines(plistPath))
        {
            if (line.Contains("<string>"))
            {
                switch (state)
                {
                    case State.CFBundleVersion:
                        sb.AppendLine(ReplaceStringValue(line, bundleVersion));
                        break;
                    case State.CFBundleShortVersionString:
                        sb.AppendLine(ReplaceStringValue(line, bundleShortVersionString));
                        break;
                    default:
                        sb.AppendLine(line);
                        continue;
                }
            }
            else
            {
                sb.AppendLine(line);
                if (line.Contains("<key>"))
                {
                    if (line.Contains("CFBundleVersion"))
                        state = State.CFBundleVersion;
                    else if (line.Contains("CFBundleShortVersionString"))
                        state = State.CFBundleShortVersionString;
                    else
                        state = State.Default;
                    continue;
                }
            }
        }

        File.WriteAllText(plistPath, sb.ToString());
    }

    static string ReplaceStringValue(string oldLine, string newValue)
    {
        int start = oldLine.IndexOf(">");
        int end = oldLine.IndexOf("</");

        string newLine = oldLine.Substring(0, start + 1) + newValue + oldLine.Substring(end);
        return newLine;
    }
}
