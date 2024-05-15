namespace $safeprojectname$.Sequences
{
    public abstract class Sequence
    {
        private static IConfiguration _configuration;

        private static string DefaultConnection => _configuration.GetConnectionString("DefaultConnection");

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static int ID_EXAMPLE()
        {
            return NextSequenceValue(AppSettings.SequenceConfig.SeqExample, DefaultConnection);
        }

        public static int GetSequenceForType<TEntity>()
        {
            return typeof(TEntity) switch
            {
                _ when typeof(TEntity) == typeof(TExample) => ID_EXAMPLE(),
                _ => throw new ArgumentException($"No sequence defined for type {typeof(TEntity).Name}")
            };
        }

        private static int NextSequenceValue(string sequenceName, string connectionString)
        {
            var querySeq = $"SELECT {sequenceName}.NEXTVAL FROM DUAL";
            var conn = new OracleConnection(connectionString);
            conn.Open();
            var command = new OracleCommand(querySeq, conn);
            var resultado = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return resultado;
        }
    }
}