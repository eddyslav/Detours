using System.Text;
using System.Text.RegularExpressions;

using System.Globalization;

namespace Detours.Core.Utils;

public static class Slugifier
{
	public static string RemoveAccents(string tourName)
	{
		if (string.IsNullOrWhiteSpace(tourName))
		{
			return tourName;
		}

		tourName = tourName.Normalize(NormalizationForm.FormD);
		var chars = tourName
			.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
			.ToArray();

		return new string(chars).Normalize(NormalizationForm.FormC);
	}

	public static string Slugify(string tourName)
	{
		// Remove all accents and make the string lower case.
		var output = RemoveAccents(tourName).ToLower();

		// Remove all special characters from the string.
		output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

		// Remove all additional spaces in favour of just one.
		output = Regex.Replace(output, @"\s+", " ").Trim();

		// Replace all spaces with the hyphen.
		output = Regex.Replace(output, @"\s", "-");

		// Return the slug.  
		return output;
	}
}
