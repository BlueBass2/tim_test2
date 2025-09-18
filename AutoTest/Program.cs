using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Demonstrate the fixed BMI calculation
            try
            {
                Height = 175; // 175 cm
                Weight = 70;  // 70 kg
                float bmi = CalculateBMI();
                Console.WriteLine($"BMI: {bmi:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BMI calculation error: {ex.Message}");
            }

            // Demonstrate the secure file handling
            try
            {
                Save("test.txt");
                Console.WriteLine("File handling completed securely");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File handling error: {ex.Message}");
            }
        }

        // Static properties for BMI calculation
        public static float Height { get; set; }
        public static float Weight { get; set; }

        public static float CalculateBMI()
        {
            if (Height <= 0 || Weight <= 0)
            {
                throw new ArgumentException("Height and Weight must be positive values");
            }

            float heightInMeters = Height / 100;
            float result = Weight / (heightInMeters * heightInMeters);
            return result;
        }

        public static void Save(string userInputFileName)
        {
            // Validate and sanitize the filename to prevent path traversal
            if (string.IsNullOrWhiteSpace(userInputFileName))
            {
                throw new ArgumentException("Filename cannot be null or empty");
            }

            // Remove dangerous characters and prevent path traversal
            string sanitizedFileName = Path.GetFileName(userInputFileName);
            if (string.IsNullOrWhiteSpace(sanitizedFileName))
            {
                throw new ArgumentException("Invalid filename");
            }

            // Use a cryptographically secure random number generator
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int secureToken = BitConverter.ToInt32(bytes, 0);
                Console.WriteLine($"Secure token: {Math.Abs(secureToken)}");
            }

            // Use a safe base directory and combine paths securely
            string baseDirectory = @"D:\safe\directory";
            string filePath = Path.Combine(baseDirectory, sanitizedFileName);
            
            // Ensure the path is within the expected directory
            if (!filePath.StartsWith(baseDirectory))
            {
                throw new UnauthorizedAccessException("Path traversal detected");
            }

            if (File.Exists(filePath))
            {
                File.ReadAllText(filePath);
            }
        }

        public static DataTable GetUserData(string userInput, SqlConnection connection)
        {
            DataTable dt = new DataTable();
            
            // Use parameterized query to prevent SQL injection
            string query = "SELECT * FROM any.USERS WHERE user_name = @userName";
            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Add parameter to prevent SQL injection
                command.Parameters.AddWithValue("@userName", userInput ?? string.Empty);
                
                // Execute the query and populate the DataTable
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }
    }
}
