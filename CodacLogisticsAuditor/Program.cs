/*
 * CODAC LOGISTICS DELIVERY & FUEL AUDITOR
 * Developed by: Rhence Bryan Tavera
 * Date: February 2026
 * 
 * PROGRAM DESCRIPTION:
 * This console application tracks daily fuel expenses and delivery performance
 * for Codac Logistics delivery drivers over a 5-day work week.
 * 
 * FEATURES:
 * 1. Driver profile collection with validation
 * 2. Weekly distance validation (1.0-5000.0 km)
 * 3. 5-day fuel expense tracking using arrays
 * 4. Fuel efficiency calculation and rating
 * 5. Budget compliance checking
 * 6. Professional audit report generation
 * 7. Multi-driver comparison (bonus feature)
 * 
 * TECHNICAL REQUIREMENTS IMPLEMENTED:
 * - Data Types: string, int, double, decimal, bool
 * - I/O: Console.ReadLine() with string interpolation
 * - Validation: while loops for input validation
 * - Data Structure: 1D arrays for 5-day expenses
 * - Control Flow: for loops and if/else statements
 */

using System;

namespace CodacLogistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear(); // Linisin ang console screen
            ShowHeader(); // Ipakita ang program header

            // Mga variables para sa multi-driver tracking
            bool runAgain = true; // Flag para sa pagpapatuloy ng program
            int driverCount = 0; // Bilang ng mga driver na na-input
            string[] driverNames = new string[10]; // Array para sa 10 drivers maximum
            decimal[] driverEfficiencies = new decimal[10]; // Array para sa efficiency ratings

            // Pangunahing loop para sa multiple drivers (hanggang 10 drivers)
            while (runAgain && driverCount < 10)
            {
                // TASK 1: Driver Registration
                ShowSectionTitle($"DRIVER {driverCount + 1} REGISTRATION");

                // Kunin ang driver profile
                string driverName = GetInput("Enter Driver's Full Name");
                // Gumamit ng decimal para sa pera para precise ang calculation
                decimal weeklyBudget = GetDecimalInput("Enter Weekly Fuel Budget (in PHP)", 0);
                // Gumamit ng while loop para sa validation ng distance
                double totalDistance = GetValidatedDistance();

                // TASK 2: Daily Fuel Expense Tracking
                ShowSectionTitle($"DAILY FUEL TRACKING - {driverName.ToUpper()}");
                decimal[] fuelExpenses = GetDailyFuelExpenses(); // Array para sa 5 days
                decimal totalFuelSpent = CalculateTotal(fuelExpenses);
                double[] dailyDistances = GetDailyDistances(); // Array para sa daily distances
                double[] dailyEfficiencies = CalculateDailyEfficiencies(dailyDistances, fuelExpenses);

                // TASK 3: Performance Analysis
                decimal averageDailyExpense = totalFuelSpent / 5;
                // Efficiency calculation: distance ÷ fuel cost
                double efficiencyRatio = CalculateEfficiencyRatio(totalDistance, totalFuelSpent);
                // Gumamit ng if/else para sa rating classification
                string efficiencyRating = GetEfficiencyRating(efficiencyRatio);
                bool underBudget = totalFuelSpent <= weeklyBudget; // Boolean para sa budget status

                // TASK 4: Audit Report Generation
                GenerateAuditReport(driverName, weeklyBudget, totalDistance, fuelExpenses,
                                  dailyDistances, dailyEfficiencies, totalFuelSpent,
                                  averageDailyExpense, efficiencyRatio, efficiencyRating, underBudget);

                // I-store ang data para sa multi-driver comparison
                driverNames[driverCount] = driverName;
                driverEfficiencies[driverCount] = (decimal)efficiencyRatio;
                driverCount++;

                // Tanungin kung gusto pang mag-add ng driver
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

            // Bonus Feature: Multi-Driver Comparison
            if (driverCount > 1)
            {
                GenerateComparisonReport(driverNames, driverEfficiencies, driverCount);
            }

            ShowFooter();
        }

        // ==================== HELPER METHODS ====================

        // Method para ipakita ang program header
        static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // Cyan color para sa header
            Console.WriteLine(new string('═', 60));
            Console.WriteLine("      CODAC LOGISTICS DELIVERY & FUEL AUDITOR");
            Console.WriteLine(new string('═', 60));
            Console.ResetColor(); // I-reset ang color para sa normal text
            Console.WriteLine();
        }

        // Method para sa mga section titles (may box borders)
        static void ShowSectionTitle(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow; // Dilaw para sa titles
            Console.WriteLine($"┌{new string('─', 58)}┐");
            Console.WriteLine($"│ {title,-56} │");
            Console.WriteLine($"└{new string('─', 58)}┘");
            Console.ResetColor();
            Console.WriteLine();
        }

        // Method para sa mga sub-section titles
        static void ShowSubSection(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Berde para sa sub-sections
            Console.WriteLine($"▶ {title}");
            Console.ResetColor();
        }

        // Method para kumuha ng string input (walang validation needed)
        static string GetInput(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine();
        }

        // Method para kumuha ng decimal input WITH VALIDATION
        static decimal GetDecimalInput(string prompt, decimal minValue)
        {
            // WHILE LOOP: Magtatanong hanggang sa makuha ang valid input
            while (true)
            {
                Console.Write($"{prompt}: ");
                // TryParse: Safe na paraan para i-convert ang string to decimal
                // out parameter: magsa-store ng result kung successful
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= minValue)
                {
                    return value; // Valid input, exit na ng loop
                }
                ShowError($"Please enter a valid number ≥ {minValue}");
            }
        }

        // Method para sa distance validation (1.0 to 5000.0 km)
        static double GetValidatedDistance()
        {
            // WHILE LOOP para sa persistent validation
            while (true)
            {
                Console.Write("Enter Total Distance Traveled this week (1.0 to 5000.0 km): ");
                // Gumamit ng double para sa distance (kailangan ng decimal points)
                if (double.TryParse(Console.ReadLine(), out double distance) &&
                    distance >= 1.0 && distance <= 5000.0)
                {
                    return distance; // Valid distance, exit na
                }
                ShowError("Distance must be between 1.0 and 5000.0 km");
            }
        }

        // Method para kumuha ng 5 days fuel expenses (gamit ang array)
        static decimal[] GetDailyFuelExpenses()
        {
            decimal[] expenses = new decimal[5]; // 1D array para sa 5 days

            // FOR LOOP: Uulitin ng 5 beses para sa 5 days
            for (int i = 0; i < 5; i++)
            {
                // (i + 1) logic: i=0 → "Day 1", i=1 → "Day 2", etc.
                expenses[i] = GetDecimalInput($"  Day {i + 1} fuel cost (PHP)", 0);
            }

            return expenses;
        }

        // Method para kumuha ng daily distances
        static double[] GetDailyDistances()
        {
            double[] distances = new double[5]; // Array para sa 5 days distances

            for (int i = 0; i < 5; i++)
            {
                // Nested while loop para sa distance validation
                while (true)
                {
                    Console.Write($"  Day {i + 1} distance (km): ");
                    if (double.TryParse(Console.ReadLine(), out double distance) && distance >= 0)
                    {
                        distances[i] = distance;
                        break; // Exit sa inner while loop
                    }
                    ShowError("Please enter a valid distance ≥ 0");
                }
            }

            return distances;
        }

        // Method para kalkulahin ang daily efficiencies
        static double[] CalculateDailyEfficiencies(double[] distances, decimal[] expenses)
        {
            double[] efficiencies = new double[5];

            for (int i = 0; i < 5; i++)
            {
                // Formula: distance ÷ fuel cost (km per PHP)
                // Ternary operator: check kung > 0 bago mag-divide
                efficiencies[i] = expenses[i] > 0 ? distances[i] / (double)expenses[i] : 0;
            }

            return efficiencies;
        }

        // Method para kalkulahin ang total expenses
        static decimal CalculateTotal(decimal[] expenses)
        {
            decimal total = 0;
            // foreach loop para i-add lahat ng expenses
            foreach (var expense in expenses) total += expense;
            return total;
        }

        // Method para kalkulahin ang overall efficiency ratio
        static double CalculateEfficiencyRatio(double totalDistance, decimal totalFuel)
        {
            // Check muna kung hindi zero bago mag-divide (avoid division by zero)
            return totalFuel > 0 ? totalDistance / (double)totalFuel : 0;
        }

        // Method para makuha ang efficiency rating (High/Standard/Low)
        static string GetEfficiencyRating(double ratio)
        {
            // IF/ELSE statements para sa rating classification
            if (ratio > 15) return "High Efficiency";          // Magaling, matipid
            if (ratio >= 10) return "Standard Efficiency";     // Normal, acceptable
            return "Low Efficiency / Maintenance Required";    // May problema, kailangan ng maintenance
        }

        // Method para magpakita ng error messages
        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Pulang color para sa errors
            Console.WriteLine($"  ✗ {message}");
            Console.ResetColor();
        }

        // Main method para sa audit report generation
        static void GenerateAuditReport(string driverName, decimal weeklyBudget, double totalDistance,
                                      decimal[] fuelExpenses, double[] dailyDistances,
                                      double[] dailyEfficiencies, decimal totalFuelSpent,
                                      decimal averageDailyExpense, double efficiencyRatio,
                                      string efficiencyRating, bool underBudget)
        {
            Console.Clear(); // Linisin ang screen para sa clean report
            ShowSectionTitle("AUDIT REPORT SUMMARY");

            // Display ng basic driver info
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Driver: {driverName}");
            Console.WriteLine($"Budget: PHP {weeklyBudget:F2}");   // F2 = 2 decimal places
            Console.WriteLine($"Total Distance: {totalDistance:F1} km"); // F1 = 1 decimal place
            Console.ResetColor();
            Console.WriteLine();

            // ===== DAILY BREAKDOWN TABLE =====
            ShowSubSection("Daily Expense Breakdown");
            Console.WriteLine(new string('─', 65));
            Console.WriteLine("│ Day │  Fuel Cost  │  Distance  │ Efficiency │ Visual │");
            Console.WriteLine(new string('─', 65));

            for (int i = 0; i < 5; i++)
            {
                string efficiencyLabel = GetEfficiencyLabel(dailyEfficiencies[i]);
                string visualBar = GenerateVisualBar(dailyEfficiencies[i]);

                // String interpolation at formatting para sa aligned table
                // {i + 1,3} = Day number, right-aligned sa 3 spaces
                // {fuelExpenses[i],7:F2} = PHP amount, 7 spaces, 2 decimals
                Console.WriteLine($"│ {i + 1,3} │ PHP {fuelExpenses[i],7:F2} │ {dailyDistances[i],9:F1} km │ {efficiencyLabel,-10} │ {visualBar,-6} │");
            }
            Console.WriteLine(new string('─', 65));
            Console.WriteLine();

            // ===== PERFORMANCE SUMMARY =====
            ShowSubSection("Performance Summary");
            Console.WriteLine($"Total Fuel Spent:     PHP {totalFuelSpent,10:F2}");
            Console.WriteLine($"Average Daily:        PHP {averageDailyExpense,10:F2}");
            Console.WriteLine($"Efficiency Ratio:     {efficiencyRatio,10:F1} km/PHP");

            // Color coding para sa efficiency rating
            Console.ForegroundColor = GetEfficiencyColor(efficiencyRating);
            Console.WriteLine($"Efficiency Rating:    {efficiencyRating,-30}");
            Console.ResetColor();

            // Color coding para sa budget status (green or red)
            Console.ForegroundColor = underBudget ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"Budget Status:        {(underBudget ? "✅ UNDER BUDGET" : "⚠️  OVER BUDGET")}");
            Console.ResetColor();
            Console.WriteLine();

            // ===== EFFICIENCY TREND CHART =====
            ShowSubSection("Weekly Efficiency Trend");
            DisplayEfficiencyChart(dailyEfficiencies);
        }

        // Method para makuha ang efficiency label (High/Good/Low/N/A)
        static string GetEfficiencyLabel(double efficiency)
        {
            if (efficiency == 0) return "N/A";              // Walang data
            if (efficiency > 15) return "High";            // Sobrang ganda
            if (efficiency >= 10) return "Good";           // Okay lang
            return "Low";                                  // Mababa, may problema
        }

        // Method para makuha ang color base sa rating
        static ConsoleColor GetEfficiencyColor(string rating)
        {
            if (rating.Contains("High")) return ConsoleColor.Green;      // Berde para sa maganda
            if (rating.Contains("Standard")) return ConsoleColor.Yellow; // Dilaw para sa warning
            return ConsoleColor.Red;                                     // Pula para sa problema
        }

        // Method para gumawa ng visual bar (ASCII art)
        static string GenerateVisualBar(double efficiency)
        {
            if (efficiency == 0) return "   -";    // No data
            if (efficiency > 15) return "█████";   // Full bar (excellent)
            if (efficiency >= 10) return "███  ";  // Medium bar (good)
            return "█    ";                        // Small bar (needs improvement)
        }

        // Method para ipakita ang efficiency trend chart
        static void DisplayEfficiencyChart(double[] efficiencies)
        {
            // Display ng day numbers
            Console.WriteLine("Day:    " + string.Join("   ", new[] { "1", "2", "3", "4", "5" }));
            Console.Write("Trend:  ");

            // Display ng trend arrows para sa bawat day
            foreach (var eff in efficiencies)
            {
                if (eff == 0) Console.Write("  -   ");      // No data
                else if (eff > 15) Console.Write("  ▲   "); // Up arrow (improving)
                else if (eff >= 10) Console.Write("  ●   "); // Circle (stable)
                else Console.Write("  ▼   ");                // Down arrow (declining)
            }
            Console.WriteLine("\n");

            // Legend para sa trend symbols
            Console.WriteLine("Legend: ▲ High (>15) ● Good (10-15) ▼ Low (<10) - No data");
            Console.WriteLine();
        }

        // Method para magtanong kung gusto pang mag-add ng driver
        static bool AskToContinue()
        {
            Console.Write("Add another driver? (yes/no): ");
            string response = Console.ReadLine().ToLower(); // Convert sa lowercase
            return response == "yes" || response == "y";    // Check kung yes or y
        }

        // Method para sa multi-driver comparison report
        static void GenerateComparisonReport(string[] driverNames, decimal[] efficiencies, int count)
        {
            ShowSectionTitle("MULTI-DRIVER EFFICIENCY COMPARISON");

            // Table header
            Console.WriteLine(new string('═', 70));
            Console.WriteLine(" Rank │ Driver Name            │ Efficiency (km/PHP) │ Rating      ");
            Console.WriteLine(new string('═', 70));

            // ===== BUBBLE SORT ALGORITHM =====
            // Simple sorting para i-rank ang mga drivers base sa efficiency
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (efficiencies[j] > efficiencies[i]) // Compare efficiencies
                    {
                        // Swap efficiencies (palitan ng positions)
                        decimal tempEff = efficiencies[i];
                        efficiencies[i] = efficiencies[j];
                        efficiencies[j] = tempEff;

                        // Swap names din para mag-match
                        string tempName = driverNames[i];
                        driverNames[i] = driverNames[j];
                        driverNames[j] = tempName;
                    }
                }
            }

            // Display ng sorted drivers
            for (int i = 0; i < count; i++)
            {
                string rating = GetComparisonRating(efficiencies[i]);
                ConsoleColor color = GetEfficiencyColor(rating + " Efficiency");

                // Color-coded display
                Console.ForegroundColor = color;
                Console.WriteLine($" {i + 1,4} │ {driverNames[i],-22} │ {efficiencies[i],18:F1} │ {rating,-11}");
                Console.ResetColor();
            }

            Console.WriteLine(new string('═', 70));
            Console.WriteLine();

            // Display ng most efficient driver (ang nasa rank 1)
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"🏆 Most Efficient: {driverNames[0]} ({efficiencies[0]:F1} km/PHP)");
            Console.ResetColor();
        }

        // Method para makuha ang rating sa comparison table
        static string GetComparisonRating(decimal efficiency)
        {
            if (efficiency > 15) return "High";
            if (efficiency >= 10) return "Standard";
            return "Low";
        }

        // Method para sa program footer
        static void ShowFooter()
        {
            Console.WriteLine(new string('═', 60));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("         Thank you for using Codac Logistics Auditor!");
            Console.ResetColor();
            Console.WriteLine(new string('═', 60));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(); // Hintayin ang user input bago mag-exit
        }
    }
}
