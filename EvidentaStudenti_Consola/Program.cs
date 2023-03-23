using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

using LibrarieModele;
using NivelStocareDate;

namespace EvidentaStudenti_Consola
{
    class Program
    {
        static void Main()
        {
            Student student = new Student();
            string numeFisier = ConfigurationManager.AppSettings["NumeFisier"];
            AdministrareStudenti_FisierText adminStudenti = new AdministrareStudenti_FisierText(numeFisier);
            int nextId = GetNextIdStudent(numeFisier);
            int nrStudenti = 0;

            string optiune;
            do
            {
                Console.WriteLine("I. Introducere informatii student");
                Console.WriteLine("A. Afisare studenti");
                Console.WriteLine("F. Afisare studenti din fisier");
                Console.WriteLine("S. Salvare student in fisier");
                Console.WriteLine("X. Inchidere program");
                Console.WriteLine("Alegeti o optiune");
                optiune = Console.ReadLine();
                switch (optiune.ToUpper())
                {
                    case "I":
                        int idStudent = nrStudenti + 1;

                        Console.WriteLine("Introdu numele studentului {0} : ", idStudent);
                        string nume = Console.ReadLine();
                        Console.WriteLine("Introdu prenumele studentului {0} : ", idStudent);
                        string prenume = Console.ReadLine();
                        student = new Student(idStudent, nume, prenume);
                        nrStudenti++;

                        break;
                    case "A":
                        string infoStudent = student.Info();
                        Console.WriteLine("Studentul {0}", infoStudent);

                        break;
                    /*case "F":
                        Student[] studenti = adminStudenti.GetStudenti(out nrStudenti);
                        AfisareStudenti(studenti, nrStudenti);

                        break;
                    case "D":
                        Student[] studenti = adminStudenti.GetStudenti(out nrStudenti);
                        AfisareStudenti(studenti, nrStudenti);
                        DeleteStudentbyID(studenti, nrStudenti);
                        break;*/
                    case "F":
                        Student[] studentiF = adminStudenti.GetStudenti(out nrStudenti);
                        AfisareStudenti(studentiF, nrStudenti);

                        break;
                    case "D":
                        Student[] studentiD = adminStudenti.GetStudenti(out nrStudenti);
                        AfisareStudenti(studentiD, nrStudenti);
                        DeleteStudentbyID(studentiD, nrStudenti);
                        Console.WriteLine("Öğrenci başarıyla silindi.");
                        break;

                    case "S":
                        idStudent = nrStudenti + 1;
                        nrStudenti++;
                        //student = new Student(idStudent, "Ioana", "Radu");
                        //adaugare student in fisier
                        adminStudenti.AddStudent(student);
                        Console.WriteLine("Studentul a fost salvat in fisierul '{0}'", numeFisier);
                        break;
                    case "X":

                        return;
                    default:
                        Console.WriteLine("Optiune inexistenta");

                        break;
                }
            } while (optiune.ToUpper() != "X");

            Console.ReadKey();
        }

        public static void AfisareStudenti(Student[] studenti, int nrStudenti)
        {
            Console.WriteLine("Studentii sunt:");
            for (int contor = 0; contor < nrStudenti; contor++)
            {
                string infoStudent = string.Format("Studentul cu id-ul #{0} are numele: {1} {2}",
                   studenti[contor].GetIdStudent(),
                   studenti[contor].GetNume() ?? " NECUNOSCUT ",
                   studenti[contor].GetPrenume() ?? " NECUNOSCUT ");

                Console.WriteLine(infoStudent);
            }
        }


        public static void DeleteStudentbyID(Student[] studenti, int nrStudenti)
        {
            Console.WriteLine("Introduceți ID-ul de student care trebuie șters:");
            int id = Convert.ToInt32(Console.ReadLine());

            bool found = false;

            for (int i = 0; i < nrStudenti; i++)
            {
                if (studenti[i].GetIdStudent() == id)
                {
                    found = true;
                    for (int j = i; j < nrStudenti - 1; j++)
                    {
                        studenti[j] = studenti[j + 1];
                    }
                    nrStudenti--;
                    Array.Resize(ref studenti, nrStudenti);
                    Console.WriteLine("Öğrenci başarıyla silindi.");
                    break;
                }
            }
            if(!found)
            {
                Console.WriteLine("Öğrenci bulunamadı.");
            }
        }


        public static int GetNextIdStudent(string numeFisier)
        {
            int nextId = 1;

            if (File.Exists(numeFisier))
            {
                using (StreamReader sr = File.OpenText(numeFisier))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');
                        int idStudent = 0;
                        if (int.TryParse(fields[0], out idStudent))
                        {
                            nextId = idStudent + 1;
                        }
                    }
                }
            }
            return nextId;
        }
    }
}
