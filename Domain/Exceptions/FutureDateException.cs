using System;
namespace Domain.Exceptions
{
    public class FutureDateException:GuardException
    {
        public FutureDateException(DateTime date):base(typeof(FutureDateException))
        {
            Properties.Add(nameof(date),date.Date);
            Properties.Add("currentDate",DateTime.Now.Date);
        }

        public FutureDateException(DateTime date, string name):base(typeof(FutureDateException))
        {
            Properties.Add(nameof(date),date.Date);
            Properties.Add("currentDate",DateTime.Now.Date);
            Properties.Add(nameof(name),name);
        }
    }
}