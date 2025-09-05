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
                        Console.WriteLine("Enter the patient's name (F/L): ");
                        patient.Name = Console.ReadLine();
                        Console.WriteLine("Enter the patient's address (F/L): ");
                        patient.Address = Console.ReadLine();
                        var maxId = -1;
                        if(patients.Any())
                        {
                            maxId = patients.Select(p => p?.Id ?? -1).Max();
                        }
                        else
                        {
                            maxId = 0;
                        }
                        patient.Id = ++maxId;
                        patients.Add(patient);
                        break;
                    case "V":
                    case "v":
                        foreach(var p in patients)
                        {
                            Console.WriteLine($"{p?.Id}: {p}");
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
