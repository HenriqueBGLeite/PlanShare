using System.Globalization;

namespace PlanShare.App.Converters;

public class NameToAvatarNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return string.Empty;

        var name = value.ToString()!.Trim();

        //Henrique Batista

        var names = name.Split(' ');

        //0 - Henrique
        //1 - Batista

        //0 - H
        //1 - E
        //2 - N

        if (names.Length == 1)
            return names[0][0].ToString().ToUpper();

        return $"{names[0][0]}{names[1][0]}".ToUpper();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value;
}
