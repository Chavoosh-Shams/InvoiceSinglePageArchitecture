using System.Net;

namespace InvoiceApp.Frameworks.ResponseFrameworks.Contracts
{
    public interface IResponse<T>
    {
        bool IsSuccessful { get; set; }

        HttpStatusCode Status { get; set; }

        string? Message { get; set; }

        T? Value { get; set; }
    }
}
