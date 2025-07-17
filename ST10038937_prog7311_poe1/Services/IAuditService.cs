namespace ST10038937_prog7311_poe1.Services
{
    public interface IAuditService
    {
        Task LogActionAsync(string userId, string action, string? details = null);
    }
}
