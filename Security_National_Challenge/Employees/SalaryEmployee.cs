using System;
namespace Security_National_Challenge
{
    public class SalaryEmployee : Employee
    {
        private const int PAYCHECKS_A_YEAR = 26;
        private double salary;

        public SalaryEmployee(
            string _id,
            string _first_name,
            string _last_name,
            double _salary,
            DateTime _start_date,
            string _state_code,
            double _hours_worked
            ) : base(
                  _id,
                  _first_name,
                  _last_name,
                  _salary,
                  _start_date,
                  _state_code,
                  _hours_worked)
        {
            salary = _salary;
        }

        public override double CalculateGrossPay(double pay_rate)
        {
            double gross_pay = salary/PAYCHECKS_A_YEAR;

            return Math.Round(gross_pay, 2, MidpointRounding.AwayFromZero);
        }
    }
}
