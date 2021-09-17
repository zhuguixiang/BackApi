using System;

namespace AstuteTec.Infrastructure
{
    public class UpdatePasswordArgs
    {
        public Guid UserId
        {
            get;set;
        }

        public string OldPassword
        {
            get;set;
        }

        public string NewPassword
        {
            get;set;
        }
    }
}
