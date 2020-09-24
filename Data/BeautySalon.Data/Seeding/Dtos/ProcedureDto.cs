namespace BeautySalon.Data.Seeding.Dtos
{
    public class ProcedureDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string SkinType { get; set; }

        public string[] SkinProblems { get; set; }

        public string[] Products { get; set; }
    }
}
