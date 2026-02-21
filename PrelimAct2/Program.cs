﻿/*
    Prelim Activity 02: Advanced Object-Oriented Programming and Robustness
    ---------------------------------------------------
    Name: Israel Andy T. Maliwat

    Task Description: The Clean‑Water Treatment Plant simulation is a C# console app built to demonstrate the four pillars of OOP. A base class WaterFilter defines shared members, while CarbonFilter and ChemicalFilter inherit and override methods to show inheritance, abstraction, and polymorphism. Encapsulation is enforced through private fields and validated properties, and robustness is achieved with try‑catch blocks that handle divide‑by‑zero, invalid input, and unexpected errors, ensuring safe and graceful program execution.

*/


using System;

namespace Maliwat_CleanWater
{
    abstract class WaterFilter
    {
        private int filterID;
        private int usageCount;

        public int FilterID => filterID;

        public int UsageCount
        {
            get => usageCount;
            set
            {
                if (value < 0) throw new ArgumentException("UsageCount cannot be negative.");
                usageCount = value;
            }
        }

        public WaterFilter(int id)
        {
            filterID = id;
            usageCount = 0;
        }

        public abstract void ProcessWater();

        public virtual double CalculateEfficiency()
        {
            try
            {
                return 100.0 / usageCount;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error: No water processed yet. Efficiency = 0.");
                return 0;
            }
        }
    }

    class CarbonFilter : WaterFilter
    {
        private int debrisLevel;

        public int DebrisLevel
        {
            get => debrisLevel;
            set
            {
                if (value < 0) throw new ArgumentException("Debris level cannot be negative.");
                debrisLevel = value;
            }
        }

        public CarbonFilter(int id) : base(id) { }

        public override void ProcessWater()
        {
            UsageCount++;
            DebrisLevel = Math.Max(0, DebrisLevel - 10);
            Console.WriteLine("Carbon filter processed water. Debris reduced.");
        }

        public override double CalculateEfficiency()
        {
            return base.CalculateEfficiency() + (100 - DebrisLevel);
        }
    }

    class ChemicalFilter : WaterFilter
    {
        private int chemicalLevel;

        public int ChemicalLevel
        {
            get => chemicalLevel;
            set
            {
                if (value < 0) throw new ArgumentException("Chemical level cannot be negative.");
                chemicalLevel = value;
            }
        }

        public ChemicalFilter(int id) : base(id) { }

        public override void ProcessWater()
        {
            UsageCount++;
            ChemicalLevel = Math.Max(0, ChemicalLevel - 5);
            Console.WriteLine("Chemical filter processed water. Chemicals reduced.");
        }

        public override double CalculateEfficiency()
        {
            return base.CalculateEfficiency() + (100 - ChemicalLevel);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== Clean-Water Treatment Plant Simulation ===");
                Console.WriteLine("Input Guidelines:");
                Console.WriteLine("1 → Carbon Filter (tracks debris)");
                Console.WriteLine("2 → Chemical Filter (tracks chemical levels)");
                Console.WriteLine("Enter only positive integers for levels.");
                Console.WriteLine("------------------------------------------------");

                string continueChoice = "y";

                while (continueChoice == "y")
                {
                    WaterFilter filter = null;

                    // Choose filter type each time
                    while (filter == null)
                    {
                        Console.Write("Choose filter type (1=Carbon, 2=Chemical): ");
                        string choiceInput = Console.ReadLine();

                        if (choiceInput == "1")
                        {
                            int debris = GetValidInt("Enter initial debris level (≥0): ");
                            filter = new CarbonFilter(1) { DebrisLevel = debris };
                        }
                        else if (choiceInput == "2")
                        {
                            int chem = GetValidInt("Enter initial chemical level (≥0): ");
                            filter = new ChemicalFilter(2) { ChemicalLevel = chem };
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                        }
                    }

                    // Process water and show efficiency
                    filter.ProcessWater();
                    Console.WriteLine($"Efficiency: {filter.CalculateEfficiency()}");

                    // Ask if user wants to continue
                    Console.Write("Do you want to process more water? (y/n): ");
                    continueChoice = Console.ReadLine()?.ToLower();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("System Shutdown");
            }
        }

        static int GetValidInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out value) && value >= 0)
                {
                    return value;
                }
                Console.WriteLine("Invalid input. Please enter a non-negative integer.");
            }
        }
    }
}