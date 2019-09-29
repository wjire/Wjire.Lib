namespace Wjire.Db
{
    public class ConnectionStringInfo
    {

        public string ConnectionString { get; set; }

        private string _type;
        public string Type
        {
            get => string.IsNullOrWhiteSpace(_type) ? "sql" : _type.ToLower();
            set => _type = value;
        }
    }
}

