using Chef.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chef.Shared
{
    class SqlHelper
    {
            public static int getDatabasesCount()
            {
                using (var context = new DatabaseContext())
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "SELECT Distinct TABLE_NAME FROM information_schema.TABLES";
                        command.CommandType = CommandType.Text;

                        context.Database.OpenConnection();

                        using (var result = command.ExecuteReader())
                        {
                        List<string> list = new List<string>();

                        while (result.Read())
                            {
                                list.Add(result[0].ToString());
                            }
                        return list.Count();
                    }
                }
            }
        }
    }
}
