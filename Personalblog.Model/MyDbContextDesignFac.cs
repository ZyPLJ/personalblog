using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model
{
    internal class MyDbContextDesignFac: IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MyDbContext> builder = new DbContextOptionsBuilder<MyDbContext>();
            //Personalblog
            string connStr = "Data Source=app.db;Foreign Keys=False";
            //builder.UseMySql(connStr, new MySqlServerVersion(new Version()));
            builder.UseSqlite(connStr);

            MyDbContext ctx = new MyDbContext(builder.Options);
            return ctx;
        }
    }
}
