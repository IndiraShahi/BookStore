using CommonLayer.Models;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RepositoryLayer.Services
{
    public class OrderRL : IOrderRL
    {
        private readonly string _connectionString;
        public OrderRL(IConfiguration config)
        {
            _connectionString = config.GetSection("ConnectionStrings").GetSection("OnlineBookStore").Value;
        }

        public bool EmailOrderDetails(int UserId, int orderNumber)
        {
            string email = GetEmailFromId(UserId);
            string messagecontent = "Your order has been successfully placed. your order number is  " + orderNumber;

            MessageQueue msgqueue;
            if (MessageQueue.Exists(@".\Private$\MyQueue"))
            {
                msgqueue = new MessageQueue(@".\Private$\MyQueue");
            }
            else
            {
                msgqueue = MessageQueue.Create(@".\Private$\MyQueue");
            }

            Message message = new Message();

            message.Formatter = new BinaryMessageFormatter();
            message.Body = messagecontent;
            msgqueue.Label = "your order number";
            msgqueue.Send(message);

            var receivequeue = new MessageQueue(@".\Private$\MyQueue");
            var receivemsg = receivequeue.Receive();
            receivemsg.Formatter = new BinaryMessageFormatter();

            string link = receivemsg.Body.ToString();
            if (Sendmail(email, link))
            {
                return true;
            }
            return false;
        }

        public bool PlaceOrder(Order order, int UserId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                int rows;
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spPlaceOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@book_id", order.BookId);
                    command.Parameters.AddWithValue("@cart_id", order.CartId);
                    command.Parameters.AddWithValue("@customer_id", UserId);
                    command.Parameters.AddWithValue("@address_id", order.AddressId);
                    rows = command.ExecuteNonQuery();
                }
                return (rows > 0 ? true : false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private bool Sendmail(string email, string message)
        {
            MailMessage mailmessage = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            mailmessage.From = new MailAddress("mayur.wankhade2@gmail.com");
            mailmessage.To.Add(new MailAddress(email));
            mailmessage.Subject = "Your Order for book has been successfully placed";
            mailmessage.IsBodyHtml = true;
            mailmessage.Body = message;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new NetworkCredential("mayur.wankhade2@gmail.com", "khikhikhi");
            smtp.EnableSsl = true;
            smtp.Send(mailmessage);
            return true;
        }

        private string GetEmailFromId(int customerId)
        {
            string email = string.Empty;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spGetEmailFromId", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customer_id", customerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        email = reader["email"].ToString();
                    }
                    connection.Close();
                    return email;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}

