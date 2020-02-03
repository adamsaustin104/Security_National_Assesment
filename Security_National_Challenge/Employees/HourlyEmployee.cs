using System;
namespace Security_National_Challenge.Employees
{
    public class HourlyEmployee : Employee
    {
        private const double OVERTIME_RATE = 1.5;
        private const double DOUBLE_TIME_RATE = 1.75;
        private const int NORMAL_HOURS = 80;
        private const int OVERTIME_HOURS = 10;

        private double normal_hours_worked;
        private double overtime_hours_worked;
        private double double_time_hours_worked;

        public HourlyEmployee(
            string _id,
            string _first_name,
            string _last_name,
            DateTime _start_date,
            string _state_code,
            double _pay_rate,
            double _hours_worked
            ) : base(
                  _id,
                  _first_name,
                  _last_name,
                  _pay_rate,
                  _start_date,
                  _state_code,
                  _hours_worked)
        {
            if(_hours_worked > NORMAL_HOURS)
            {
                normal_hours_worked = NORMAL_HOURS;
                overtime_hours_worked = _hours_worked - NORMAL_HOURS;
            }
            else
            {
                normal_hours_worked = _hours_worked;
            }
            if(overtime_hours_worked > OVERTIME_HOURS)
            {
                overtime_hours_worked = OVERTIME_HOURS;
                double_time_hours_worked = _hours_worked - OVERTIME_HOURS - NORMAL_HOURS;
            }
        }

        public override double CalculateGrossPay(double pay_rate)
        {
            double gross_pay =
                (normal_hours_worked * pay_rate) +
                (overtime_hours_worked * OVERTIME_RATE) +
                (double_time_hours_worked * DOUBLE_TIME_RATE);

            SetGrossPay(gross_pay);

            return Math.Round(gross_pay, 2, MidpointRounding.AwayFromZero);
        }
    }
}

