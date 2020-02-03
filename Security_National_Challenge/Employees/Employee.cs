using System;
using System.Text;

namespace Security_National_Challenge
{
    public class Employee
    {
        //Constant Variables
        public const double UT_WY_NV_TAX_RATE = 0.05;
        public const double CO_ID_AZ_OR_RATE = 0.065;
        public const double WA_NM_TX_RATE = 0.07;
        public const double FEDERAL_TAX_RATE = 0.15;

        private string employee_id;
        private string first_name;
        private string last_name;
        private double gross_pay;
        private double federal_tax_paid;
        private double state_tax_paid;
        private double net_pay;
        private double pay_rate;
        private DateTime start_date;
        private string state_code;
        private double hours_worked;
        private double years_worked;
        

        public Employee() { }

        public Employee(string _id,
            string _firstname,
            string _lastname,
            double _pay_rate,
            DateTime _start_date,
            string _state_code,
            double _hours_worked)
        {
            employee_id = _id;
            first_name = _firstname;
            last_name = _lastname;
            pay_rate = _pay_rate;
            start_date = _start_date;
            state_code = _state_code;
            hours_worked = _hours_worked;

        }

        public double CalculatePayCheck()
        {
            CalculateGrossPay(pay_rate);
            Calculate_Federal_Tax();
            Calculate_State_Tax(state_code);
            //gross_pay - taxes paid;
            net_pay = gross_pay - federal_tax_paid - state_tax_paid;
            return net_pay;
        }

        public virtual double CalculateGrossPay(double pay_rate)
        {
            //Calculate Hourly pay by hours worked, Salary by 26 weeks
            return gross_pay;
        }

        public double Calculate_Federal_Tax()
        {
            //Calculate by gross and federal tax
            return gross_pay * FEDERAL_TAX_RATE;
        }

        public double Calculate_State_Tax(string state)
        {
            if(state == "UT" || state == "WY" || state == "NV")
            {
                state_tax_paid = UT_WY_NV_TAX_RATE * gross_pay;
            }
            else if(state == "CO" || state == "ID" || state == "AZ" || state == "OR")
            {
                state_tax_paid = CO_ID_AZ_OR_RATE * gross_pay;
            }
            else if(state == "WA" || state == "NM" || state == "TX")
            {
                state_tax_paid = WA_NM_TX_RATE * gross_pay;
            }
            else
            {
                Console.WriteLine("Unrecognized state code: " + state + " while calculating state taxes.");
            }
            //Calculate by gross and state tax
            return state_tax_paid;
        }


        public double GetYearsWorked()
        {
            DateTime now = DateTime.Now;

            TimeSpan timeSpan = now.Subtract(start_date);
            double years = Math.Round((timeSpan.TotalDays / 365.25), 0); //365 1/4 days per year
            SetYearsWorked(years);

            return years;
        }

        //Getter and Setter Methods
        public string GetEmployeeId() { return employee_id; }
        public string GetFirstName() { return first_name; }
        public string GetLastName() { return last_name; }
        public string GetStateCode() { return state_code; }
        public double GetHoursWorked() { return hours_worked; }
        public double GetGrossPay() {
            gross_pay = CalculateGrossPay(pay_rate);
            return gross_pay;
        }
        public double GetFederalTaxPaid() {
            federal_tax_paid = Calculate_Federal_Tax();
            return federal_tax_paid;
        }
        public double GetStateTaxPaid() {
            state_tax_paid = Calculate_State_Tax(state_code);
            return state_tax_paid;
        }
        public double GetNetPay() {
            net_pay = CalculatePayCheck();
            return net_pay;
        }
        public DateTime GetStartDate() { return start_date; }

        public override string ToString()
        {
            return String.Format("" +
                "ID:           {0}\n" +
                "Name:         {1}\n" +
                "Pay:          {2}\n" +
                "Start Date:   {3}\n" +
                "State:        {4}\n" +
                "Hours Worked: {5}\n",
                employee_id,
                first_name + " " + last_name,
                pay_rate,
                start_date.ToShortDateString(),
                state_code,
                hours_worked);
        }

        public void SetGrossPay(double g_pay) { gross_pay = g_pay; }
        public void SetFederalTaxPaid(double fed_tax) { federal_tax_paid = fed_tax; }
        public void SetStateTaxPaid(double state_tax) { state_tax_paid = state_tax; }
        public void SetNetPay(double n_pay) { net_pay = n_pay; }
        public void SetYearsWorked(double years) { years_worked = years; }
    }
}
