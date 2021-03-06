using Chef.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                    try
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
                    catch
                    {
                        MessageBox.Show("Проблема при соединении с базой данных.");
                        return 0;
                    }
                }
            }
        }
    }
}
