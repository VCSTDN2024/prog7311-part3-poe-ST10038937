using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ST10038937_prog7311_poe1.Data;

namespace ST10038937_prog7311_poe1.Controllers
{
    [Authorize(Roles = "Employee")]
    public class AuditLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? userId, string? action, DateTime? startDate, DateTime? endDate)
        {
            var auditLogs = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                auditLogs = auditLogs.Where(a => a.UserId == userId);
            }

            if (!string.IsNullOrEmpty(action))
            {
                auditLogs = auditLogs.Where(a => a.Action != null && a.Action.Contains(action));
            }

            if (startDate.HasValue)
            {
                auditLogs = auditLogs.Where(a => a.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                auditLogs = auditLogs.Where(a => a.Timestamp <= endDate.Value);
            }

            ViewBag.UserId = userId;
            ViewBag.Action = action;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(await auditLogs.OrderByDescending(a => a.Timestamp).ToListAsync());
        }
    }
}
