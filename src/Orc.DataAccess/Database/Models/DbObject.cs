namespace Orc.DataAccess.Database
{
    public class DbObject
    {
        #region Constructors
        public DbObject(TableType type)
        {
            Type = type;
        }
        #endregion

        #region Properties
        public string Name { get; set; }
        public TableType Type { get; }
        #endregion
    }
}
