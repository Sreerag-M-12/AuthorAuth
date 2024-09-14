using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorAuthentication.Models;
using FluentNHibernate.Mapping;

namespace AuthorAuthentication.Mappings
{
    public class BookMap:ClassMap<Book>
    {
        public BookMap()
        {
            Table("Books");
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Title);
            Map(b => b.Genre);
            Map(b => b.Description);
            References(b => b.Author).Columns("authorId").Cascade.None();

        }

    }
}