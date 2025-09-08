using Library.Healthcare.Models;
using System;

namespace CLI.Healthcare
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the healthcare thingy!");
            var cont = true;

            List<Patient?> patients = new List<Patient?>();

            do
            {
                Console.WriteLine("C. Create a New Patient Account");
                Console.WriteLine("P. Create a New Physician Account");
                Console.WriteLine("V. View Patients");
                Console.WriteLine("F. Detailed Patient View");
                Console.WriteLine("D. Write a New Diagnosis");
                Console.WriteLine("X. Delete a Patient Account");
                Console.WriteLine("Q. Quit");

                var userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "C":
                    case "c":
                        var patient = new Patient();
                        Console.WriteLine("Enter the patient's name (F/L): ");
                        patient.Name = Console.ReadLine();
                        Console.WriteLine("Enter the patient's address: ");
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
                        patients.ForEach(p => Console.WriteLine($"{p?.Id}: {p}"));
                        break;
                    case "D":
                    case "d":
                        {
                            //Prompt the user to disagnose a patient
                            Console.WriteLine("\nCurrent Patients:\n");
                            patients.ForEach(p => Console.WriteLine($"{p?.Id}: {p}"));
                            Console.WriteLine("Patient to Diagnose (ID): ");

                            var selection = Console.ReadLine();
                            if (int.TryParse(selection ?? "-1", out int intSelection))
                            {
                                Patient? patientToDiagnose = patients
                                    .Where(p => p != null)
                                    .FirstOrDefault(p => p?.Id == intSelection);

                                if (patientToDiagnose != null)
                                {
                                    //Gather diagnosis details
                                    Console.WriteLine("State Patient's Symptoms: ");
                                    var symp = Console.ReadLine();
                                    Console.WriteLine("State Diagnosis: ");
                                    var diag = Console.ReadLine();

                                    //Create diagnosis
                                    Diagnosis currentDiag = new();
                                    currentDiag.Symptoms = symp;
                                    currentDiag.Diagnosis_Given = diag;
                                    currentDiag.PatientId = patientToDiagnose.Id;

                                    //Add diagnosis to patient's account
                                    patientToDiagnose.Diagnoses.Add(currentDiag);
                                }
                                else
                                    Console.WriteLine("Patient not found!");
                            }
                            break;
                        }
                    case "F":
                    case "f":
                        {
                            //Prompt the user to view a patient in detail
                            Console.WriteLine("\nCurrent Patients:\n");
                            patients.ForEach(p => Console.WriteLine($"{p?.Id}: {p}"));
                            Console.WriteLine("Patient to View in Full (ID): ");

                            var selection = Console.ReadLine();
                            if (int.TryParse(selection ?? "-1", out int intSelection))
                            {
                                Patient? patientToView = patients
                                    .Where(p => p != null)
                                    .FirstOrDefault(p => p?.Id == intSelection);

                                if (patientToView != null)
                                {
                                    //Print patients details with DetailedView function
                                    Console.WriteLine("\nDETAILED PATIENT VIEW:");
                                    patientToView.DetailedView();
                                }
                                else
                                    Console.WriteLine("Patient not found!");
                            }
                            break;
                        }
                    case "X":
                    case "x":
                        {
                            //Give the user options for deletion
                            patients.ForEach(Console.WriteLine);
                            Console.WriteLine("Patient to Delete (ID): ");

                            //Get user selection and convert to int
                            var selection = Console.ReadLine();
                            if (int.TryParse(selection ?? "-1", out int intSelection))
                            {
                                //Find and remove the patient
                                var patientToDelete = patients
                                    .Where(p => p != null)
                                    .FirstOrDefault(p => p.Id == intSelection);
                                patients.Remove(patientToDelete);
                            }
                            break;
                        }
                    case "Q":
                    case "q":
                        cont = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Command!");
                        break;
                }
            } while (cont);
        }
    }
}
