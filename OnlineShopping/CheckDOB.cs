using System.ComponentModel.DataAnnotations;

namespace OnlineShopping
{
    public class CheckDOB:ValidationAttribute
    {

        public override bool IsValid(object? value)
        {
            DateTime dt = (DateTime)value;
            if (dt < DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
       
        }

    }
}
