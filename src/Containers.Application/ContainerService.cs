using Containers.Models;
using Microsoft.Data.SqlClient;

namespace Containers.Application;

public class ContainerService : IContainerServiceRepository
{
    private readonly string _connectionString;

    public ContainerService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Container> GetAllContainers()
    {
        var containers = new List<Container>();
        var query = "SELECT * FROM Containers";

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(query, connection);

            connection.Open();

            var reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        var container = new Container
                        {
                            ID = reader.GetInt32(0),
                            ContainerTypeId = reader.GetInt32(1),
                            IsHazardious = reader.GetBoolean(2),
                            Name = reader.GetString(3)
                        };
                        containers.Add(container);
                    }
            }
            finally
            {
                reader.Close();
            }
        }

        return containers;
    }

    public bool Create(Container container)
    {
        var insertQuery =
            "INSERT INTO Containers(ContainerTypeId, IsHazardious, Name) VALUES (@containerTypeId, @isHazardious, @name)";
        var countRowsAdd = -1;

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@containerTypeId", container.ContainerTypeId);
            command.Parameters.AddWithValue("@isHazardious", container.IsHazardious);
            command.Parameters.AddWithValue("@name", container.Name);

            connection.Open();
            countRowsAdd = command.ExecuteNonQuery();
        }

        return countRowsAdd != -1;
    }

    public bool Delete(int id)
    {
        var deleteQuery = "DELETE FROM Containers WHERE ID = @id";
        int countRowsDelete = 0;
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@id", id);
            
            connection.Open();
            countRowsDelete = command.ExecuteNonQuery();
        }
        return countRowsDelete > 0;
    }
}