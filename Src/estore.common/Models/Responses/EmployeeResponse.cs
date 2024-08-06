namespace estore.common.Models.Responses;

public class EmployeeResponse
{
    public int EmployeeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public int Age { get; set; }

    public int ServiceYears { get; set; }

    public string ContactNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}
