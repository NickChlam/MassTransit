using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace Ticketing.Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus;
        
        public TicketController(IBus bus)
        {
            _bus = bus;
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            var rabbitmqUri = Environment.GetEnvironmentVariable("RABBITMQ_URI");
            if (ticket !=null)
            {
                
                ticket.BookedOn = DateTime.Now;
                Uri uri = new Uri($"{rabbitmqUri}/ticketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket);
                
                return Ok();
            }
            return BadRequest();
        }
    }
}