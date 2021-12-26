using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MOM.Core.Db;
using MOM.Core.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MOM.Core.Models.Shared
{
	public class DataTypes
	{
		public int DataTypeId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsEntity { get; set; }

		[NotMapped]
		public List<CustomObject> CustomOptions { get; set; }

		private CoreDbContext _dbContext;

		public DataTypes(CoreDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public DataTypes()
		{

		}

		public List<DataTypes> List(IConfiguration configuration)
		{
			var dataTypes = _dbContext.DataTypes.ToList();

			foreach (var dataType in dataTypes)
			{
				if (dataType.IsEntity)
				{
					dataType.CustomOptions = ListEntities(dataType.Name, configuration);
				}
			}

			return dataTypes;
		}

		public List<DataTypes> List(SqlConnection connection)
		{
			var dataTypes = _dbContext.DataTypes.ToList();

			foreach (var dataType in dataTypes)
			{
				if (dataType.IsEntity)
				{
					dataType.CustomOptions = ListEntities(dataType.Name, connection);
				}
			}

			return dataTypes;
		}

		public List<CustomObject> ListEntities(string entity, SqlConnection connection)
		{
			List<CustomObject> result = new List<CustomObject>();

			var commandText = $"SELECT * FROM {entity} WHERE IsDeleted = 0";

			//using (connection)
			//{
				SqlCommand command = new SqlCommand(commandText, connection);

				if (connection.State != System.Data.ConnectionState.Open)
					connection.Open();

				SqlDataReader reader = command.ExecuteReader();

				// Call Read before accessing data.
				while (reader.Read())
				{
					var id = reader[0].ToString();
					var name = reader["Name"].ToString();

					result.Add(new CustomObject() { Id = long.Parse(id), Name = name });
				}

				// Call Close when done reading.
				reader.Close();
			//}

			return result;
		}

		public List<CustomObject> ListEntities(string entity, IConfiguration configuration)
		{
			List<CustomObject> result = new List<CustomObject>();

			//var commandText = $"SELECT * FROM {entity}";
			var commandText = $"SELECT * FROM {entity} WHERE IsDeleted = 0";
			var connString = configuration.GetConnectionString("CoreConnection");

			using (SqlConnection connection = new SqlConnection(connString))
			{
				SqlCommand command = new SqlCommand(commandText, connection);
				connection.Open();

				SqlDataReader reader = command.ExecuteReader();

				// Call Read before accessing data.
				while (reader.Read())
				{
					var id = reader[0].ToString();
					var name = reader["Name"].ToString();

					result.Add(new CustomObject() { Id = long.Parse(id), Name = name });
				}

				// Call Close when done reading.
				reader.Close();
			}

			return result;
		}
	}
}