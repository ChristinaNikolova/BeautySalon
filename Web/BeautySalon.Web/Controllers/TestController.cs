namespace BeautySalon.Web.Controllers
{
    using BeautySalon.Web.MLModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;

    public class TestController : BaseController
    {
        private readonly PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine;

        public TestController(PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine)
        {
            this.predictionEngine = predictionEngine;
        }

        [HttpPost]
        public IActionResult LoadQuiz([FromBody] string[] names)
        {
            string result = "";

            for (int i = 0; i < names.Length; i++)
            {
                result += names[i];
            }

            var input = new SkinTypeModelInput()
            {
                Description = result,
            };

            var output = this.predictionEngine.Predict(input);
            ;
            return this.View(output);
        }

        public IActionResult LoadQuiz()
        {
            return this.View();
        }
    }
}
