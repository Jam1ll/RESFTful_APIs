using core.domain.Common;

namespace core.domain.Entities
{
    public class Cliente : AuditableBaseEntity
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public required string Direccion { get; set; }
        public int Edad
        {
            get
            {
                if (_edad <= 0)
                {
                    _edad = new DateTime(DateTime.Now.Subtract(FechaNacimiento).Ticks).Year - 1;
                }
                return _edad;
            }
            set
            {
                _edad = value;
            }
        }

        private int _edad;
    }
}
