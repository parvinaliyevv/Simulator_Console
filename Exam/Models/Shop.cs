using System;

namespace Exam.Models
{
    internal sealed class Shop
    {
        private Dictionary<string, Stack<Vegetable>> Stands { get; } = new();

        private Queue<Customer> Customers { get; } = new();

        private List<Employee> Employers { get; } = new();

    }
}
