using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SiteMapGeneratorTool.Models;
using System.Collections.Generic;
using System.Net;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// SQS Helper
    /// </summary>
    public class SQSHelper
    {
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
        /// <param name="messageBody">Object body of message</param>
        public void SendMessage(WebCrawlerRequestModel messageBody)
        {
            SendMessageResponse response = Client.SendMessageAsync(new SendMessageRequest
            {
                MessageGroupId = messageBody.Guid.ToString(),
                MessageDeduplicationId = messageBody.Guid.ToString(),
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
        public WebCrawlerRequestModel DeleteAndReceiveFirstMessage()
        {
            List<Message> messages = Client.ReceiveMessageAsync(QueueUrl).Result.Messages;
            if (messages.Count == 0)
                return default;
            else
            {
                Message message = messages[0];
                DeleteMessageResponse response = Client.DeleteMessageAsync(new DeleteMessageRequest
                {
                    QueueUrl = QueueUrl,
                    ReceiptHandle = message.ReceiptHandle
                }).Result;

                if (response.HttpStatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<WebCrawlerRequestModel>(message.Body);
                else
                    throw new AmazonServiceException($"Message could not be deleted : {response.HttpStatusCode}");
            }
        }
    }
}