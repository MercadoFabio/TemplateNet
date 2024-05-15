namespace $safeprojectname$
{
    /// <summary>
    /// Clase que contiene la configuración de la aplicación.
    /// </summary>
    public abstract class AppSettings
    {

        /// <summary>
        /// Clase que contiene la configuración de la base de datos.
        /// </summary>
        public class ConnectionString
        {
            /// <summary>
            /// Ejemplo de configuración de la base de datos.
            /// </summary>
            public static string? Example { get; set; }
        }

        /// <summary>
        /// Clase que contiene la configuración de JWT.
        /// </summary>
        public class Jwt
        {
            /// <summary>
            /// Clave secreta para la generación y validación de tokens JWT.
            /// </summary>
            public static string SecretKey { get; set; } = " ";

            /// <summary>
            /// Emisor del token JWT.
            /// </summary>
            public static string? Issuer { get; set; }

            /// <summary>
            /// Audiencia del token JWT.
            /// </summary>
            public static string? Audience { get; set; }

            /// <summary>
            /// Duración en minutos del token JWT.
            /// </summary>
            public static int Minutes { get; set; }
        }

        /// <summary>
        /// Clase que contiene la configuración de Amazon S3.
        /// </summary>
        public class AmazonS3
        {
            /// <summary>
            /// Access Key ID de Amazon S3.
            /// </summary>
            public static string AccessKeyId { get; set; }

            /// <summary>
            /// Secret Key ID de Amazon S3.
            /// </summary>
            public static string SecretKeyId { get; set; }

            /// <summary>
            /// Nombre de Amazon S3.
            /// </summary>
            public static string Name { get; set; }

            /// <summary>
            /// Región de Amazon S3.
            /// </summary>
            public static string Region { get; set; }
        }

        /// <summary>
        /// Clase que contiene la configuración de la ruta de Amazon S3.
        /// </summary>
        public class RutaS3
        {
            /// <summary>
            /// Ruta de Amazon S3.
            /// </summary>
            public static string RutaExample { get; set; }
        }

        /// <summary>
        /// Clase que contiene la configuración de SMTP.
        /// </summary>
        public class Smtp
        {
            /// <summary>
            /// Host de SMTP.
            /// </summary>
            public static string Host { get; set; }

            /// <summary>
            /// Nombre de SMTP.
            /// </summary>
            public static string Name { get; set; }

            /// <summary>
            /// Contraseña de SMTP.
            /// </summary>
            public static string Password { get; set; }

            /// <summary>
            /// Puerto de SMTP.
            /// </summary>
            public static int Port { get; set; }

            /// <summary>
            /// Nombre de usuario de SMTP.
            /// </summary>
            public static string UserName { get; set; }

            /// <summary>
            /// Dirección de correo electrónico de SMTP.
            /// </summary>
            public static string Address { get; set; }

            /// <summary>
            /// Autenticación de SMTP.
            /// </summary>
            public static int Authentication { get; set; }
        }
        /// <summary>
        /// Configuración de las secuencias para base de datos con oracle.
        /// </summary>
        public class SequenceConfig
        {
            public static string? SeqExample { get; set; }
        }
    }
}