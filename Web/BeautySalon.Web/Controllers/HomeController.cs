namespace BeautySalon.Web.Controllers
{
    using System.Diagnostics;

    using BeautySalon.Web.MLModels;
    using BeautySalon.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;

    public class HomeController : BaseController
    {
        // HAVE to Remove This!!!!!!
        private readonly PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine;

        public HomeController(PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine)
        {
            this.predictionEngine = predictionEngine;
        }

        public IActionResult TestML()
        {
            var input = new SkinTypeModelInput()
            {
                Description = @"My pores almost invisible and small My skin feel tight and it is screaming for moisture. Like a desert! I need to put moisturizer. Clean in the T zone but dry on the cheeks. Oily in some places dry in others. Only nose and forehead are shining.",
            };

            var output = this.predictionEngine.Predict(input);

            return this.Content(output.Prediction);
        }

        // End Remove!!!!!
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
