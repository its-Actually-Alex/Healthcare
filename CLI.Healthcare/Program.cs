using Library.Healthcare.Models;
using System;

namespace CLI.Healthcare
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the healthcare thingy!");

            List<Patient?> patients = new List<Patient?>();

            do
            {
                Console.WriteLine("C. Create a New Patient Account");
                Console.WriteLine("V. View Patients");
                Console.WriteLine("D. Delete a Patient Account");
                Console.WriteLine("Q. Quit");

                var userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "C":
                    case "c":
                        var patient = new Patient();
                        patient.Name = Console.ReadLine();
                        patient.Address = Console.ReadLine();
                        patients.Add(patient);
                        break;
                    case "R":
                    case "r":
                        foreach(var p in patients)
                        {
                            Console.WriteLine($"({p?.Length}) {p}");
                        }
                        break;
                    case "D":
                    case "d":
                        break;
                    case "Q":
                    case "q":
                        break;
                    default:
                        Console.WriteLine("Invalid Command!");
                        break;
                }
            } while (true);
        }
    }
}
