using GeekShopping.Email.Messages;
using GeekShopping.Email.Model.Context;
using GeekShopping.OrderAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public EmailRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResulMessage message)
        {
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfuly"
            };

            await using var _db = new MySQLContext(_context);
            _db.Emails.Add(email);
            await _db.SaveChangesAsync();
        }

    }
}
