using System;

namespace CodacLogistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            ShowHeader();

            bool runAgain = true;
            int driverCount = 0;
            string[] driverNames = new string[10];
            decimal[] driverEfficiencies = new decimal[10];

            while (runAgain && driverCount < 10)
            {
                ShowSectionTitle($"DRIVER {driverCount + 1} REGISTRATION");

                // Driver Profile
                string driverName = GetInput("Enter Driver's Full Name");
                decimal weeklyBudget = GetDecimalInput("Enter Weekly Fuel Budget (in PHP)", 0);
                double totalDistance = GetValidatedDistance();

                // Daily Expenses
                ShowSectionTitle($"DAILY FUEL TRACKING - {driverName.ToUpper()}");
                decimal[] fuelExpenses = GetDailyFuelExpenses();
                decimal totalFuelSpent = CalculateTotal(fuelExpenses);
                double[] dailyDistances = GetDailyDistances();
                double[] dailyEfficiencies = CalculateDailyEfficiencies(dailyDistances, fuelExpenses);

                // Calculations
                decimal averageDailyExpense = totalFuelSpent / 5;
                double efficiencyRatio = CalculateEfficiencyRatio(totalDistance, totalFuelSpent);
                string efficiencyRating = GetEfficiencyRating(efficiencyRatio);
                bool underBudget = totalFuelSpent <= weeklyBudget;

                // Generate Report
                GenerateAuditReport(driverName, weeklyBudget, totalDistance, fuelExpenses,
                                  dailyDistances, dailyEfficiencies, totalFuelSpent,
                                  averageDailyExpense, efficiencyRatio, efficiencyRating, underBudget);

                // Store for comparison
                driverNames[driverCount] = driverName;
                driverEfficiencies[driverCount] = (decimal)efficiencyRatio;
                driverCount++;

                // Continue or Exit
                if (driverCount < 10)
                {
                    runAgain = AskToContinue();
                }
                else
                {
                    Console.WriteLine("\n" + new string('═', 60));
                    Console.WriteLine("⚠️  Maximum of 10 drivers reached.");
                    runAgain = false;
                }
            }

            // Multi-Driver Comparison
            if (driverCount > 1)
            {
                GenerateComparisonReport(driverNames, driverEfficiencies, driverCount);
            }

            ShowFooter();
        }

        // ==================== HELPER METHODS ====================

        static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('═', 60));
            Console.WriteLine("      CODAC LOGISTICS DELIVERY & FUEL AUDITOR");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor();
            Console.WriteLine();
        }

        static void ShowSectionTitle(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"┌{new string('─', 58)}┐");
            Console.WriteLine($"│ {title,-56} │");
            Console.WriteLine($"└{new string('─', 58)}┘");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void ShowSubSection(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"▶ {title}");
            Console.ResetColor();
        }

        static string GetInput(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine();
        }

        static decimal GetDecimalInput(string prompt, decimal minValue)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= minValue)
                {
                    return value;
                }
                ShowError($"Please enter a valid number ≥ {minValue}");
            }
        }

        static double GetValidatedDistance()
        {
            while (true)
            {
                Console.Write("Enter Total Distance Traveled this week (1.0 to 5000.0 km): ");
                if (double.TryParse(Console.ReadLine(), out double distance) &&
                    distance >= 1.0 && distance <= 5000.0)
                {
                    return distance;
                }
                ShowError("Distance must be between 1.0 and 5000.0 km");
            }
        }

        static decimal[] GetDailyFuelExpenses()
        {
            decimal[] expenses = new decimal[5];

            for (int i = 0; i < 5; i++)
            {
                expenses[i] = GetDecimalInput($"  Day {i + 1} fuel cost (PHP)", 0);
            }

            return expenses;
        }

        static double[] GetDailyDistances()
        {
            double[] distances = new double[5];

            for (int i = 0; i < 5; i++)
            {
                while (true)
                {
                    Console.Write($"  Day {i + 1} distance (km): ");
                    if (double.TryParse(Console.ReadLine(), out double distance) && distance >= 0)
                    {
                        distances[i] = distance;
                        break;
                    }
                    ShowError("Please enter a valid distance ≥ 0");
                }
            }

            return distances;
        }

        static double[] CalculateDailyEfficiencies(double[] distances, decimal[] expenses)
        {
            double[] efficiencies = new double[5];

            for (int i = 0; i < 5; i++)
            {
                efficiencies[i] = expenses[i] > 0 ? distances[i] / (double)expenses[i] : 0;
            }

            return efficiencies;
        }

        static decimal CalculateTotal(decimal[] expenses)
        {
            decimal total = 0;
            foreach (var expense in expenses) total += expense;
            return total;
        }

        static double CalculateEfficiencyRatio(double totalDistance, decimal totalFuel)
        {
            return totalFuel > 0 ? totalDistance / (double)totalFuel : 0;
        }

        static string GetEfficiencyRating(double ratio)
        {
            if (ratio > 15) return "High Efficiency";
            if (ratio >= 10) return "Standard Efficiency";
            return "Low Efficiency / Maintenance Required";
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ✗ {message}");
            Console.ResetColor();
        }

        static void GenerateAuditReport(string driverName, decimal weeklyBudget, double totalDistance,
                                      decimal[] fuelExpenses, double[] dailyDistances,
                                      double[] dailyEfficiencies, decimal totalFuelSpent,
                                      decimal averageDailyExpense, double efficiencyRatio,
                                      string efficiencyRating, bool underBudget)
        {
            Console.Clear();
            ShowSectionTitle("AUDIT REPORT SUMMARY");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Driver: {driverName}");
            Console.WriteLine($"Budget: PHP {weeklyBudget:F2}");
            Console.WriteLine($"Total Distance: {totalDistance:F1} km");
            Console.ResetColor();
            Console.WriteLine();

            // Daily Breakdown Table
            ShowSubSection("Daily Expense Breakdown");
            Console.WriteLine(new string('─', 65));
            Console.WriteLine("│ Day │  Fuel Cost  │  Distance  │ Efficiency │ Visual │");
            Console.WriteLine(new string('─', 65));

            for (int i = 0; i < 5; i++)
            {
                string efficiencyLabel = GetEfficiencyLabel(dailyEfficiencies[i]);
                string visualBar = GenerateVisualBar(dailyEfficiencies[i]);

                Console.WriteLine($"│ {i + 1,3} │ PHP {fuelExpenses[i],7:F2} │ {dailyDistances[i],9:F1} km │ {efficiencyLabel,-10} │ {visualBar,-6} │");
            }
            Console.WriteLine(new string('─', 65));
            Console.WriteLine();

            // Summary Section
            ShowSubSection("Performance Summary");
            Console.WriteLine($"Total Fuel Spent:     PHP {totalFuelSpent,10:F2}");
            Console.WriteLine($"Average Daily:        PHP {averageDailyExpense,10:F2}");
            Console.WriteLine($"Efficiency Ratio:     {efficiencyRatio,10:F1} km/PHP");

            Console.ForegroundColor = GetEfficiencyColor(efficiencyRating);
            Console.WriteLine($"Efficiency Rating:    {efficiencyRating,-30}");
            Console.ResetColor();

            Console.ForegroundColor = underBudget ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"Budget Status:        {(underBudget ? "✅ UNDER BUDGET" : "⚠️  OVER BUDGET")}");
            Console.ResetColor();
            Console.WriteLine();

            // Efficiency Trend Chart
            ShowSubSection("Weekly Efficiency Trend");
            DisplayEfficiencyChart(dailyEfficiencies);
        }

        static string GetEfficiencyLabel(double efficiency)
        {
            if (efficiency == 0) return "N/A";
            if (efficiency > 15) return "High";
            if (efficiency >= 10) return "Good";
            return "Low";
        }

        static ConsoleColor GetEfficiencyColor(string rating)
        {
            if (rating.Contains("High")) return ConsoleColor.Green;
            if (rating.Contains("Standard")) return ConsoleColor.Yellow;
            return ConsoleColor.Red;
        }

        static string GenerateVisualBar(double efficiency)
        {
            if (efficiency == 0) return "   -";
            if (efficiency > 15) return "█████";
            if (efficiency >= 10) return "███  ";
            return "█    ";
        }

        static void DisplayEfficiencyChart(double[] efficiencies)
        {
            Console.WriteLine("Day:    " + string.Join("   ", new[] { "1", "2", "3", "4", "5" }));
            Console.Write("Trend:  ");

            foreach (var eff in efficiencies)
            {
                if (eff == 0) Console.Write("  -   ");
                else if (eff > 15) Console.Write("  ▲   ");
                else if (eff >= 10) Console.Write("  ●   ");
                else Console.Write("  ▼   ");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Legend: ▲ High (>15) ● Good (10-15) ▼ Low (<10) - No data");
            Console.WriteLine();
        }

        static bool AskToContinue()
        {
            Console.Write("Add another driver? (yes/no): ");
            string response = Console.ReadLine().ToLower();
            return response == "yes" || response == "y";
        }

        static void GenerateComparisonReport(string[] driverNames, decimal[] efficiencies, int count)
        {
            ShowSectionTitle("MULTI-DRIVER EFFICIENCY COMPARISON");

            Console.WriteLine(new string('═', 70));
            Console.WriteLine(" Rank │ Driver Name            │ Efficiency (km/PHP) │ Rating      ");
            Console.WriteLine(new string('═', 70));

            // Sort by efficiency (simple bubble sort)
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (efficiencies[j] > efficiencies[i])
                    {
                        // Swap efficiencies
                        decimal tempEff = efficiencies[i];
                        efficiencies[i] = efficiencies[j];
                        efficiencies[j] = tempEff;

                        // Swap names
                        string tempName = driverNames[i];
                        driverNames[i] = driverNames[j];
                        driverNames[j] = tempName;
                    }
                }
            }

            for (int i = 0; i < count; i++)
            {
                string rating = GetComparisonRating(efficiencies[i]);
                ConsoleColor color = GetEfficiencyColor(rating + " Efficiency");

                Console.ForegroundColor = color;
                Console.WriteLine($" {i + 1,4} │ {driverNames[i],-22} │ {efficiencies[i],18:F1} │ {rating,-11}");
                Console.ResetColor();
            }

            Console.WriteLine(new string('═', 70));
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"🏆 Most Efficient: {driverNames[0]} ({efficiencies[0]:F1} km/PHP)");
            Console.ResetColor();
        }

        static string GetComparisonRating(decimal efficiency)
        {
            if (efficiency > 15) return "High";
            if (efficiency >= 10) return "Standard";
            return "Low";
        }

        static void ShowFooter()
        {
            Console.WriteLine(new string('═', 60));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("         Thank you for using Codac Logistics Auditor!");
            Console.ResetColor();
            Console.WriteLine(new string('═', 60));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}