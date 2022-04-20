
using CleanBlog.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Core.Services
{
    /// <summary>
    /// Handles all email operations
    /// </summary>
    public interface IEmailService
    {
        void SendContactNotificationToAdmin(ContactViewModel vm);
    }
}
