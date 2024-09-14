using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorAuthentication.Models;
using FluentNHibernate.Mapping;

namespace AuthorAuthentication.Mappings
{
    public class AuthorDetailMap:ClassMap<AuthorDetail>
    {
        public AuthorDetailMap()
        {
            Table("AuthorDetails");
            Id(a => a.Id).GeneratedBy.Identity();
            Map(a => a.Street);
            Map(a => a.City);
            Map(a => a.State);
            Map(a => a.Country);
            References(b => b.Author).Column("AuthorId").Cascade.None();
        }
    }
}