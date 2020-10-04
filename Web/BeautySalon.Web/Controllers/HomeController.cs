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
                Description = @"My skin is very shiny - it's like a diamond. Oily in some places dry in others. Clean in the T zone but dry on the cheeks. My pores are very small, but not visible Dull everywhere.",
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
