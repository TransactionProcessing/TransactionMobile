namespace TransactionProcessor.Mobile.BusinessLogic.Models;

public static class AutomationIdHelper
{
    public static string Create(string prefix, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return prefix;
        }

        var builder = new System.Text.StringBuilder(prefix.Length + value.Length + 1);
        builder.Append(prefix);
        builder.Append('_');

        bool appendedCharacter = false;
        bool lastWasSeparator = false;

        foreach (char character in value.Trim())
        {
            char sanitized = char.IsLetterOrDigit(character) ? character : '_';

            if (sanitized == '_')
            {
                if (lastWasSeparator)
                {
                    continue;
                }

                lastWasSeparator = true;
            }
            else
            {
                lastWasSeparator = false;
            }

            builder.Append(sanitized);
            appendedCharacter = true;
        }

        if (appendedCharacter == false)
        {
            return prefix;
        }

        if (builder.Length > 0 && builder[^1] == '_')
        {
            builder.Length--;
        }

        return builder.ToString();
    }
}
