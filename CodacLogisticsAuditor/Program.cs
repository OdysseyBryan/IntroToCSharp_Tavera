using System;

namespace CodacLogistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Codac Logistics Delivery & Fuel Auditor ===\n");

            bool runAgain = true;
            int driverCount = 0;
            string[] driverNames = new string[10];
            decimal[] driverEfficiencies = new decimal[10];

            while (runAgain && driverCount < 10)
            {
                Console.WriteLine($"\n--- Driver {driverCount + 1} Profile ---");

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
                double[] dailyDistances = new double[5];
                double[] dailyEfficiencies = new double[5];

                Console.WriteLine("\n--- Daily Fuel Expenses (Day 1 to Day 5) ---");

                for (int i = 0; i < 5; i++)
                {
                    bool validExpense = false;
                    decimal dailyExpense = 0;

                    while (!validExpense)
                    {
                        Console.Write($"Enter fuel cost for Day {i + 1} (in USD): ");
                        dailyExpense = decimal.Parse(Console.ReadLine());

                        if (dailyExpense >= 0)
                        {
                            validExpense = true;
                        }
                        else
                        {
                            Console.WriteLine("Error: Fuel expense cannot be negative. Please try again.");
                        }
                    }

                    fuelExpenses[i] = dailyExpense;
                    totalFuelSpent += dailyExpense;

                    Console.Write($"Enter distance for Day {i + 1} (in km): ");
                    double dailyDistance = double.Parse(Console.ReadLine());
                    dailyDistances[i] = dailyDistance;

                    if (dailyExpense > 0)
                    {
                        dailyEfficiencies[i] = dailyDistance / (double)dailyExpense;
                    }
                    else
                    {
                        dailyEfficiencies[i] = 0;
                    }
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
                Console.WriteLine("| Day | Fuel Cost | Distance | Efficiency |");
                Console.WriteLine(new string('-', 40));

                for (int i = 0; i < 5; i++)
                {
                    string efficiencyLabel = "N/A";
                    if (dailyEfficiencies[i] > 15) efficiencyLabel = "High";
                    else if (dailyEfficiencies[i] >= 10) efficiencyLabel = "Good";
                    else if (dailyEfficiencies[i] > 0) efficiencyLabel = "Low";

                    Console.WriteLine($"| {i + 1,3} | ${fuelExpenses[i],9:F2} | {dailyDistances[i],8:F1} km | {efficiencyLabel,10} |");
                }
                Console.WriteLine(new string('-', 40));

                Console.WriteLine("\nSUMMARY:");
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"Total Fuel Spent:     ${totalFuelSpent:F2}");
                Console.WriteLine($"Average Daily Expense: ${averageDailyExpense:F2}");
                Console.WriteLine($"Fuel Efficiency Ratio: {efficiencyRatio:F1} km/$");
                Console.WriteLine($"Efficiency Rating:     {efficiencyRating}");
                Console.WriteLine($"Under Budget:          {underBudget}");

                Console.WriteLine("\nDAILY EFFICIENCY TREND:");
                Console.Write("Day:  ");
                for (int i = 0; i < 5; i++) Console.Write($"{i + 1,6} ");
                Console.Write("\nEff:  ");
                for (int i = 0; i < 5; i++) Console.Write($"{dailyEfficiencies[i],5:F1} ");
                Console.WriteLine();

                Console.Write("Trend: ");
                for (int i = 0; i < 5; i++)
                {
                    if (dailyEfficiencies[i] == 0) Console.Write("   -  ");
                    else if (dailyEfficiencies[i] > 15) Console.Write("  ▲  ");
                    else if (dailyEfficiencies[i] >= 10) Console.Write("  ●  ");
                    else Console.Write("  ▼  ");
                }
                Console.WriteLine("\n");

                driverNames[driverCount] = driverName;
                driverEfficiencies[driverCount] = (decimal)efficiencyRatio;
                driverCount++;

                if (driverCount < 10)
                {
                    Console.Write("Do you want to enter another driver? (yes/no): ");
                    string response = Console.ReadLine().ToLower();

                    if (response != "yes" && response != "y")
                    {
                        runAgain = false;
                    }
                }
                else
                {
                    Console.WriteLine("Maximum of 10 drivers reached.");
                    runAgain = false;
                }
            }

            if (driverCount > 1)
            {
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("           MULTI-DRIVER EFFICIENCY COMPARISON");
                Console.WriteLine(new string('=', 60));
                Console.WriteLine("\n| No. | Driver Name          | Efficiency | Rating        |");
                Console.WriteLine(new string('-', 60));

                for (int i = 0; i < driverCount; i++)
                {
                    string rating = "";
                    if (driverEfficiencies[i] > 15) rating = "High";
                    else if (driverEfficiencies[i] >= 10) rating = "Standard";
                    else rating = "Low";

                    Console.WriteLine($"| {i + 1,3} | {driverNames[i],-20} | {driverEfficiencies[i],10:F1} | {rating,-12} |");
                }
                Console.WriteLine(new string('-', 60));

                decimal highestEfficiency = 0;
                string bestDriver = "";

                for (int i = 0; i < driverCount; i++)
                {
                    if (driverEfficiencies[i] > highestEfficiency)
                    {
                        highestEfficiency = driverEfficiencies[i];
                        bestDriver = driverNames[i];
                    }
                }

                Console.WriteLine($"\nMost Efficient Driver: {bestDriver} ({highestEfficiency:F1} km/$)");
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Thank you for using Codac Logistics Auditor!");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}