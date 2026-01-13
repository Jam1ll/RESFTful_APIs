using Ardalis.Specification;

namespace core.application.Interfaces
{
    public interface IReadRepositoryAsync<T> : IReadRepositoryBase<T> where T : class
    {
        //La interfaz utiliza metodos genéricos
        //ya existentes en Ardalis.Specification para la lectura de datos
    }
}
