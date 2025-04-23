using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ValuationSourceAPI.Models;

namespace ValuationSourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuationSourceController : ControllerBase
    {

        private IConfiguration _configuration;

        public ValuationSourceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet]
        [Route("GetList")]
        public JsonResult ValuationSourceList()
        {
            // Define your connection string from appsettings.json or any other place.
            string? connectionString = _configuration.GetConnectionString("DBConnection");

            // Create the query.
            string?  query = "SELECT intValuationSourceID, strValuationSourceName, intSort FROM tblValuationSource";

            // Initialize an empty list to store results.
            var valuationSources = new List<object>();

            // Use ADO.NET to execute the query and retrieve data.
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                // Read data from the reader and add to the list
                while (reader.Read())
                {
                    valuationSources.Add(new
                    {
                        ValuationSourceID = reader["intValuationSourceID"],
                        ValuationSourceName = reader["strValuationSourceName"],
                        ValuationSort = reader["intSort"]
                    });
                }
                conn.Close();
            }

            // Return the data as a JSON result
            return new JsonResult(valuationSources);
        }



[HttpGet]
    [Route("GetById/{id}")]
    public IActionResult GetValuationSourceById(int id)
    {
        ValuationSource? valuationSource = null;
        string? connectionString = _configuration.GetConnectionString("DBConnection");

        // Using System.Data.SqlClient for SQL connection
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string? query = "SELECT intValuationSourceID, strValuationSourceName, intSort, strSourceContactName, strSourceAddress1, strSourceAddress2, strSourceCity, strSourceState, strSourceZip, strSourceContactMethod1, strSourceContactMethod2, strSourceContactMethod3, sngLat, sngLong, geoLocation.STAsText() AS GeoLocation FROM tblValuationSource WHERE intValuationSourceID = @id";

            using (var command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        valuationSource = new ValuationSource
                        {
                            ValuationSourceID = reader["intValuationSourceID"] != DBNull.Value ? Convert.ToInt32(reader["intValuationSourceID"]) : 0,
                            ValuationSourceName = reader["strValuationSourceName"] != DBNull.Value ? reader["strValuationSourceName"].ToString() : string.Empty,
                            Sort = reader["intSort"] != DBNull.Value ? Convert.ToInt32(reader["intSort"]) : 0,
                            SourceContactName = reader["strSourceContactName"] != DBNull.Value ? reader["strSourceContactName"].ToString() : string.Empty,
                            SourceAddress1 = reader["strSourceAddress1"] != DBNull.Value ? reader["strSourceAddress1"].ToString() : string.Empty,
                            SourceAddress2 = reader["strSourceAddress2"] != DBNull.Value ? reader["strSourceAddress2"].ToString() : string.Empty,
                            SourceCity = reader["strSourceCity"] != DBNull.Value ? reader["strSourceCity"].ToString() : string.Empty,
                            SourceState = reader["strSourceState"] != DBNull.Value ? reader["strSourceState"].ToString() : string.Empty,
                            SourceZip = reader["strSourceZip"] != DBNull.Value ? reader["strSourceZip"].ToString() : string.Empty,
                            SourceContactMethod1 = reader["strSourceContactMethod1"] != DBNull.Value ? reader["strSourceContactMethod1"].ToString() : string.Empty,
                            SourceContactMethod2 = reader["strSourceContactMethod2"] != DBNull.Value ? reader["strSourceContactMethod2"].ToString() : string.Empty,
                            SourceContactMethod3 = reader["strSourceContactMethod3"] != DBNull.Value ? reader["strSourceContactMethod3"].ToString() : string.Empty,
                            Latitude = reader["sngLat"] != DBNull.Value ? Convert.ToSingle(reader["sngLat"]) : 0f,
                            Longitude = reader["sngLong"] != DBNull.Value ? Convert.ToSingle(reader["sngLong"]) : 0f,
                            GeoLocation = reader["GeoLocation"] != DBNull.Value ? reader["GeoLocation"].ToString() : string.Empty // Handle GeoLocation as a plain string
                        };
                    }
                }
                conn.Close();
            }
        }

        if (valuationSource != null)
        {
            return Ok(valuationSource);  // Return the populated ValuationSource object as JSON
        }
        else
        {
            return NotFound("Valuation source not found.");
        }
    }



    [HttpPut]
        [Route("Update/{id}")]
        public IActionResult UpdateValuationSource(int id, [FromBody] ValuationSource request)
        {
            // Fetch the connection string from appsettings.json
            string? connectionString = _configuration.GetConnectionString("DBConnection");

            // Define the SQL query for updating the valuation source
            string? query = "UPDATE tblValuationSource SET strValuationSourceName = @ValuationSourceName WHERE intValuationSourceID = @ValuationSourceID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create the command to execute the SQL query
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters to the SQL command
                    cmd.Parameters.AddWithValue("@ValuationSourceID", id);
                    cmd.Parameters.AddWithValue("@ValuationSourceName", request.ValuationSourceName);

                    // Execute the update query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if any rows were updated
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Valuation Source updated successfully" });
                    }
                    else
                    {
                        return NotFound(new { message = "Valuation Source not found for the provided ID" });
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle any database-related exceptions
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPost]  // Use POST for Insert (not PUT)
        [Route("Insert")]
        public IActionResult InsertValuationSource([FromBody] ValuationSource request)
        {
            // Fetch the connection string from appsettings.json
            string? connectionString = _configuration.GetConnectionString("DBConnection");

            // Define the SQL query for inserting the valuation source
            string? query = "INSERT INTO tblValuationSource (strValuationSourceName) VALUES (@ValuationSourceName);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create the command to execute the SQL query
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters to the SQL command
                    cmd.Parameters.AddWithValue("@ValuationSourceName", request.ValuationSourceName);  // This should match the parameter name in the query

                    // Execute the insert query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if any rows were inserted
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Valuation Source inserted successfully" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to insert Valuation Source" });
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle any database-related exceptions
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
