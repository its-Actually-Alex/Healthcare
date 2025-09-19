using Library.Healthcare.Models;
using Library.Healthcare.Services;
using System;
using System.Globalization;

namespace CLI.Healthcare
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the healthcare thingy!");
            var cont = true;

            List<Patient?> patients = PatientServiceProxy.Current.Patients;
            List<Physician?> physicians = new List<Physician?>();

            do
            {
                Console.WriteLine("C. Create a New Patient Account");
                Console.WriteLine("P. Create a New Physician Account");
                Console.WriteLine("V. View Patients and Physicians");
                Console.WriteLine("F. Detailed Patient View");
                Console.WriteLine("D. Write a New Diagnosis");
                Console.WriteLine("A. Create an Appointment");
                Console.WriteLine("X. Delete a Patient Account");
                Console.WriteLine("Q. Quit");

                var userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "C":
                    case "c":
                        {
                            var patient = new Patient();
                            Console.WriteLine("Enter the patient's name (F/L): ");
                            patient.Name = Console.ReadLine();
                            Console.WriteLine("Enter the patient's address: ");
                            patient.Address = Console.ReadLine();
                            PatientServiceProxy.Current.Create(patient);
                            break;
                        }
                    case "P":
                    case "p":
                        {
                            //Create new physician object
                            var physician = new Physician();

                            //Gather data for physician from user
                            Console.WriteLine("Enter the physician's name (F/L): ");
                            physician.Name = Console.ReadLine();
                            Console.WriteLine("Enter the physician's license number: ");
                            var license = Console.ReadLine();
                            physician.License = int.Parse(license ?? "-1");
                            Console.WriteLine("Enter the physician's graduation date (MM/DD/YYYY): ");
                            physician.GraduationDate = Console.ReadLine();
                            Console.WriteLine("Provide the physician's specializations: ");
                            physician.Specialization = Console.ReadLine();

                            //Create an ID for the physician
                            var maxId = -1;
                            if (physicians.Any())
                            {
                                maxId = physicians.Select(p => p?.Id ?? -1).Max();
                            }
                            else
                            {
                                maxId = 0;
                            }
                            physician.Id = ++maxId;
                            physicians.Add(physician);
                            break;
                        }
                    case "V":
                    case "v":
                        Console.WriteLine("PATIENTS");
                        PatientServiceProxy.Current.Patients.ForEach(p => Console.WriteLine($"{p?.Id}. {p}"));
                        Console.WriteLine("\nPHYSICIANS");
                        physicians.ForEach(p => Console.WriteLine($"{p?.Id}. {p}"));
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
                                    Console.WriteLine("State patient's symptoms: ");
                                    var symp = Console.ReadLine();
                                    Console.WriteLine("State diagnosis: ");
                                    var diag = Console.ReadLine();

                                    //Create diagnosis
                                    Diagnosis currentDiag = new();
                                    currentDiag.Symptoms = symp;
                                    currentDiag.Diagnosis_Given = diag;
                                    currentDiag.PatientId = patientToDiagnose.Id;

                                    //Add a physician to diagnosis
                                    Console.WriteLine("Enter diagnosing physician's ID: ");
                                    var pSelection = Console.ReadLine();
                                    if (int.TryParse(selection ?? "-1", out int pintSelection))
                                    {
                                        Physician? diagnoser = physicians
                                            .Where(p => p != null)
                                            .FirstOrDefault(p => p?.Id == pintSelection);

                                        if (diagnoser != null)
                                            currentDiag.Physician_Diagnosed_By = diagnoser;
                                        else
                                            Console.WriteLine("Physician not found!");
                                    }

                                    //Add diagnosis to patient's account
                                    patientToDiagnose.Diagnoses.Add(currentDiag);
                                }
                                else
                                    Console.WriteLine("Patient not found!");
                            }
                            break;
                        }
                    case "A":
                    case "a":
                        {
                            Console.WriteLine("Enter appointment physician's ID: ");
                            var pSelection = Console.ReadLine();
                            if (int.TryParse(pSelection ?? "-1", out int pintSelection))
                            {
                                Physician? appPhysician = physicians
                                    .Where(p => p != null)
                                    .FirstOrDefault(p => p?.Id == pintSelection);

                                if (appPhysician != null)
                                {
                                    Console.WriteLine("Enter appointment time (mm/dd/yyyy hh:mm)");
                                    string? t = Console.ReadLine();

                                    if(DateTime.TryParseExact(t, "MM/dd/yyyy HH:mm",
                                                              CultureInfo.InvariantCulture,
                                                              DateTimeStyles.None,
                                                              out DateTime result))
                                    {
                                        foreach(Appointment a in appPhysician.Appointments)
                                        {
                                            if(a.Time == result)
                                            {
                                                Console.WriteLine("Physician already has appointment at that time.");
                                                break;
                                            }
                                        }
                                        Appointment app = new();
                                        app.Time = result;
                                        appPhysician.Appointments.Add(app);
                                    }
                                }
                                else
                                    Console.WriteLine("Physician not found!");
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
