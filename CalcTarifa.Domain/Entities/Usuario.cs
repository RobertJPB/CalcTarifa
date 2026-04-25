using CalcTarifa.Domain.Enums;

namespace CalcTarifa.Domain.Entities
{
    // representa un usuario del sistema en el dominio
    public class Usuario
    {
        public string Id { get; }
        public string Email { get; }
        public string NombreCompleto { get; }
        public RolUsuario Rol { get; }

        public Usuario(string id, string email, string nombreCompleto, RolUsuario rol)
        {
            Id = id;
            Email = email;
            NombreCompleto = nombreCompleto;
            Rol = rol;
        }
    }
}
