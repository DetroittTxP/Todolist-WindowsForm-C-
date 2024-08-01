using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolist
{
    internal class ConnectDB
    {

        public static string ConnectString()
        {
            return "Server=DESKTOP-E8F1SPO\\SQLEXPRESS; Database=Todolist; Trusted_Connection=True; TrustServerCertificate=True";
        }
    }
}
