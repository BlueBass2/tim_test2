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
        // Properties for BMI calculation
        public static float Height { get; set; }
        public static float Weight { get; set; }

        static void Main(string[] args)
        {
        }

		public static float CalculateBMI()
		{
			float result = 0;

			float height = Height / 100; // Convert cm to meters
			result = Weight / (height * height);
			// Remove division by zero - this was causing DivideByZeroException
			// result = result/0;

			return result;
		}

		public static void Save(string userInputFileName)
		{
			// Use cryptographically secure random number generator instead of Random
			using (var rng = RandomNumberGenerator.Create())
			{
				byte[] tokenBytes = new byte[4];
				rng.GetBytes(tokenBytes);
				int secureToken = BitConverter.ToInt32(tokenBytes, 0);
				Console.WriteLine(secureToken);
			}

			// Use Path.Combine to prevent path traversal attacks and validate input
			string baseDirectory = @"D:\some\directory";
			string fileName = Path.GetFileName(userInputFileName); // Remove any path components
			string filePath = Path.Combine(baseDirectory, fileName);
			
			// Validate that the resolved path is still within the base directory
			string fullBasePath = Path.GetFullPath(baseDirectory);
			string fullFilePath = Path.GetFullPath(filePath);
			if (!fullFilePath.StartsWith(fullBasePath))
			{
				throw new UnauthorizedAccessException("Invalid file path");
			}
			
			File.ReadAllText(filePath);
		}

		public static DataTable GetUserData(string userInput, SqlConnection connection)
		{
			DataTable dt = new DataTable();
			// Use parameterized query to prevent SQL injection
			string query = "select * from any.USERS where user_name = @username";
			
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				// Add parameter to prevent SQL injection
				command.Parameters.AddWithValue("@username", userInput);
				// 執行查詢
				using (SqlDataAdapter adapter = new SqlDataAdapter(command))
				{
					adapter.Fill(dt);
				}
			}
			
			return dt;
		}
    }
}
