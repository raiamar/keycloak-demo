public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string Citizenship { get; set; } = string.Empty;
    public string Status { get; set; } = "pending";
    // Status can be "approved", "rejected", or "pending"
}
