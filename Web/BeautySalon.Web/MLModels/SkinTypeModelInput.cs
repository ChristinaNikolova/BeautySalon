namespace BeautySalon.Web.MLModels
{
    using Microsoft.ML.Data;

    public class SkinTypeModelInput
    {
        [ColumnName("col0")]
        [LoadColumn(0)]
        public string SkinType { get; set; }

        [ColumnName("col1")]
        [LoadColumn(1)]
        public string Description { get; set; }
    }
}
