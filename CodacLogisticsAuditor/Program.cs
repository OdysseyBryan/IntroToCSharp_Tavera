using System;

namespace CodacLogistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Codac Logistics Delivery & Fuel Auditor ===\n");

            Console.Write("Enter Driver's Full Name: ");
            string driverName = Console.ReadLine();

            Console.Write("Enter Weekly Fuel Budget (in USD): ");
            decimal weeklyBudget = decimal.Parse(Console.ReadLine());

            double totalDistance = 0;
            bool validDistance = false;

            while (!validDistance)
            {
                Console.Write("Enter Total Distance Traveled this week (1.0 to 5000.0 km): ");
                totalDistance = double.Parse(Console.ReadLine());

                if (totalDistance >= 1.0 && totalDistance <= 5000.0)
                {
                    validDistance = true;
                }
                else
                {
                    Console.WriteLine("Error: Distance must be between 1.0 and 5000.0 km. Please try again.");
                }
            }

            decimal[] fuelExpenses = new decimal[5];
            decimal totalFuelSpent = 0;

            Console.WriteLine("\n--- Daily Fuel Expenses (Day 1 to Day 5) ---");

            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Enter fuel cost for Day {i + 1} (in USD): ");
                decimal dailyExpense = decimal.Parse(Console.ReadLine());
                fuelExpenses[i] = dailyExpense;
                totalFuelSpent += dailyExpense;
            }

            decimal averageDailyExpense = totalFuelSpent / 5;

            double efficiencyRatio = 0;
            if (totalFuelSpent > 0)
            {
                efficiencyRatio = totalDistance / (double)totalFuelSpent;
            }

            string efficiencyRating = "";
            if (efficiencyRatio > 15)
            {
                efficiencyRating = "High Efficiency";
            }
            else if (efficiencyRatio >= 10)
            {
                efficiencyRating = "Standard Efficiency";
            }
            else
            {
                efficiencyRating = "Low Efficiency / Maintenance Required";
            }

            bool underBudget = totalFuelSpent <= weeklyBudget;

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("                CODAC LOGISTICS AUDIT REPORT");
            Console.WriteLine(new string('=', 60));

            Console.WriteLine($"Driver: {driverName}");
            Console.WriteLine($"Weekly Budget: ${weeklyBudget:F2}");
            Console.WriteLine($"Total Distance: {totalDistance:F1} km");
            Console.WriteLine(new string('-', 60));

            Console.WriteLine("\nDAILY EXPENSE BREAKDOWN:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("| Day | Fuel Cost  |");
            Console.WriteLine(new string('-', 40));

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"| {i + 1,3} | ${fuelExpenses[i],10:F2} |");
            }
            Console.WriteLine(new string('-', 40));

            Console.WriteLine("\nSUMMARY:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Total Fuel Spent:     ${totalFuelSpent:F2}");
            Console.WriteLine($"Average Daily Expense: ${averageDailyExpense:F2}");
            Console.WriteLine($"Fuel Efficiency Ratio: {efficiencyRatio:F1} km/$");
            Console.WriteLine($"Efficiency Rating:     {efficiencyRating}");
            Console.WriteLine($"Under Budget:          {underBudget}");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Thank you for using Codac Logistics Auditor!");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}