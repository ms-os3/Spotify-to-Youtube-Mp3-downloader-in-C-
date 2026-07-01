using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Offline_Streamer
{
    public partial class Contact : Page
    {

        

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            var connStringSetting = ConfigurationManager.ConnectionStrings["OfflineStreamer"];
            var connectionString = connStringSetting.ConnectionString;
            var name = (NameText?.Text ?? string.Empty).Trim();
            var email = (EmailText?.Text ?? string.Empty).Trim();
            var message = (MessageText?.Text ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(message))
            {
                Response.Write("<div style='color:#f8d7da;background:#3a0b09;padding:8px;border-radius:6px;margin:8px 0;'>Please provide a name, email or message before sending.</div>");
                return;
            }

            

              // Parameterized insert to avoid SQL injection.
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("INSERT INTO Issues ([Name], Email, Message) VALUES (@name, @email, @message)", conn))
                {
                    // Use NVARCHAR; adjust sizes to match your schema. Message uses MAX (-1).
                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 200) { Value = (object)name ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 200) { Value = (object)email ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@message", SqlDbType.NVarChar, -1) { Value = (object)message ?? DBNull.Value });

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Response.Write("<div style='color:#d4edda;background:#083a12;padding:8px;border-radius:6px;margin:8px 0;'>Message saved. Thank you for contacting us.</div>");

                // Clear inputs after successful save.
                NameText.Text = string.Empty;
                EmailText.Text = string.Empty;
                MessageText.Text = string.Empty;
          
        }
    }
}