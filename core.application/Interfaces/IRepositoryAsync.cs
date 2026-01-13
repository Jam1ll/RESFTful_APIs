using Ardalis.Specification;

namespace core.application.Interfaces
{
    public interface IRepositoryAsync<T> : IRepositoryBase<T> where T : class
    {
        //La interfaz utiliza metodos genéricos
        //ya existentes en Ardalis.Specification para la gestión de datos
    }
}
