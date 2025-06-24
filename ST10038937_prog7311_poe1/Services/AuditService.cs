using ST10038937_prog7311_poe1.Data;
using ST10038937_prog7311_poe1.Models;

namespace ST10038937_prog7311_poe1.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string userId, string action, string? details = null)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(action))
            {
                return; 
            }

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
