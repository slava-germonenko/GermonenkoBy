using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Contacts;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, /*Authorize,*/ Route("api/contacts")]
public class ContactsController : ControllerBaseWrapper
{
    private readonly IContactsClient _contactsClient;

    public ContactsController(IContactsClient contactsClient)
    {
        _contactsClient = contactsClient;
    }

    [HttpPost("")]
    [SwaggerOperation("Create contact.", "Creates contact using data supplied to the request body.")]
    [SwaggerResponse(200, "New contact model.", typeof(ContentResponse<Contact>), "application/json")]
    [SwaggerResponse(400, "Bad request exception.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Contact>>> CreateContactAsync(
        [FromBody, SwaggerRequestBody] CreateContactDto contactDto
    )
    {
        var contact = await _contactsClient.CreateContactAsync(contactDto);
        return OkWrapped(contact);
    }
}