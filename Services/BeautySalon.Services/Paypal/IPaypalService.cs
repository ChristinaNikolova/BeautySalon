namespace BeautySalon.Services.Paypal
{
    using System.Threading.Tasks;

    using PayPal.Api;

    public interface IPaypalService
    {
        Task<Payment> CreatePayment(int price);

        Task<Payment> ExecutePayment(string payerId, string paymentId, string token);
    }
}
