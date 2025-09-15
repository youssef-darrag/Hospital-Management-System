﻿namespace Hospital.Core.Models
{
    public class Payroll
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = default!;
        public ApplicationUser Employee { get; set; } = default!;
        public string AccountNumber { get; set; } = default!;
        public decimal Salary { get; set; }
        public decimal NetSalary { get; set; }
        public decimal HourlySalary { get; set; }
        public decimal BonusSalary { get; set; }
        public decimal Compensation { get; set; }
    }
}