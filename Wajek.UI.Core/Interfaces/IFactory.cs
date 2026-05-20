using Microsoft.EntityFrameworkCore;

namespace Wajek.UI.Core.Interfaces;

public interface IFactory {
    public T CreateDbContext<T>() where T : DbContext;
}