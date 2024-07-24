using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale
{
    public  class EmailValidation
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
