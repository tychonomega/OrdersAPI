using App.Models;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>
/// Basic Controller to verify auth functionality
/// </summary>
[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("public")]
    public ActionResult<Message> GetPublicMessage()
    {
        return _messageService.GetPublicMessage();
    }

    [HttpGet("protected")]
    [Authorize]
    public ActionResult<Message> GetProtectedMessage()
    {
        return _messageService.GetProtectedMessage();
    }

    [HttpGet("admin")]
    [Authorize]
    public ActionResult<Message> GetAdminMessage()
    {
        return _messageService.GetAdminMessage();
    }
}
