using AutoMapper;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Services.Mapping;

/// <summary>Defines mappings between persistence entities and API request/response types.</summary>
public sealed class InsuranceMappingProfile : Profile
{
    /// <summary>Registers supported domain-to-contract and request-to-domain maps.</summary>
    public InsuranceMappingProfile()
    {
        CreateMap<Customer, CustomerResponse>().MaxDepth(32);
        CreateMap<CreateCustomerRequest, Customer>().MaxDepth(32);
        CreateMap<UpdateCustomerRequest, Customer>().MaxDepth(32);

        CreateMap<Policy, PolicyResponse>().MaxDepth(32);
        CreateMap<CreatePolicyRequest, Policy>().MaxDepth(32);
        CreateMap<UpdatePolicyRequest, Policy>().MaxDepth(32);

        CreateMap<Claim, ClaimResponse>().MaxDepth(32);
        CreateMap<CreateClaimRequest, Claim>().MaxDepth(32);
    }
}
