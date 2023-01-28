using Ardalis.SmartEnum;

using Microsoft.AspNetCore.Http;

namespace Detours.Core;

public class ErrorCode : SmartEnum<ErrorCode>
{
	public static readonly ErrorCode InternalServerError =
		new ErrorCode("SHP-100", StatusCodes.Status500InternalServerError);

	public static readonly ErrorCode InvalidValue = new ErrorCode("SHP-101", StatusCodes.Status400BadRequest);

	public static readonly ErrorCode UnexpectedServiceResponse = new ErrorCode("SHP-103", StatusCodes.Status502BadGateway);

	public int StatusCode => Value;
	public string StatusName => Name;

	private ErrorCode(string name, int value)
		: base(name, value)
	{
	}
}
