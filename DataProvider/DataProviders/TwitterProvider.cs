
using Common.Configurations;
using DataProvider.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;

namespace DataProvider.DataProviders
{
     public class TwitterProvider : ISocialMediaProvider
    {

        private readonly TwitterConfiguration _twitterConfiguration;

        public TwitterProvider(IOptions<TwitterConfiguration> twitterConfiguration)
        {
            _twitterConfiguration = twitterConfiguration.Value;
        }
        public List<string> GetMessages(string username)
        {
            List<string> messages = new List<string>();
            var appCreds = Auth.SetApplicationOnlyCredentials(_twitterConfiguration.ConsumerKey,_twitterConfiguration.ConsumerSecret, true);

            var userIdentifier = new UserIdentifier(username);

            var tweets = Timeline.GetUserTimeline(userIdentifier, 3);

            if (tweets != null)
            {
                foreach (var tweet in tweets)
                {
                    messages.Add(tweet.FullText);
                }
            }


            return messages;
        }

       

    }
}
