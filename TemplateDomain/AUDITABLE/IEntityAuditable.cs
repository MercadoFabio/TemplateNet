using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Auditable
{
    /// <summary>
    /// Interfaz para realizar las auditorias de las entities.
    /// </summary>
    public interface IEntityAuditable
    {
        public int Id { get; set; }

        public string? UserIng { get; set; }

        public DateTime? FecIng { get; set; }

        public string? UserMod { get; set; }

        public DateTime? FecMod { get; set; }

        public string? UserBaja { get; set; }

        public DateTime? FecBaja { get; set; }
        //TODO: Revisar si es necesario agregar el motivo de baja
        // public string? MotivoBaja { get; set; }
    }
}