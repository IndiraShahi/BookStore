using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Services
{
    public class BookRL : IBookRL
    {
        private string connectionString;
        public BookRL(IConfiguration configuration)
        {
            
            connectionString = configuration.GetSection("ConnectionStrings").GetSection("OnlineBookStore").Value;
        }

        public List<Book> GetAllBooks()
        {
                List<Book> bookList = new List<Book>();
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spGetAllBooks", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader sqlreader = command.ExecuteReader();
                    while (sqlreader.Read())
                    {
                        Book book = new Book();
                        book.BookId = Convert.ToInt32(sqlreader["bookId"]);
                        book.BookName = sqlreader["bookName"].ToString();
                        book.Author = sqlreader["author"].ToString();
                        book.Price = Convert.ToDouble(sqlreader["price"]);
                        book.Quantity = Convert.ToInt32(sqlreader["quantity"]);
                        book.Description = sqlreader["description"].ToString();
                        book.Image = sqlreader["image"].ToString();
                        book.Rating = Convert.ToDouble(sqlreader["rating"]);
                        bookList.Add(book);
                    }
                    return bookList;
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

