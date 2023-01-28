namespace Detours.Data.Models.Responses;

public class BookingResponse
{
    public required QueryTourResponse Tour { get; init; }
    
    public required decimal Price { get; init; }
    
    public required DateTimeOffset CreatedAt { get; init; }
}
