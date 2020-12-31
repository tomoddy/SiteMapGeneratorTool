using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// SQS Helper
    /// </summary>
    public class SQSHelper
    {
        // Constants
        private const int TRIES = 100;
        private const int REST = 500;

        // Variables
        private readonly AmazonSQSClient Client;
        private readonly string QueueUrl;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="accessKey">AWS access key</param>
        /// <param name="secretKey">AWS secret key</param>
        /// <param name="serviceUrl">SQS serice url</param>
        /// <param name="queueName">SQS queue name (with .fifo)</param>
        /// <param name="accountId">AWS account id</param>
        public SQSHelper(string accessKey, string secretKey, string serviceUrl, string queueName, string accountId)
        {
            Client = new AmazonSQSClient(new BasicAWSCredentials(accessKey, secretKey), new AmazonSQSConfig { ServiceURL = serviceUrl });
            QueueUrl = Client.GetQueueUrlAsync(new GetQueueUrlRequest
            {
                QueueName = queueName,
                QueueOwnerAWSAccountId = accountId
            }).Result.QueueUrl;
        }

        /// <summary>
        /// Sends message to SQS queue
        /// </summary>
        /// <param name="guid">GUID of message</param>
        /// <param name="messageBody">Object body of message</param>
        public void SendMessage(string guid, object messageBody)
        {
            SendMessageResponse response = Client.SendMessageAsync(new SendMessageRequest
            {
                MessageGroupId = guid,
                MessageDeduplicationId = guid,
                QueueUrl = QueueUrl,
                MessageBody = JsonConvert.SerializeObject(messageBody)
            }).Result;

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new AmazonServiceException($"Message could not be sent : {response.HttpStatusCode}");
        }

        /// <summary>
        /// Delete and retireve top message from queue
        /// </summary>
        /// <returns>Object of message body</returns>
        public object DeleteAndReieveFirstMessage()
        {
            Message message = GetTopMessage();
            DeleteMessageResponse response = Client.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = QueueUrl,
                ReceiptHandle = message.ReceiptHandle
            }).Result;

            if (response.HttpStatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject(message.Body);
            else
                throw new AmazonServiceException($"Message could not be deleted : {response.HttpStatusCode}");
        }

        /// <summary>
        /// Makes multiple attempts to get top message from queue
        /// </summary>
        /// <returns>Message object</returns>
        private Message GetTopMessage()
        {
            int count = 0;
            do
            {
                try
                {
                    Console.WriteLine(count);
                    return Client.ReceiveMessageAsync(QueueUrl).Result.Messages[0];
                }
                catch (ArgumentOutOfRangeException)
                {
                    Thread.Sleep(REST);
                    count++;
                }
            } while (count < TRIES);
            throw new Exception("No messages found");
        }
    }
}
