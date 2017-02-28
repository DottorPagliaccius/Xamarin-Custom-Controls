using System;

namespace Xamarin.CustomControls
{
    public class MemberNotFoundException : Exception
    {

        public MemberNotFoundException()
        {
        }

        public MemberNotFoundException(string message) : base(message)
        {
        }

        public MemberNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
