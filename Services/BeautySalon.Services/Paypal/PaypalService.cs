namespace BeautySalon.Services.Paypal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using PayPal.Api;

    public class PaypalService : IPaypalService
    {
        private readonly IConfiguration configuration;

        public PaypalService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Payment> CreatePayment(int price)
        {
            var apiContext = this.PreparePaypalConfigurations();

            try
            {
                Payment payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            payee = new Payee
                            {
                                email = "softuni-beautysalon@abv.bg",
                            },
                            amount = new Amount
                            {
                                currency = "EUR",
                                total = price.ToString(),
                            },
                            description = "Buying card subscription card for BeautySalon.",
                        },
                    },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = @"https://localhost:44319/Paypal/FailedPayment",
                        return_url = $@"https://localhost:44319/Paypal/SuccessedPayment?price={price}",
                    },
                };

                var createdPayment = await Task.Run(() => payment.Create(apiContext));
                return createdPayment;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Payment> ExecutePayment(string payerId, string paymentId, string token)
        {
            var apiContext = this.PreparePaypalConfigurations();

            PaymentExecution paymentExecution = new PaymentExecution() { payer_id = payerId };
            Payment payment = new Payment() { id = paymentId };
            Payment executedPayment = await Task.Run(() => payment.Execute(apiContext, paymentExecution));

            return executedPayment;
        }

        private APIContext PreparePaypalConfigurations()
        {
            var paypalConfugurations = new Dictionary<string, string>();
            paypalConfugurations.Add("mode", this.configuration["Paypal:Mode"]);
            paypalConfugurations.Add("clientId", this.configuration["Paypal:ClientId"]);
            paypalConfugurations.Add("clientSecret", this.configuration["Paypal:ClientSecret"]);

            var accessToken = new OAuthTokenCredential(paypalConfugurations).GetAccessToken();

            var apiContext = new APIContext(accessToken)
            {
                Config = paypalConfugurations,
            };

            return apiContext;
        }
    }
}
