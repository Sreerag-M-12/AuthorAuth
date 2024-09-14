using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorAuthentication.Models;

namespace AuthorAuthentication.ViewModel
{
    public class AuthorBookVM
    {
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
        public string Genre { get; set; }
    }
}