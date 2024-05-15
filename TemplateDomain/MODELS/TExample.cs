namespace $safeprojectname$.Models
{
    public class TExample : IEntityAuditable
    {
        public int Id { get; set; }
        public string? UserIng { get; set; }
        public DateTime? FecIng { get; set; }
        public string? UserMod { get; set; }
        public DateTime? FecMod { get; set; }
        public string? UserBaja { get; set; }
        public DateTime? FecBaja { get; set; }
        public string? MotivoBaja { get; set; }
        public string? Name { get; set; }
    }
}
