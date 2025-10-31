using Mensageria.Application.DTOs;
using Mensageria.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mensageria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessagePublisher _messagePublisher;

    public MessagesController(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage([FromBody] MessageInputModel input)
    {
        if (string.IsNullOrEmpty(input.Content))
        {
            return BadRequest("O conteúdo da mensagem não pode ser vazio.");
        }

        await _messagePublisher.PublishAsync(input.Content);

        return Ok("Mensagem enviada com sucesso!");
    }
}