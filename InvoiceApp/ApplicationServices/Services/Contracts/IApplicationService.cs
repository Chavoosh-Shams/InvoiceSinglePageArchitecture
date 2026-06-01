using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;

namespace InvoiceApp.ApplicationServices.Services.Contracts
{
    public interface IApplicationService<TPost, TPut, TDelete, TGetById, TGetAll>
    {
        Task<IResponse<TPost>> PostAsync(TPost obj);

        Task<IResponse<TPut>> PutAsync(TPut obj);

        Task<IResponse<TDelete>> DeleteAsync(TDelete obj);

        Task<IResponse<TGetById>> GetByIdAsync(TGetById obj);

        Task<IResponse<List<TGetAll>>> GetAllAsync();
    }
}
