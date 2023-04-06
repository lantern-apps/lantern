using System.Text.RegularExpressions;

namespace Lantern.AsService;

internal static class StringExtensions
{
    private static readonly char[] _escapeGlobChars = new[] { '/', '$', '^', '+', '.', '(', ')', '=', '!', '|' };

    public static Regex? GlobToRegex(this string glob)
    {
        if (string.IsNullOrEmpty(glob))
            return null;

        List<string> tokens = new() { "^" };
        bool inGroup = false;

        for (int i = 0; i < glob.Length; ++i)
        {
            char c = glob[i];
            if (_escapeGlobChars.Contains(c))
            {
                tokens.Add("\\" + c);
                continue;
            }

            if (c == '*')
            {
                char? beforeDeep = i == 0 ? (char?)null : glob[i - 1];
                int starCount = 1;

                while (i < glob.Length - 1 && glob[i + 1] == '*')
                {
                    starCount++;
                    i++;
                }

                char? afterDeep = i >= glob.Length - 1 ? (char?)null : glob[i + 1];
                bool isDeep = starCount > 1 &&
                    (beforeDeep == '/' || beforeDeep == null) &&
                    (afterDeep == '/' || afterDeep == null);
                if (isDeep)
                {
                    tokens.Add("((?:[^/]*(?:\\/|$))*)");
                    i++;
                }
                else
                {
                    tokens.Add("([^/]*)");
                }

                continue;
            }

            switch (c)
            {
                case '?':
                    tokens.Add(".");
                    break;
                case '{':
                    inGroup = true;
                    tokens.Add("(");
                    break;
                case '}':
                    inGroup = false;
                    tokens.Add(")");
                    break;
                case ',':
                    if (inGroup)
                    {
                        tokens.Add("|");
                        break;
                    }

                    tokens.Add("\\" + c);
                    break;
                default:
                    tokens.Add(c.ToString());
                    break;
            }
        }

        tokens.Add("$");
        return new Regex(string.Concat(tokens.ToArray()));
    }

    public static bool EqualUrlString(this string s, string url)
    {
        if (s.EndsWith('/'))
            s = s[..^1];
        if (url.EndsWith('/'))
            url = url[..^1];
        return s.Equals(url);
    }

    public static bool IsExpressionScript(this string script)
    {
        if (string.IsNullOrEmpty(script))
            return false;

        char ch;
        int hits = 0;
        for (int i = 0; i < script.Length; i++)
        {
            ch = script[i];
            if (char.IsWhiteSpace(ch))
                continue;

            if (hits == 0 && ch == '(' ||
                hits == 1 && ch == ')' ||
                hits == 2 && ch == '=' ||
                hits == 3 && ch == '>')
            {
                if (hits == 3)
                    return true;

                hits++;
                continue;
            }
            break;
        }
        return false;
    }

}
