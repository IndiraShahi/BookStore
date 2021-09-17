using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class BookBL : IBookBL
    {
        private readonly IBookRL bookRL;
        public BookBL(IBookRL bookRL)
        {
            this.bookRL = bookRL;
        }

        public List<Book> GetAllBooks()
        {
            var allbooks = bookRL.GetAllBooks();
            if (allbooks.Count != 0)
            {
                return allbooks;
            }
            return null;
        }
    }
}

