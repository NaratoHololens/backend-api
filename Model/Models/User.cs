using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Model.Models
{

    public class User
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string ProfessionalOccupation { get; set; }
        public string Twittername { get; set; }

        private readonly string type = "USER";
        public string Type { get { return type; } }

        public User()
        {

        }

        public User(string personId, string firstname, string lastname)
        {
            Id = personId;
            Firstname = firstname;
            Lastname = lastname;
        }

        public User(string personId, string firstname, string lastname, DateTime birthday, string professionalOccupation, string twittername) : this(personId, firstname, lastname)
        {
            Birthday = birthday;
            ProfessionalOccupation = professionalOccupation;
            Twittername = twittername;
            Id = personId;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
