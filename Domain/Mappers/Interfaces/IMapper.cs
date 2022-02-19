namespace Domain.Mappers.Interfaces
{
    public interface IMapper<TEntity, TDal>
    {
        TEntity? ToEntity(TDal? dal);
        TDal? ToDal(TEntity? entity);
    }
}
