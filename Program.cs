using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentGradeSystem
{
    struct Student
    {
        public string Id;
        public string Name;
    }

    struct GradeEntry
    {
        public string StudentId;
        public string Subject;
        public int Grade;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Student[] school = new Student[30];
            int studentCount = 0;
            List<GradeEntry> grades = new List<GradeEntry>();

            while (true)
            {
                Console.WriteLine("\n=> Student Grade Management System <=");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Add Grade");
                Console.WriteLine("3. Query all grades of a student");
                Console.WriteLine("4. Query all students' grades in a subject");
                Console.WriteLine("5. Query average, highest, and lowest grade in a subject");
                Console.WriteLine("6. Print number of students in each subject");
                Console.WriteLine("0. Exit");

                Console.Write("Please select an option: ");
                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    break;
                }
                else if (choice == "1")
                {
                    AddStudent(school, ref studentCount);
                }
                else if (choice == "2")
                {
                    AddGrade(school, studentCount, grades);
                }
                else if (choice == "3")
                {
                    QueryStudentGrades(grades);
                }
                else if (choice == "4")
                {
                    QuerySubjectGrades(grades);
                }
                else if (choice == "5")
                {
                    QuerySubjectStats(grades);
                }
                else if (choice == "6")
                {
                    PrintStudentsPerSubject(grades);
                }
                else
                {
                    Console.WriteLine("Invalid option! Please enter a number between 0 and 6.");
                }
            }
        }

        static void AddStudent(Student[] school, ref int count)
        {
            if (count >= 30)
            {
                Console.WriteLine("Maximum capacity of 30 students reached.");
                return;
            }

            string id = GetValidId();

            if (school.Take(count).Any(s => s.Id == id))
            {
                Console.WriteLine($"Student with ID {id} already exists. Cannot add.");
                return;
            }

            string name = GetValidName();

            school[count] = new Student { Id = id, Name = name };
            count++;
            Console.WriteLine("Student successfully added.");
        }

        // --- FEATURE 2: Add Grade ---
        static void AddGrade(Student[] school, int count, List<GradeEntry> grades)
        {
            string id = GetValidId();

            if (!school.Take(count).Any(s => s.Id == id))
            {
                Console.WriteLine($"Student ID {id} not found in the system. Please add the student first.");
                return;
            }

            string subject = GetValidSubject();

            if (grades.Any(g => g.StudentId == id && g.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"A grade for '{subject}' already exists for student {id}. Cannot add duplicate.");
                return;
            }

            int grade = GetValidGrade();

            grades.Add(new GradeEntry { StudentId = id, Subject = subject, Grade = grade });
            Console.WriteLine("Grade successfully added.");
        }

        // --- FEATURE 3: Query Student's Grades ---
        static void QueryStudentGrades(List<GradeEntry> grades)
        {
            string id = GetValidId();
             
            var studentGrades = grades.Where(g => g.StudentId == id).ToList();

            if (!studentGrades.Any())
            {
                Console.WriteLine("This student currently has no grade records.");
                return;
            }

            foreach (var record in studentGrades)
            {
                Console.WriteLine($"Subject: {record.Subject}, Grade: {record.Grade}");
            }
        }

        // --- FEATURE 4: Query All Students in a Subject ---
        static void QuerySubjectGrades(List<GradeEntry> grades)
        {
            string subject = GetValidSubject();

            var subjectGrades = grades.Where(g => g.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!subjectGrades.Any())
            {
                Console.WriteLine($"No grades found for subject: {subject}.");
                return;
            }

            foreach (var record in subjectGrades)
            {
                Console.WriteLine($"Student ID: {record.StudentId}, Grade: {record.Grade}");
            }
        }

        // --- FEATURE 5: Query Subject Statistics (Avg, Max, Min) ---
        static void QuerySubjectStats(List<GradeEntry> grades)
        {
            string subject = GetValidSubject();

            var subjectGrades = grades.Where(g => g.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!subjectGrades.Any())
            {
                Console.WriteLine($"No grades found for subject: {subject}.");
                return;
            }

            
            double average = subjectGrades.Average(g => g.Grade);
            int highest = subjectGrades.Max(g => g.Grade);
            int lowest = subjectGrades.Min(g => g.Grade);

            Console.WriteLine($"Course: {subject}");
            Console.WriteLine($"Average score: {average:F2}");
            Console.WriteLine($"Highest score: {highest}");
            Console.WriteLine($"Lowest score: {lowest}");
        }

        // --- FEATURE 6: Print Students Count Per Subject ---
        static void PrintStudentsPerSubject(List<GradeEntry> grades)
        {
            if (!grades.Any())
            {
                Console.WriteLine("No grade records exist in the system yet.");
                return;
            }

            // LINQ: Group by subject and count items in each group
            var subjectCounts = grades.GroupBy(g => g.Subject)
                                      .Select(group => new
                                      {
                                          Subject = group.Key,
                                          Count = group.Count()
                                      });

            foreach (var item in subjectCounts)
            {
                Console.WriteLine($"Course: {item.Subject}, Number of students: {item.Count}");
            }
        }


        static string GetValidId()
        {
            while (true)
            {
                Console.Write("Please enter Student ID (exactly 4 digits): ");
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^\d{3}$"))
                {
                    return input;
                }
                Console.WriteLine("Invalid ID. It must contain exactly 4 numeric digits.");
            }
        }

        static string GetValidName()
        {
            while (true)
            {
                Console.Write("Please enter Student Name (letters only): ");
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
                {
                    return input;
                }
                Console.WriteLine("Invalid Name. It must only contain letters and spaces.");
            }
        }

        static string GetValidSubject()
        {
            while (true)
            {
                Console.Write("Please enter Subject Name: ");
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.WriteLine("Subject Name cannot be empty.");
            }
        }

        static int GetValidGrade()
        {
            while (true)
            {
                Console.Write("Please enter Grade (0 - 109): ");
                string input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out int grade) && grade >= 0 && grade <= 109)
                {
                    return grade;
                }
                Console.WriteLine("Invalid Grade. It must be an integer between 0 and 109.");
            }
        }
    }
}