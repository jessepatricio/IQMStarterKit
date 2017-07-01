using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IQMStarterKit.DAL
{

    public static class DataLayer
    {
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public static DataTable GetActivityResult(int activityId)
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_ReportByActivityId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@activityId", activityId);

                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static DataTable GetActivityPercentage()
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_StudentActivityPercentage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static DataTable GetGroupActivityPercentage()
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_GroupActivityPercentage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static DataTable GetGroupFeedbackProgramme()
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_GetProgramGroupAverage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetGroupFeedbackComments()
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_GetProgramGroupComments", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetGroupFeedbackTutor()
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_GetTutorGroupAverage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetGroupFeedbackTutorComments()
        {
            var retval = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    using (SqlCommand cmd = new SqlCommand("usp_GetTutorGroupComments", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataset);
                    }

                    return dataset.Tables[0];

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}