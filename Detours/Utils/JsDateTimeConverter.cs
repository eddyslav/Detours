using System.Text.Json;
using System.Text.Json.Serialization;

using System.Globalization;

namespace Detours.Utils;

public class JsDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (!reader.TryGetUInt64(out var unixTimestamp))
		{
			throw new InvalidOperationException("Could not parse a value as long");
		}

		return DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(unixTimestamp));
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset dateTimeOffset, JsonSerializerOptions options)
	{
		var unixTimestamp = dateTimeOffset.ToUnixTimeMilliseconds();
		writer.WriteRawValue(unixTimestamp.ToString(CultureInfo.InvariantCulture));
	}
}
