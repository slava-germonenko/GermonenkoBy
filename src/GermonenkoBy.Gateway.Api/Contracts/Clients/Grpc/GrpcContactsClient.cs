using AutoMapper;
using Grpc.Core;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Contacts.Api.Grpc;
using GermonenkoBy.Gateway.Api.Models.Contacts;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Grpc;

public class GrpcContactsClient : IContactsClient
{
    private readonly ContactsService.ContactsServiceClient _grpcClient;

    private readonly IMapper _mapper;

    public GrpcContactsClient(
        ContactsService.ContactsServiceClient grpcClient,
        IMapper mapper
    )
    {
        _grpcClient = grpcClient;
        _mapper = mapper;
    }

    public async Task<Contact> CreateContactAsync(CreateContactDto contactDto)
    {
        try
        {
            var request = _mapper.Map<CreateContactRequest>(contactDto);
            var response = await _grpcClient.CreateContactAsync(request);
            return _mapper.Map<Contact>(response);
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.InvalidArgument)
        {
            throw new CoreLogicException(e.Message, e);
        }
    }
}