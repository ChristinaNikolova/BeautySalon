namespace BeautySalon.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class IsDateBeforeTodayAttribute : ValidationAttribute
    {
        private readonly DateTime currentDate;

        public IsDateBeforeTodayAttribute()
        {
            this.currentDate = DateTime.UtcNow;
            this.ErrorMessage = $"Date must be greater than {this.currentDate:dd/MM/yyyy}!";
        }

        public override bool IsValid(object value)
        {
            bool isDateValid = true;

            if (value is DateTime dateInput)
            {
                if (dateInput > this.currentDate)
                {
                    return isDateValid;
                }
            }

            return !isDateValid;
        }
    }
}
