namespace Orc.DataAccess.Database
{
    using System.Collections.Generic;

    public class DbDataSourceSchema
    {
        public DbDataSourceSchema()
        {
            Databases = new();
        }

        public List<string> Databases { get; set; }
    }
}
