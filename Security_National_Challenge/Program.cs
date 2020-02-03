using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Security_National_Challenge.Employees;

namespace Security_National_Challenge
{
    class Program
    {
        //Constant Index for reading .txt file
        public const int EMPLOYEE_ID_INDEX = 0;
        public const int FIRST_NAME_INDEX = 1;
        public const int LAST_NAME_INDEX = 2;
        public const int PAY_TYPE_INDEX = 3;
        public const int PAY_RATE_INDEX = 4;
        public const int START_DATE_INDEX = 5;
        public const int STATE_CODE_INDEX = 6;
        public const int HOURS_WORKED_INDEX = 7;

        public const int lenOfAppName = 31; //length of executable name

        //Struct to hold data read from file
        public struct EmployeeInfo
        {
            public string employee_id;
            public string first_name;
            public string last_name;
            public string pay_type;
            public double pay_rate;
            public DateTime start_date;
            public string state_code;
            public double hours_worked;
        }

        public static List<Employee> _Employees = new List<Employee>();
        public static EmployeeInfo[] _EmployeeInfo;

        public static List<string> States_List = new List<string>(){ "CO", "UT", "WY", "NV", "ID", "AZ", "OR", "WA", "NM", "TX" };

        public static Dictionary<string, double> _Median_Hours = new Dictionary<string, double>();
        public static Dictionary<string, double> _Median_Pay = new Dictionary<string, double>();
        public static Dictionary<string, double> _MedianTaxes = new Dictionary<string, double>();




        static void Main(string[] args)
        {
            Console.WriteLine("Thank you for the opportunity to interview with you!");

            Console.Write("Please type the direct path to the Employees.txt file: ");
            string filepath = Console.ReadLine();

            Console.WriteLine("Reading in Employees.txt from " + filepath);
            ReadEmployeeFile(filepath);

            Console.WriteLine("Finished Reading text file.");
            WritePayChecksFile();
            WriteHighestEarnersFile();

            States_List.Sort();
            WriteStateStatsFile();
        }
        /*
         * Calculate pay checks for every line in the text file.
         * The output should be: employee id, first name, last name, gross pay,
         * federal tax, state tax, net pay. Ordered by gross pay (highest to lowest)
         */
        private static bool WritePayChecksFile()
        {

            //Sort the list into highest paid by gross paycheck
            _Employees.Sort(delegate (Employee x, Employee y)
            {
                // Sort by total in descending order
                return y.GetGrossPay().CompareTo(x.GetGrossPay());
            });


            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("PayChecks.txt"))
            {
                //The output should be employee id, first name, last name, gross pay,
                // federal tax, state tax, net pay.Ordered by gross pay highest to lowest
                file.WriteLine("\t\t\t Pay Checks");
                file.WriteLine("\t\t--------------------------");
                file.WriteLine(string.Format("{0,4} {1,-21}   {2,-8:F2}   {3,-7:F2}   {4,-6:F2}   {5,-8:F2}",
                        "ID",
                        "Full Name",
                        "Gross",
                        "Federal",
                        "State",
                        "Net"));

                foreach (Employee employee in _Employees)
                {
                    file.WriteLine(string.Format("{0,-4} {1,-21}  ${2,-8:F2}  ${3,-7:F2}  ${4,-6:F2}  ${5,-8:F2}",
                        employee.GetEmployeeId(),
                        employee.GetFirstName() + " " + employee.GetLastName(),
                        employee.GetGrossPay(),
                        employee.GetFederalTaxPaid(),
                        employee.GetStateTaxPaid(),
                        employee.GetNetPay())); ;
                }
            }
            return true;
        }

        /*
         * A list of the top 15% earners sorted by the number or years worked
         * at the company (highest to lowest), then alphabetically by last name then first.
         * The output should be first name, last name, number of years worked, gross pay
         */
        private static bool WriteHighestEarnersFile()
        {
            //Sort the list into highest paid by gross paycheck
            _Employees.Sort(delegate (Employee x, Employee y)
            {
                // Sort by total in descending order
                return y.GetGrossPay().CompareTo(x.GetGrossPay());
            });

            //Seperated top 15% earners to seperate list to sort by name
            List<Employee> Top15Earners = new List<Employee>();
            for (int i = 0; i < _Employees.Count * 0.15; i++)
            {
                Top15Earners.Add(_Employees[i]);
            }

            //Sort the list into last name and then first name
            Top15Earners.Sort(delegate (Employee x, Employee y)
            {
                // Sort by years worked in descending order
                int sorted = y.GetYearsWorked().CompareTo(x.GetYearsWorked());

                if (sorted == 0)
                {
                    sorted = x.GetLastName().CompareTo(y.GetLastName());
                    if (sorted == 0)
                    {
                        sorted = x.GetFirstName().CompareTo(y.GetFirstName());
                    }
                }
                return sorted;
            });

            //Sort the list by years worked
            Top15Earners.Sort(delegate (Employee x, Employee y)
            {
                // Sort by years worked in descending order
                return y.GetYearsWorked().CompareTo(x.GetYearsWorked());
            });

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("HighestEarners.txt"))
            {
                //The output should be first name, last name, number of years worked, gross pay
                file.WriteLine("\t\t Top Earners by Longevity");
                file.WriteLine("\t\t--------------------------");
                file.WriteLine(string.Format("{0,-21}   {1,-5}   {2,-10}",
                        "Full Name",
                        "Years",
                        "Gross Pay"));
                foreach (Employee employee in Top15Earners)
                {
                    file.WriteLine(string.Format("{0,-21}   {1, -5}    ${2:F2}",
                        employee.GetFirstName() + " " + employee.GetLastName(),
                        employee.GetYearsWorked(),
                        employee.GetGrossPay()));
                }
            }
            return true;
        }

        /*
         * A list of all states with median time worked,
         * median net pay, and total state taxes paid.
         * The output should be state, median time worked,
         * median net pay, state taxes. Ordered by states alphabetically
         */
        private static void WriteStateStatsFile()
        {

            List<double> median_hours = new List<double>();
            List<double> median_netpay = new List<double>();
            List<double> median_taxes = new List<double>();


            //Loop through the list of emlpoyees for each state
            //Each loop will grab the needed statistic to sort by
            //Find the median of the stat since the list is already
            //sorted by hours worked.

            _Employees.Sort(delegate (Employee x, Employee y)
            {
                // Sort by alphabetical order
                return y.GetHoursWorked().CompareTo(x.GetHoursWorked());
            });

            #region Hours_Worked

            foreach (string state in States_List)
            {
                for (int i = 0; i < _Employees.Count; i++)
                {
                    if (_Employees[i].GetStateCode() == state)
                    {
                        median_hours.Add(_Employees[i].GetHoursWorked());
                    }
                }

                //Calculate Median Hours and store in Dictionary by state key
                if (median_hours.Count % 2 != 0)
                {
                    //if middle index is even, get the average of the two middle values
                    _Median_Hours.Add(state,
                        (median_hours[median_hours.Count / 2] +
                         median_hours[(median_hours.Count + 1) / 2])
                         / 2);
                }
                else
                {
                    _Median_Hours.Add(state,
                        median_hours[median_hours.Count / 2]);
                }
            }
            #endregion

            _Employees.Sort(delegate (Employee x, Employee y)
            {
                return x.GetNetPay().CompareTo(y.GetNetPay());
            });

            #region Net Pay
            foreach (string state in States_List)
            {
                for (int i = 0; i < _Employees.Count; i++)
                {
                    if (_Employees[i].GetStateCode() == state)
                    {
                        median_netpay.Add(_Employees[i].GetNetPay());
                    }
                }

                //Calculate Median Hours and store in Dictionary by state key
                if (median_netpay.Count % 2 != 0)
                {
                    //if middle index is even, get the average of the two middle values
                    _Median_Pay.Add(state,
                        (median_netpay[median_netpay.Count / 2] +
                         median_netpay[(median_netpay.Count + 1) / 2])
                         / 2);
                }
                else
                {
                    _Median_Pay.Add(state,
                        median_netpay[median_netpay.Count / 2]);
                }
            }

            #endregion

            _Employees.Sort(delegate (Employee x, Employee y)
            {
                return x.GetStateTaxPaid().CompareTo(y.GetStateTaxPaid());
            });

            #region State Taxes
            foreach (string state in States_List)
            {
                for (int i = 0; i < _Employees.Count; i++)
                {
                    if (_Employees[i].GetStateCode() == state)
                    {
                        median_taxes.Add(_Employees[i].GetStateTaxPaid());
                    }
                }

                //Calculate Median Hours and store in Dictionary by state key
                if (median_taxes.Count % 2 != 0)
                {
                    //if middle index is even, get the average of the two middle values
                    _MedianTaxes.Add(state,
                        (median_taxes[median_taxes.Count / 2] +
                         median_taxes[(median_taxes.Count + 1) / 2])
                         / 2);
                }
                else
                {
                    _MedianTaxes.Add(state,
                        median_taxes[median_taxes.Count / 2]);
                }
            }

            #endregion

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("StateStats.txt"))
            {
                //The output should be state, median time worked,
                //median net pay, state taxes. Ordered by states alphabetically
                file.WriteLine("\t State Median Information");
                file.WriteLine("\t--------------------------");
                file.WriteLine(string.Format("{0,-5} {1,-5} {2,-10} {3,-9}",
                        "State",
                        "Hours",
                        "Net Pay",
                        "State Tax"));

                foreach(string state in States_List)
                {
                    file.WriteLine(string.Format("{0,-5} {1,-5} ${2,-10:F2} ${3,-9:F2}",
                         state,
                        _Median_Hours[state],
                        _Median_Pay[state],
                        _MedianTaxes[state]));
                }

            }
        }

        /*
         * Reads in a text file and stores its data in 2D array 
         */
        private static bool ReadEmployeeFile(string filePath)
        {
            try
            {
                FileStream Employee_File;
                StreamReader Employee_Reader;
                //First see if there is a file to read from
                if (File.Exists(filePath))
                {
                    Employee_File = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Employee_Reader = new StreamReader(Employee_File);
                }
                else
                {
                    Console.WriteLine("Employees.txt could not be found.");
                    return false;
                }

                string line;
                string[] row;
                List<string[]> parsedData = new List<string[]>();
                while ((line = Employee_Reader.ReadLine()) != null)
                {
                    row = line.Split(',');
                    parsedData.Add(row);
                }

                //Now that we have the length of the text file, we can initialize the array with the correct size.
                _EmployeeInfo = new EmployeeInfo[parsedData.Count];

                //Store the csv data into struct for easy callback
                for(int i = 0; i < parsedData.Count; i++)
                {
                    _EmployeeInfo[i].employee_id =parsedData[i][EMPLOYEE_ID_INDEX];
                    _EmployeeInfo[i].first_name = parsedData[i][FIRST_NAME_INDEX];
                    _EmployeeInfo[i].last_name = parsedData[i][LAST_NAME_INDEX];
                    _EmployeeInfo[i].pay_type = parsedData[i][PAY_TYPE_INDEX];
                    _EmployeeInfo[i].pay_rate = Convert.ToDouble(parsedData[i][PAY_RATE_INDEX]);
                    _EmployeeInfo[i].start_date = Convert.ToDateTime(parsedData[i][START_DATE_INDEX]);
                    _EmployeeInfo[i].state_code = parsedData[i][STATE_CODE_INDEX];
                    _EmployeeInfo[i].hours_worked = Convert.ToDouble(parsedData[i][HOURS_WORKED_INDEX]);
                }

                //Create employees based on pay type and store in an array
                for(int i = 0; i < _EmployeeInfo.Length; i++)
                {
                    if(_EmployeeInfo[i].pay_type == "H")
                    {
                        Employee hourlyEmployee = new HourlyEmployee(
                            _EmployeeInfo[i].employee_id,
                            _EmployeeInfo[i].first_name,
                            _EmployeeInfo[i].last_name,
                            _EmployeeInfo[i].start_date,
                            _EmployeeInfo[i].state_code,
                            _EmployeeInfo[i].pay_rate,
                            _EmployeeInfo[i].hours_worked
                            );

                        _Employees.Add(hourlyEmployee);
                    }
                    else
                    {
                        Employee salaryEmployee = new SalaryEmployee(
                            _EmployeeInfo[i].employee_id,
                            _EmployeeInfo[i].first_name,
                            _EmployeeInfo[i].last_name,
                            _EmployeeInfo[i].pay_rate,
                            _EmployeeInfo[i].start_date,
                            _EmployeeInfo[i].state_code,
                            _EmployeeInfo[i].hours_worked);

                        _Employees.Add(salaryEmployee);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private Employee GetEmployeeById(string employeeId)
        {
            Employee requestedEmployee = null;
            for(int i = 0; i < _Employees.Count; i++)
            {
                if(_Employees[i].GetEmployeeId() == employeeId)
                {
                    requestedEmployee = _Employees[i];
                }
            }

            return requestedEmployee ?? null;
        }
    }
}
