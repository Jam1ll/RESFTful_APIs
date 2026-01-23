using Ardalis.Specification;
using core.domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace core.application.Specifications
{
    public class PagedClientesSpecification : Specification<Cliente>
    {
        public PagedClientesSpecification(int pageSize, int pageNumber, string nombre, string apellido)
        {
            Query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (!string.IsNullOrEmpty(nombre))
                Query.Search(c => c.Nombre, "%" + nombre + "%");

            if (!string.IsNullOrEmpty(apellido))
                Query.Search(c => c.Apellido, "%" + apellido + "%");
        }
    }
}
