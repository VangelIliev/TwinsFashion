using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IAdminService
    {
        Task<bool> AuthoriseUser(string username, string password);
        Task<bool> RemoveProduct(Guid productId);
        Task<bool> RemoveProductSize(Guid productId, string size);
    }
}
