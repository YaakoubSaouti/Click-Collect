using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClickandCollect.DAL;

namespace ClickandCollect.Models
{
    abstract public class Person
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public Person(){}
        public Person(int id, string ln,string fn, string un,string pw)
        {
            Id = id;
            Lastname = ln;
            Firstname = fn;
            Username = un;
            Password = pw;
        }

        public Person(string ln, string fn, string un, string pw)
        {
            Lastname = ln;
            Firstname = fn;
            Username = un;
            Password = pw;
        }

        public static Person LogIn(string un,string pw,IPersonsDAL personsDAL)
        {
            return personsDAL.LogIn(un,pw);
        }
    }
}
