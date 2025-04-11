using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoAppV2.Infrastructure.Data
{
    public static class AppDbContextFactory
    {
        public static void AddInMemoryDataBase(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("ToDoAppV2Db"));
        }
    }
}
