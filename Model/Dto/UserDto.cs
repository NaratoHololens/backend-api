using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime Birthday { get; set; }

        public string ProfessionalOccupation { get; set; }

        public string Twittername { get; set; }

        public string PhotoString { get; set; }

        private readonly string type = "USER";
        public string Type { get { return type; } }

        public FaceRectangle faceRectangle { get; set; }

        public List<TweetDto> tweets { get; set; }

    }
}
