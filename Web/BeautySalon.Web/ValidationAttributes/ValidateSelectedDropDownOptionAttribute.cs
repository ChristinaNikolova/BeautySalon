namespace BeautySalon.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class ValidateSelectedDropDownOptionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool isSelectedOptionValid = true;

            if (value is string selectedValue)
            {
                if (!selectedValue.StartsWith("Please select"))
                {
                    return isSelectedOptionValid;
                }
            }

            return !isSelectedOptionValid;
        }
    }
}
