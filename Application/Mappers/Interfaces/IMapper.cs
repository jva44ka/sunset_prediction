namespace Application.Mappers.Interfaces;

public interface IMapper<TEntity, TDto>
{
    TEntity? ToEntity(TDto? dal);
    TDto? ToDto(TEntity? entity);
}