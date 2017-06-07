using Common.Configurations;
using Common.Exceptions;
using DataProvider.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Narato.Common.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.DataProviders
{
    public class DocumentDBProvider : IStorageProvider
    {

        private readonly DbConfiguration _dbConfiguration;


        private DocumentClient _documentClient;

        public DocumentDBProvider(IOptions<DbConfiguration> DbConfiguration)
        {
            _dbConfiguration = DbConfiguration.Value;
            _documentClient = new DocumentClient(_dbConfiguration.Uri, _dbConfiguration.Key);

            _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _dbConfiguration.DatabaseName }).Wait();
            _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_dbConfiguration.DatabaseName), new DocumentCollection { Id = _dbConfiguration.CollectionName}).Wait();

            _documentClient.OpenAsync().Wait();

        }


        //To get Uri of a Collection of type
        private Uri GetDocumentCollectionUri()
        {

            try
            {
                return UriFactory.CreateDocumentCollectionUri(_dbConfiguration.DatabaseName, _dbConfiguration.CollectionName);
            }
            catch (Exception)
            {

                throw;
            }
                    

           
        }

        //To get Uri for of a Document of type 
        private Uri GetDocumentUri(string id)
        {


            try
            {
             return UriFactory.CreateDocumentUri(_dbConfiguration.DatabaseName, _dbConfiguration.CollectionName, id);

            }
            catch (Exception e)
            {

                throw;
            }
            


        }

       
        //This will create a query, feedOptions is to set how many results will be returned in one time
        public IQueryable<T> CreateQuery<T>(FeedOptions feedOptions)
        {
            return _documentClient.CreateDocumentQuery<T>(GetDocumentCollectionUri(), feedOptions);
        }

        //This will create a query and manually add SQL expression
        public IQueryable<T> CreateQuery<T>(string sql, FeedOptions feedOptions)
        {
            return _documentClient.CreateDocumentQuery<T>(GetDocumentCollectionUri(), sql, feedOptions);
        }

        //this will delete a document form the collection of type
        public async Task DeleteDocument(string id)
        {

            try
            {
                await _documentClient.DeleteDocumentAsync(GetDocumentUri(id));
            }
            catch (Exception e )
            {

                throw new EntityNotFoundException(e.Message);
            }
                
        }

        //this will add a document to the collection of type
        public async Task<string> AddDocument<T>(T document)
        {
            var doc = await _documentClient.CreateDocumentAsync(GetDocumentCollectionUri(), document);
            return doc.Resource.Id;
        }

        //this will update a document of the collection of type
        public async Task<bool> UpdateDocument<T>(T document, string id)
        {
            try
            {
                var doc = await _documentClient.ReplaceDocumentAsync(GetDocumentUri(id), document);
                if (doc != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

    }
}
