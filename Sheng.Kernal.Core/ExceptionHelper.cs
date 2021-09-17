using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Kernal
{
    public class ExceptionHelper
    {
        public static string GetMessage(Exception ex)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append(ex.Message);

            GetInnerMessage(ex.InnerException, msg);

            return msg.ToString();
        }

        private static void GetInnerMessage(Exception ex,StringBuilder msg)
        {
            if (ex == null)
                return;

            msg.Append(Environment.NewLine);
            msg.Append(ex.Message);

            GetInnerMessage(ex.InnerException, msg);
        }
    }
}
