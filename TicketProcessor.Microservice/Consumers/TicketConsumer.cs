using System;
using System.Threading.Tasks;
using MassTransit;
using Models;


namespace TicketProcessor.Microservice.Consumers
{
    public class TicketConsumer : IConsumer<Ticket>
    {
        public async Task Consume(ConsumeContext<Ticket> context)
        {
            var data = context.Message;
            var time = DateTime.Now;
            Console.WriteLine("hello");
        
            Console.WriteLine(value: $"{data.UserName} on flight to {data.Destination}: has been processed at {time}");
            // validate 
            // store to database
            // notify user 
        }

        
    }
}