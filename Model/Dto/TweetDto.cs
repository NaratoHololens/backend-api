using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto
{
    public class TweetDto
    {

        public string tweet { get; set; }


        public TweetDto(string tweet)
        {
            this.tweet = tweet;
        }
    }
}
